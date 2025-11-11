using CornerstoneApiServices.Interfaces;
using CornerstoneApiServices.Models.Reporting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Rest.Data.Enums;
using System.Linq;

namespace CornerstoneApiServices.Services
{
    public class ReportingApiService
    {
        const int GroupTypeOu = 128;
        private readonly HttpService _httpService;
        private readonly ILogger<ReportingApiService> _logger;

        public ReportingApiService(HttpService httpService, ILogger<ReportingApiService> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        #region Learning Objects
        private const int BatchSize = 20;


        public async Task<List<T>> GetTranscriptCustomFields<T>(IEnumerable<string> learningObjectIds, int userId) where T : class
        {
            return await FetchDataByChunks<T>("vw_rpt_transcript_cf", learningObjectIds.Select(id => $"transc_cf_object_id eq {id}"), $"transc_cf_user_id eq {userId}");
        }

        public async Task<List<CustomFieldDisplayValue>> GetCustomFieldDisplayValues(int fieldId, int cultureId = 1)
        {
            var results = await QueryReportingApi("vw_rpt_lo_form_cf_display_value_local", $"cfvl_field_id eq {fieldId} and culture_id eq {cultureId}");
            var cfDisplayValues = results.ToObject<List<CustomFieldDisplayValue>>()!;
            return cfDisplayValues;
        }
        public async Task<List<RptTranscript>> GetTranscripts(int userId, int statusId, string[] loIds, bool getAll)
        {
            var staticClause = $"transc_user_id eq {userId} and user_lo_status_id eq {statusId}";

            var transcripts = await FetchDataByChunks<RptTranscript>("vw_rpt_transcript", loIds.Select(id => $"transc_object_id eq {id}"), staticClause);

            return transcripts;
        }
        public async Task<List<RptTranscript>> GetTranscripts(int userId, int statusId, bool getAll = true)
        {
            var filter = $"transc_user_id eq {userId}" +
                $" and user_lo_status_id eq {statusId}";

            var results = await QueryReportingApi("vw_rpt_transcript", filter, getAll);
            var transcripts = results.ToObject<List<RptTranscript>>()!;

            return transcripts;
        }
        public async Task<List<RptTranscript>> GetTranscripts(int userId, int statusId, DateTime dateFrom, DateTime dateTo, bool getAll = true)
        {
            var fromDateString = dateFrom.ToString("u").Replace(' ', 'T');
            var toDateString = dateTo.ToString("u").Replace(' ', 'T');

            var filter = $"transc_user_id eq {userId}" +
                $" and user_lo_status_id eq {statusId}" +
                $" and user_lo_comp_dt gt {fromDateString} and user_lo_comp_dt lt {toDateString}";

            var results = await QueryReportingApi("vw_rpt_transcript", filter, getAll);
            var transcripts = results.ToObject<List<RptTranscript>>()!;

            return transcripts;
        }
        public async Task<List<T>> ListLearningObjectCustomFields<T>(IEnumerable<string> learningObjectIds) where T : class
        {
            return await FetchDataByChunks<T>("vw_rpt_training_cf", learningObjectIds.Select(id => $"train_cf_object_id eq {id}"));
        }
        public async Task<List<RptLearningObject>> ListLearningObjectsByEndDate(LOTypeEnum loType, DateTime dateFrom, DateTime dateTo, bool getAll)
        {
            var fromDateString = dateFrom.ToString("u").Replace(' ', 'T');
            var toDateString = dateTo.ToString("u").Replace(' ', 'T');

            var filter = $"lo_type_id eq {(int)loType}" +
                $" and lo_end_dt gt {fromDateString} and lo_end_dt lt {toDateString}";

            var results = await QueryReportingApi("vw_rpt_training", filter, getAll);
            var learningObjects = results.ToObject<List<RptLearningObject>>()!;

            return learningObjects;
        }
        public async Task<List<RptLearningObject>> ListLearningObjects(IEnumerable<string> learningObjectIds)
        {
            return await FetchDataByChunks<RptLearningObject>("vw_rpt_training", learningObjectIds.Select(id => $"object_id eq {id}"));
        }
        #endregion

        #region Organization Units
        public async Task<List<Group>> ListGroupsByOuIds(IEnumerable<int> groupOuIds)
        {
            return await FetchDataByChunks<Group>("vw_rpt_user_groups", groupOuIds.Select(id => $"group__ou_id eq {id}"));
        }

        public async Task<List<int>> ListUserIdsByGroupReference(string ouGroupReference)
        {
            var ous = await FindOrganizationUnitsByReference(ouGroupReference);
            var allOus = await ListGroupOusRecursively(ous);

            var groups = await ListGroupsByOuIds(allOus.Select(o => o.OuId));
            var usersIds = groups.Select(g => g.UserId);

            return usersIds.ToList();
        }
        public async Task<List<Group>> ListAllGroups()
        {
            var results = await QueryReportingApi("vw_rpt_user_groups");
            return results.ToObject<List<Group>>()!;
        }


        List<OrganizationUnit> allGroups = new List<OrganizationUnit>();
        public async Task<List<OrganizationUnit>> ListGroupOusRecursively(List<OrganizationUnit> groups)
        {
            allGroups.AddRange(groups);

            for (int i = 0; i < groups.Count; i++)
            {
                List<OrganizationUnit> children = await ListOuSubgroups(groups[i].OuId);
                await ListGroupOusRecursively(children);
            }

            return allGroups;
        }
        public async Task<List<OrganizationUnit>> ListOuSubgroups(int parentId)
        {
            var reportingApiResponse = await QueryReportingApi("vw_rpt_ou", $"parent_id eq {parentId} and type_id eq {GroupTypeOu}");
            var orgUnits = reportingApiResponse.ToObject<List<OrganizationUnit>>();

            return orgUnits!;
        }

        public async Task<List<OrganizationUnit>> FindOrganizationUnitsByReference(string reference)
        {
            var reportingApiQueryResult = await QueryReportingApi("vw_rpt_ou", $"ref eq '{reference}'");
            var orgUnits = reportingApiQueryResult.ToObject<List<OrganizationUnit>>();
            return orgUnits!;
        }
        #endregion

        #region Internal Service Methods
        private async Task<List<T>> FetchDataByChunks<T>(string viewName, IEnumerable<string> queryParts, string? staticQuery = null, string queryPartsOperator = "or") where T : class
        {
            List<T> allResults = new List<T>();

            var batches = queryParts.Select((part, index) => new { part, index })
                                     .GroupBy(x => x.index / BatchSize)
                                     .Select(group => group.Select(x => x.part));

            foreach (var batch in batches)
            {
                string filters = string.Empty;
                if (staticQuery != null)
                {
                    filters = staticQuery + " and ";
                }

                filters += $"({string.Join(" or ", batch.Select(p => p))})";
                var results = await QueryReportingApi(viewName, filters);
                allResults.AddRange(results.ToObject<List<T>>()!);
            }

            return allResults;
        }

        private async Task<JToken> QueryReportingApi(string view, string? filter = null, bool getAll = true)
        {
            int pageCount = 0;

            IEnumerable<JToken> combinedData = new List<JToken>();
            JObject? response;

            do
            {

                var url = $"x/odata/api/views/{view}";
                if (filter == null)
                {
                    _logger.LogDebug("Sending GET request. URL={Url}", url);
                    response = await _httpService.GetAsync(url);
                }
                else
                {
                    var queryParams = new Dictionary<string, string>
                    {
                        { "$filter", filter },
                        { "$skip", pageCount.ToString() },
                    };

                    var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
                    _logger.LogDebug("Sending GET request. URL={Url}?{QueryString} | QueryParams={@QueryParams}", url, queryString, queryParams);

                    response = await _httpService.GetAsync(url, queryParams);
                }

                var responseObject = response?.GetValue("value");
                if (responseObject == null)
                {
                    throw new Exception("couldn't parse the result");
                }

                pageCount = responseObject.Count();
                combinedData = responseObject.Union(combinedData);

                // Reporting API gives max 1000 records, so fetch them all if there are more
            } while (getAll && pageCount == 1000);

            return JToken.FromObject(combinedData);
        }
        #endregion
    }
}
