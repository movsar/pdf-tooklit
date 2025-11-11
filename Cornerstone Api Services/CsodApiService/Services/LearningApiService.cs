using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CornerstoneApiServices.Models;
using CornerstoneApiServices.Enums;
using CornerstoneApiServices.Interfaces;
using CornerstoneApiServices.Models.Learning;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using CornerstoneApiServices.Helpers;

namespace CornerstoneApiServices.Services
{
    public class LearningApiService
    {
        private readonly HttpService _httpService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LearningApiService> _logger;

        public LearningApiService(HttpService httpService, IConfiguration configuration, ILogger<LearningApiService> logger)
        {
            _httpService = httpService;
            _configuration = configuration;
            _logger = logger;
        }

        #region Transcript
        public async Task<bool> CheckForTranscripts(string externalId, string withStatus, DateTime dateFrom, DateTime dateTo)
        {
            var queryParameters = new Dictionary<string, string> {
                { "UserId", externalId },
                { "Status", withStatus }
            };

            var pageResponse = await _httpService.GetAsync("TranscriptAndTask/Transcript", queryParameters);
            var data = (JObject?)GetResponseData(pageResponse)?.Children().First();
            var transcriptObjects = data!.Property("Transcripts")?.ToArray().Children().Cast<JObject>();
            var transcripts = ExtractTranscripts(transcriptObjects, externalId);
            transcripts = transcripts.Where(t => t.CompletedDate >= dateFrom && t.CompletedDate < dateTo.AddDays(1)).ToList();
            return transcripts.Any();
        }

