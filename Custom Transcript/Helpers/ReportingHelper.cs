using CornerstoneApiServices.Models.Learning;
using CornerstoneApiServices.Models.Reporting;
using CornerstoneApiServices.Services;
using CustomTranscript.App.Models;
using System.Text.RegularExpressions;

namespace CustomTranscript.App.Helpers
{
    internal static class ReportingHelper
    {
        internal static string[] AllowedLoTypes = {
            "Online Class", "Session", "External Training",
            "Test", "Video", "Material"
        };
        internal static async Task<ReportRow> CreateReportRow(string externalId, 
            RptTranscript transcript, 
            RptLearningObject lo, PilotRptTrainingCfs loCustomFields, 
            List<PilotRptTranscriptCfs> transcriptsCustomFields, 
            LrnSession session, List<CustomFieldDisplayValue> displayValues, LearningApiService learningApi)
        {
            var transcriptCustomFields = transcriptsCustomFields.FirstOrDefault(cf => cf.LearningObjectId == lo.LoObjectId);
            var row = new ReportRow();

            row.DeliveryType = GetDeliveryType(transcriptCustomFields, lo, displayValues);
            row.ProviderName = GetProviderName(lo);
            row.CourseName = GetCourseName(lo, session);
            row.DateCompleted = GetCompletionDate(transcript, lo);
            row.Author = GetAuthorName(transcriptCustomFields, lo, loCustomFields, session);

            var rptTranscriptCustomFields = transcriptsCustomFields.FirstOrDefault(cfs => cfs.LearningObjectId == lo.LoObjectId);
            if (rptTranscriptCustomFields != null)
            {
                row.CEUs = GetDouble(rptTranscriptCustomFields.CreditCEU);
                row.CPDs = GetDouble(rptTranscriptCustomFields.CreditCPD);
                row.LUs = GetDouble(rptTranscriptCustomFields.CreditLU);
                row.PDHs = GetDouble(rptTranscriptCustomFields.CreditPDH);
                row.HSWs = GetDouble(rptTranscriptCustomFields.CreditLUHSW);
            }
            else
            {
                // Try with Learning API to get the data
                var customFields = await learningApi.GetTranscriptCustomFields(lo.LoObjectId, externalId);
                if (customFields == null)
                {
                    throw new Exception($"Custom fields for loId: {lo.LoObjectId}, externalId: {externalId} returned null");
                }

                var lrnTranscriptCustomFields = new PilotLrnTranscriptCustomFieldsDto(customFields);
                row.CEUs = lrnTranscriptCustomFields.CreditCEU;
                row.CPDs = lrnTranscriptCustomFields.CreditCPD;
                row.LUs = lrnTranscriptCustomFields.CreditLU;
                row.PDHs = lrnTranscriptCustomFields.CreditPDH;
                row.HSWs = lrnTranscriptCustomFields.CreditLUHSW;
            }

            return row;
        }

