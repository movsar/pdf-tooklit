using CornerstoneApiServices.Models.Reporting;
using CornerstoneApiServices.Services;
using CustomTranscript.App.Enum;
using CustomTranscript.App.Helpers;
using CustomTranscript.App.Models;
using Rest.Data.Enums;

namespace CustomTranscript.App.Services
{
    public class SnowflakeReportingService
    {
        private readonly SnowflakeApiService _snowflakeApi;
        private readonly LearningApiService _learningApi;
        private readonly LearningSessionCache _sessionCache;

        public SnowflakeReportingService(SnowflakeApiService snowflakeApiService,
            LearningApiService learningApi, LearningSessionCache sessionCache)
        {
            _snowflakeApi = snowflakeApiService;
            _learningApi = learningApi;
            _sessionCache = sessionCache;
        }

        public async Task<List<ReportRow>> GetReportDataAsync(int userId, string externalId, DateTime dateFrom, DateTime dateTo)
        {
            // Get transcripts by Transcript -> CompletionDate
            var transcripts = await _snowflakeApi.GetTranscripts(userId, (int)TranscriptStatus.Completed, dateFrom, dateTo);

            // Get associated learning objects
            var learningObjects = await _snowflakeApi.ListLearningObjects(transcripts.Select(t => t.TranscObjectId)!);

            // Filter out transcripts with unnecessary course types
            learningObjects.RemoveAll(l => !ReportingHelper.AllowedLoTypes.Any(type => type.Equals(l.LoType)));
            transcripts.RemoveAll(t => !learningObjects.Any(lo => lo.ObjectId == t.TranscObjectId));

            // Remove external training transcripts from the result to handle them more accurately later
            transcripts.RemoveAll(t => learningObjects.Any(lo => lo.ObjectId == t.TranscObjectId && lo.LoTypeId == (int)LOTypeEnum.ExternalTraining));

            // Get external training transcripts by LO -> EndDate
            var externalTrainingsByEndDate = await _snowflakeApi.ListLearningObjectsByEndDate(LOTypeEnum.ExternalTraining, dateFrom, dateTo, true);
            var externalTrainingIds = externalTrainingsByEndDate?.Select(t => t.ObjectId)?.ToArray();
            if (externalTrainingIds?.Length > 0)
            {
                // Get the external trainings based on LOIDs
                var externalTraningTranscripts = await _snowflakeApi.GetTranscripts(userId, (int)TranscriptStatus.Completed, externalTrainingIds!, true);

                // Combine both results
                learningObjects = learningObjects.Concat(externalTrainingsByEndDate)
                    .DistinctBy(lo => lo.ObjectId)
                    .ToList();
                transcripts = transcripts.Concat(externalTraningTranscripts).ToList();
            }

            if (!transcripts.Any())
            {
                throw new Exception("User has no completions for the date range selected.");
            }

            // Request sessions to get ILT instructor field value when applicable
            var sessions = await _sessionCache.GetOrAddAsync(externalId, async () => await _learningApi.GetSessions(externalId, "Completed"), TimeSpan.FromMinutes(30));

            // Extract learning objects from the selected transcripts
            var loIds = transcripts.Select(t => t.TranscObjectId).ToList();

            // All transcripts' custom fields
            var transcriptsCustomFields = await _snowflakeApi.GetTranscriptCustomFields<PilotRptTranscriptCfs>(loIds, userId);

            // All learning objects' custom fields, to use for find author field's value
            var losCustomFields = await _snowflakeApi.ListLearningObjectCustomFields<PilotRptTrainingCfs>(loIds);

            // Get external training type display values
            var cfDisplayValues = await _snowflakeApi.GetCustomFieldDisplayValues(11);
            var transcObjects = transcripts.Select(t => t.TranscObjectId).ToList();
            var reportData = new List<ReportRow>();
            for (int i = 0; i < transcripts.Count; i++)
            {
                RptTranscript? transcript = transcripts[i];
                var lo = learningObjects.First(lo => lo.ObjectId == transcript.TranscObjectId);
                var loCustomFields = losCustomFields.FirstOrDefault(cf => cf.CfLearningObjectId == lo.LoObjectId);
                var session = sessions.FirstOrDefault(s => s.LoId == lo.LoObjectId);

                try
                {
                    var row = await ReportingHelper.CreateReportRow(externalId, transcript, lo, loCustomFields, transcriptsCustomFields, session, cfDisplayValues, _learningApi);
                    reportData.Add(row);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return reportData.OrderByDescending(rd => rd.DateCompleted).ToList();
        }
    }
}