        public async Task<List<LrnSession>> GetSessions(string externalId, string withStatus)
        {
            var queryParameters = new Dictionary<string, string> {
                { "UserId", externalId },
                { "Status", withStatus }
            };

            var responseData = await QueryLearningApi("TranscriptAndTask/Session", queryParameters);
            var sessionObjects = responseData?.Property("Sessions")?.ToArray().Children().Cast<JObject>();
            if (sessionObjects == null)
            {
                throw new NullReferenceException("transcripts object is null");
            }

            return sessionObjects.Select(s => s.ToObject<LrnSession>()).ToList()!;
        }
        public async Task<List<CustomField>?> GetTranscriptCustomFields(string loId, string externalId)
        {
            var queryParameters = new Dictionary<string, string> {
                { "externalId", externalId },
                { "learningObjectId", loId },
            };

            try
            {
                var pageResponse = await _httpService.GetAsync("v1/transcripts/custom-fields", queryParameters);
                if (pageResponse == null || pageResponse["status"]?.ToString() != "success")
                {
                    throw new Exception("Couldn't fetch transcript custom fields");
                }
                var customFields = pageResponse["data"]!.ToObject<List<CustomField>>();
                return customFields;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("record not found"))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<List<Transcript>> GetTranscripts(string userId, string? withStatus = null)
        {
            var queryParameters = new Dictionary<string, string> {
                { "UserId", userId },
            };

            if (!string.IsNullOrEmpty(withStatus))
            {
                queryParameters.Add("Status", withStatus);
            }

            var responseData = await QueryLearningApi("TranscriptAndTask/Transcript", queryParameters);
            var transcriptObjects = responseData?.Property("Transcripts")?.ToArray().Children().Cast<JObject>();
            return ExtractTranscripts(transcriptObjects, userId);
        }
        private List<Transcript> ExtractTranscripts(IEnumerable<JObject?> transcriptObjects, string externalId)
        {
            List<Transcript> transcripts = new List<Transcript>();
            if (transcriptObjects == null)
            {
                throw new NullReferenceException("transcripts object is null");
            }
            foreach (var to in transcriptObjects)
            {
                DateTime? dueDate = null,
                    completionDate = null,
                    lastAccessDate = null,
                    registrationDate = null;

                DateTime tempDate;
                if (DateTime.TryParse(to.GetValue("DueDate")?.ToString(), out tempDate))
                {
                    dueDate = tempDate;
                }
                if (DateTime.TryParse(to.GetValue("CompletionDate")?.ToString(), out tempDate))
                {
                    completionDate = tempDate;
                }
                if (DateTime.TryParse(to.GetValue("LastAccessDate")?.ToString(), out tempDate))
                {
                    lastAccessDate = tempDate;
                }
                if (DateTime.TryParse(to.GetValue("RegistrationDate")?.ToString(), out tempDate))
                {
                    registrationDate = tempDate;
                }

                var loId = to.GetValue("LoId")?.ToString();
                var loTitle = to.GetValue("Title")?.ToString();
                var loProviderId = to.GetValue("LoProviderId")?.ToString();
                var loType = to.GetValue("LoType")?.ToString();
                var loProviderName = to.GetValue("ProviderName")?.ToString();
                var score = to.GetValue("Score")?.ToString();
                var status = to.GetValue("Status")?.ToString();
                var trainingHours = to.GetValue("TrainingHours")?.ToString();
                var totalTime = to.GetValue("TotalTime")?.ToString();

                transcripts.Add(new Transcript()
                {
                    DueDate = dueDate,
                    CompletedDate = completionDate,
                    LearningTitle = loTitle,
                    LearningObjectId = loId!,
                    TrainingType = loType,
                    ProviderId = loProviderId,
                    UserId = externalId,
                    TrainingHours = trainingHours,
                    Status = status,
                    ProviderName = loProviderName
                });
            }

            return transcripts;
        }
        public async Task UpdateTranscript(string externalId, string learningObjectId, string comment, int drillProficiency, string drillStatus, DateTime evaluationDate)
        {
            var drillEvalDateTag = _configuration["TRANSCRIPT_EVAL_TAG"];
            var drillProficiencyTag = _configuration["TRANSCRIPT_PROFICIENCY_TAG"];
            var drillStatusTag = _configuration["TRANSCRIPT_STATUS_TAG"];

            var requestData = new
            {
                externalId,
                learningObjectId,
                comment,
                customFields = new[]
                {
                    new
                    {
                        name = drillEvalDateTag,
                        values = new[] { evaluationDate.ToString("G") }
                    },
                    new
                    {
                        name = drillProficiencyTag,
                        values = new[] { drillProficiency.ToString() }
                    },
                    new
                    {
                        name = drillStatusTag,
                        values = new[] { drillStatus == "OK" ? "Met" : "Not Met" }
                    }
                }
            };

            var reqBody = Serializer.Serialize(requestData);
            var requestContent = new StringContent(reqBody, Encoding.UTF8, Application.Json);
            await _httpService.PatchAsync("v1/transcripts/update", Serializer.Serialize(requestData));
        }
        public async Task RegisterTranscript(string externalId, string learningObjectId)
        {
            var requestData = new
            {
                externalId,
                learningObjectId,
            };

            await _httpService.PatchAsync("v1/transcripts/register", Serializer.Serialize(requestData));
        }
        public async Task AssignTranscript(string externalId, string learningObjectId, Assignment assignment)
        {
            string assignmentValue = assignment switch
            {
                Assignment.Approved => "approved",
                Assignment.Assigned => "assigned",
                Assignment.Registered => "registered",
                _ => throw new Exception("Invalid Assignment value")
            };

            var requestData = new
            {
                externalId,
                learningObjectId,
                assignment = assignmentValue
            };

            await _httpService.PostAsync("v1/transcripts/assign", Serializer.Serialize(requestData));
        }
        public async Task<Transcript?> GetTranscript(string externalId, string loId)
        {
            var queryParameters = new Dictionary<string, string> {
                { "externalId", externalId },
                { "learningObjectId", loId },
            };

            try
            {
                var responseData = await QueryLearningApi("v1/transcripts/details", queryParameters);
                var transcript = responseData?.ToObject<Transcript>();
                return transcript;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("record not found"))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion

        #region Learning Objects
        public async Task<TrainingItem?> GetLoDetails(string loId, string externalId)
        {
            var queryParameters = new Dictionary<string, string> {
                { "ObjectID", loId},
                { "ActorID", externalId },
            };

            var responseData = await QueryLearningApi("LO/GetDetails", queryParameters);
            return responseData?.GetValue("trainingItem")?.ToObject<TrainingItem>();
        }
        public async Task<CreateLoResponse> CreateLo(LearningObject learningObject)
        {
            var requestData = new Dictionary<string, object>() {
                { "data", new { record = new List<JObject>() { JObject.FromObject(learningObject) } } }
            };

            var response = await _httpService.PostAsync("LO/Create", Serializer.Serialize(requestData));
            var responseData = GetResponseData(response);
            if (responseData.Count() == 0)
            {
                throw new Exception($"The request didn't get processed as expected, response: {responseData}");
            }

            return responseData[0]!.ToObject<CreateLoResponse>()!;
        }
        #endregion

        #region Internal Service Methods
        /// <summary>
        /// Handles Get request results, including those with pagination
        /// To retrieve paginated results recursively, specify @recordsPerPage
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="queryParameters"></param>
        /// <param name="recordsPerPage"></param>
        /// <returns>Deserizalized "data" object from the API response</returns>
        /// <exception cref="Exception"></exception>
        private async Task<JObject?> QueryLearningApi(string endpoint, Dictionary<string, string> queryParameters)
        {
            JObject? combinedData = null;

            int pageNumber = queryParameters.ContainsKey("PageNumber") ? int.Parse(queryParameters["PageNumber"]) : 1;

            int totalRecords = 0;
            int recordsPerPage = 0;
            do
            {
                queryParameters["PageNumber"] = pageNumber.ToString();

                // Build querystring for logging
                var queryString = string.Join("&", queryParameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
                _logger.LogDebug("Sending GET request. URL={Endpoint}?{QueryString} | QueryParams={@QueryParams}",
                    endpoint, queryString, queryParameters);

                var pageResponse = await _httpService.GetAsync(endpoint, queryParameters);
                var responseData = GetResponseData(pageResponse);
                var pageData = responseData?.Children<JObject>().FirstOrDefault();
                if (pageData == null)
                {
                    pageData = responseData.ToObject<JObject>();
                    if (pageData == null)
                    {
                        throw new Exception("Empty result");
                    }
                }

                if (recordsPerPage == 0)
                {
                    // Assign the first page's results count
                    recordsPerPage = pageData.Properties()
                         .Where(p => p.Value is JArray)
                         .Sum(p => ((JArray)p.Value).Count);
                }

                var totalRecordsString = pageResponse!.GetValue("totalRecords")?.ToString();

                if (combinedData == null)
                {
                    // First page
                    combinedData = pageData;
                    if (!int.TryParse(totalRecordsString, out totalRecords) || totalRecords <= recordsPerPage)
                    {
                        break;
                    }
                }
                else
                {
                    // Combine data of the second and next pages with the previous
                    MergePageData(combinedData, pageData);
                }

                pageNumber++;
            }
            while ((pageNumber - 1) * recordsPerPage < totalRecords);

            return combinedData;
        }

        // Merges the current page of data into the cumulative data object
        private void MergePageData(JObject? combinedData, JObject? currentPageData)
        {
            if (currentPageData == null || combinedData == null)
            {
                return;
            }

            foreach (var property in currentPageData.Properties().Where(p => p.Value is JArray))
            {
                if (combinedData[property.Name] is JArray existingArray)
                {
                    var newArray = (JArray)property.Value;
                    existingArray.Merge(newArray, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat });
                }
                else
                {
                    // If the property does not exist in combinedData, add it
                    combinedData.Add(property.Name, property.Value);
                }
            }
        }
        private JToken? GetResponseData(JObject? response)
        {
            var errorObject = response?.GetValue("error");
            if (errorObject != null)
            {
                var error = errorObject.ToObject<ErrorResponse>();
                throw new Exception(error!.Message ?? "InvalidData");
            }

            var statusObject = response?.GetValue("status");
            var data = response!.GetValue("data");
            if (data == null || statusObject != null && statusObject.ToString().ToLower() != "success" && int.Parse(statusObject.ToString()) > 299)
            {
                throw new Exception($"Unexpected error with status code: {statusObject}, response: {response}");
            }

            if (data.Count() == 0)
            {
                return data;
            }

            // When data consists of multiple objects, get the first as default error check
            JObject dataObject;
            try
            {
                dataObject = (JObject)data[0]!;
            }
            catch (Exception)
            {
                dataObject = (JObject)data;
            }

            if (dataObject == null)
            {
                return data;
            }

            var status = dataObject?.GetValue("Result");
            if (status != null && status.ToString().ToLower() == "failed")
            {
                var reason = dataObject?.GetValue("Reason");
                throw new Exception($"Request failed with the following reason: {reason}");
            }

            return data;
        }
        #endregion
    }
}