        private static DateTime? GetCompletionDate(RptTranscript transcript, RptLearningObject lo)
        {
            DateTime? completionDate = transcript.UserLoCompDt;
            if (lo.LoType == "External Training" && !string.IsNullOrWhiteSpace(lo.LoEndDt))
            {
                completionDate = DateTime.Parse(lo.LoEndDt);
            }

            return completionDate;
        }
        private static string GetProviderName(RptLearningObject lo)
        {
            if (lo.LoType == "External Training")
            {
                return !string.IsNullOrWhiteSpace(lo.LoProvider) ? lo.LoProvider : Constants.NO_DATA;
            }
            else
            {
                return "HDR";
            }
        }
        private static string GetAuthorName(PilotRptTranscriptCfs? transcriptCustomFields, RptLearningObject lo, PilotRptTrainingCfs? loCustomFields, LrnSession? session)
        {
            string? author = null;

            if (lo.LoType == "External Training")
            {
                author = transcriptCustomFields?.Instructor;
            }
            else if (lo.LoType == "Session" && session != null && !string.IsNullOrWhiteSpace(session.InstructorName))
            {
                author = session.InstructorName;
            }
            else if (loCustomFields != null && !string.IsNullOrWhiteSpace(loCustomFields.ContentOwnerName))
            {
                author = loCustomFields.ContentOwnerName;
            }

            return !string.IsNullOrWhiteSpace(author) ? author : Constants.NO_DATA;
        }
        private static string GetDeliveryType(PilotRptTranscriptCfs? transcriptCustomFields, RptLearningObject lo, List<CustomFieldDisplayValue> displayValues)
        {
            var deliveryType = lo.LoType;
            if (lo.LoType == "External Training")
            {
                deliveryType = displayValues.FirstOrDefault(cf => cf.ValueId == transcriptCustomFields?.ExternalTrainingType)?.Title;
            }

            return !string.IsNullOrWhiteSpace(deliveryType) ? deliveryType : Constants.NO_DATA;
        }
        private static string GetCourseName(RptLearningObject lo, LrnSession? session)
        {
            var courseName = lo.LoTitle;
            if (lo.LoType == "Session")
            {
                courseName = lo.LoRef ?? lo.LoTitle;
            }

            return !string.IsNullOrWhiteSpace(courseName) ? courseName : Constants.NO_DATA;
        }

        // Extracts timezone name and adjusted current date and time based on the user's timezone string
        internal static KeyValuePair<string, DateTimeOffset> GetDateAndTimeZone(string? userTimeZone)
        {
            var defaultResult = new KeyValuePair<string, DateTimeOffset>("Central Standard Time", DateTimeOffset.Now);
            if (string.IsNullOrWhiteSpace(userTimeZone))
            {
                return defaultResult;
            }

            string pattern = @"\((UTC(?<sign>[+-])(?<hours>\d{2}):(?<minutes>\d{2}))\)";
            Match match = Regex.Match(userTimeZone, pattern);
            if (!match.Success)
            {
                return defaultResult;
            }

            try
            {
                // Extract timezone offset
                int hours = int.Parse(match.Groups["hours"].Value);
                int minutes = int.Parse(match.Groups["minutes"].Value);
                string sign = match.Groups["sign"].Value;

                // Create a timezone offset based on the extracted values
                TimeSpan baseOffset = new TimeSpan(hours, minutes, 0);
                if (sign == "-")
                {
                    baseOffset = -baseOffset;
                }

                // Get the timezone based on the timezone ID
                string? timezoneId = TimeZoneInfo.GetSystemTimeZones()
                    .FirstOrDefault(tz => tz.BaseUtcOffset == baseOffset
                    && (tz.DisplayName.Contains(userTimeZone) || tz.StandardName.Contains(userTimeZone)))?.Id;

                if (timezoneId == null)
                {
                    return defaultResult;
                }

                // Current date and time in UTC
                DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

                // Get the timezone info
                TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

                // Adjust datetime based on the timezone offset and daylight saving time
                DateTimeOffset adjustedDateTime = TimeZoneInfo.ConvertTime(currentDateTime, timezone);

                // Extract timezone name
                string timezoneName = userTimeZone.Substring(userTimeZone.IndexOf(')') + 1).Trim();

                return new KeyValuePair<string, DateTimeOffset>(timezoneName, adjustedDateTime);
            }
            catch (Exception ex)
            {
                return defaultResult;
            }
        }
        // Parses a string to a double, returning 0 for null, empty, whitespace, or "n/a" values
        internal static double GetDouble(string? customFieldValue)
        {
            if (string.IsNullOrWhiteSpace(customFieldValue) || customFieldValue.ToLower() == "n/a")
            {
                return 0;
            }
            else
            {
                var isDouble = double.TryParse(customFieldValue, out var result);
                return isDouble ? result : 0;
            }
        }
        // Parses a string to a nullable boolean, returning false for null, empty, or whitespace values
        internal static bool? GetBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.ToLower();
            if (value == "true" || value == "yes" || value == "1")
            {
                return true;
            }

            return false;
        }
    }
}
