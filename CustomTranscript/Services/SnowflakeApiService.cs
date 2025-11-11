using Rest.Data.Enums;
using Snowflake_Api_Services.Services;
using CornerstoneApiServices.Models.Reporting;

namespace CustomTranscript.App.Services
{
    public class SnowflakeApiService
    {
        private readonly RequestService _requestService;

        public SnowflakeApiService(RequestService requestService)
        {
            _requestService = requestService;
        }

        private string FormatDate(DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss");

        public async Task<List<RptTranscript>> GetTranscripts(int userId, int statusId,
                                                              DateTime dateFrom,
                                                              DateTime dateTo,
                                                              bool getAll = true)
        {
            var sql = $@"
                SELECT *
                FROM ""TRANSCRIPT""
                WHERE ""transc_user_id"" = {userId}
                  AND ""user_lo_status_id"" = {statusId}
                  AND ""user_lo_comp_dt"" > '{FormatDate(dateFrom)}'
                  AND ""user_lo_comp_dt"" < '{FormatDate(dateTo)}'
                {(getAll ? "" : "LIMIT 100")}
            ";

            return await _requestService.ExecuteSnowflakeQueryAsync<RptTranscript>(sql);
        }

        public async Task<List<RptTranscript>> GetTranscripts(int userId,
                                                              int statusId,
                                                              IEnumerable<string> loIds,
                                                              bool getAll = true)
        {
            var ids = string.Join(",", loIds.Select(id => $"'{id}'"));
            var sql = $@"
                SELECT *
                FROM ""TRANSCRIPT""
                WHERE ""transc_user_id"" = {userId}
                  AND ""user_lo_status_id"" = {statusId}
                  AND ""transc_object_id"" IN ({ids})
                {(getAll ? "" : "LIMIT 100")}
            ";

            return await _requestService.ExecuteSnowflakeQueryAsync<RptTranscript>(sql);
        }

        public async Task<List<RptLearningObject>> ListLearningObjects(IEnumerable<string> learningObjectIds)
        {
            var ids = string.Join(",", learningObjectIds.Select(id => $"'{id}'"));
            var sql = $@"
                SELECT *
                FROM ""COURSES""
                WHERE ""object_id"" IN ({ids})
            ";

            return await _requestService.ExecuteSnowflakeQueryAsync<RptLearningObject>(sql);
        }

        public async Task<List<RptLearningObject>> ListLearningObjectsByEndDate(LOTypeEnum loType,
                                                                                DateTime dateFrom,
                                                                                DateTime dateTo,
                                                                                bool getAll = true)
        {
            var sql = $@"
                SELECT *
                FROM ""COURSES""
                WHERE ""lo_type_id"" = {(int)loType}
                  AND ""lo_end_dt"" > '{FormatDate(dateFrom)}'
                  AND ""lo_end_dt"" < '{FormatDate(dateTo)}'
                {(getAll ? "" : "LIMIT 100")}
            ";

            return await _requestService.ExecuteSnowflakeQueryAsync<RptLearningObject>(sql);
        }

        public async Task<List<T>> GetTranscriptCustomFields<T>(IEnumerable<string> learningObjectIds, int userId) where T : class
        {
            var ids = string.Join(",", learningObjectIds.Select(id => $"'{id}'"));
            var sql = $@"
                SELECT *
                FROM ""TRANSCRIPT_CUSTOM_FIELDS""
                WHERE ""transc_cf_user_id"" = {userId}
                  AND ""transc_cf_object_id"" IN ({ids})
            ";
            return await _requestService.ExecuteSnowflakeQueryAsync<T>(sql);
        }

        public async Task<List<T>> ListLearningObjectCustomFields<T>(IEnumerable<string> learningObjectIds) where T : class
        {
            var ids = string.Join(",", learningObjectIds.Select(id => $"'{id}'"));
            var sql = $@"
                SELECT *
                FROM ""COURSE_CUSTOM_FIELDS""
                WHERE ""train_cf_object_id"" IN ({ids})
            ";
            return await _requestService.ExecuteSnowflakeQueryAsync<T>(sql);
        }

        public async Task<List<CustomFieldDisplayValue>> GetCustomFieldDisplayValues(int fieldId, int cultureId = 1)
        {
            var sql = $@"
                SELECT *
                FROM ""FORM_CUSTOM_FIELD_DISPLAY_VALUES""
                WHERE ""cfvl_field_id"" = {fieldId}
                  AND ""culture_id"" = {cultureId}
            ";
            return await _requestService.ExecuteSnowflakeQueryAsync<CustomFieldDisplayValue>(sql);
        }

        public Task<List<Dictionary<string, string>>> GetTablesAsync(string schemaName)
            => _requestService.GetTablesAsync(schemaName);

        public Task<List<string>> GetTableColumnsAsync(string schemaName, string tableName)
            => _requestService.GetTableColumnsAsync(schemaName, tableName);
    }
}