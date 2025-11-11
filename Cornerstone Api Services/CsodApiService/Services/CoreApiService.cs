using CornerstoneApiServices.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedApiServiceHelpers.Enums;
using CornerstoneApiServices.Interfaces;
using CornerstoneApiServices.Models.Core;
using CornerstoneApiServices.Helpers;

namespace CornerstoneApiServices.Services
{
    public class CoreApiService
    {
        private readonly HttpService _httpService;
        public CoreApiService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Employee?> GetUserById(int targetUserId)
        {
            var result = await QueryLearningApi(HttpVerb.GET, $"x/users/v2/employees/{targetUserId}", null);
            var user = result as JObject;
            if (user == null)
            {
                throw new Exception("User could not be found");
            }

            return user.ToObject<Employee>();
        }

        #region Internal Service Methods
        private async Task<JToken?> QueryLearningApi(HttpVerb verb, string endpoint, object? payload, Dictionary<string, string>? queryParameters = null)
        {
            JObject? response;

            switch (verb)
            {
                case HttpVerb.GET:
                    response = await _httpService.GetAsync($"{endpoint}", queryParameters);
                    break;
                case HttpVerb.POST:
                    response = await _httpService.PostAsync($"{endpoint}", Serializer.Serialize(payload));
                    break;
                case HttpVerb.PUT:
                    response = await _httpService.PutAsync($"{endpoint}", null);
                    break;
                case HttpVerb.PATCH:
                    response = await _httpService.PatchAsync($"{endpoint}", Serializer.Serialize(payload));
                    break;
                default:
                    throw new Exception("Incorrect http verb");
            }

            if (response == null)
            {
                // Completed successfully with empty response
                return null;
            }

            return ParseResponse(response);
        }
        private JToken? ParseResponse(JObject? response)
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
            JObject dataObject = (JObject)data;

            if (dataObject == null)
            {
                return data;
            }

            return data;
        }
        #endregion
    }
}
