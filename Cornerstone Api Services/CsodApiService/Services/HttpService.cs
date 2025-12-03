using CornerstoneApiServices.Helpers;
using CornerstoneApiServices.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace CornerstoneApiServices.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly CornerstoneApiConfiguration _apiConfiguration;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<HttpService> _logger;

        private string ApiRoot => _apiConfiguration.BaseUrl;
        private string TokenUrl => _apiConfiguration.BaseUrl + "/oauth2/token";

        public HttpService(CornerstoneApiConfiguration CornerstoneApiConfiguration, IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("cs-client");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiConfiguration = CornerstoneApiConfiguration;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private string GetUrl(string endpoint, Dictionary<string, string>? queryParameters = null)
        {
            string url = endpoint.Contains("http") ? endpoint : $"{ApiRoot}/{endpoint}";

            // If queryParameters are not empty, add them to the url
            if (queryParameters != null)
            {
                var builder = new UriBuilder(url);

                var query = HttpUtility.ParseQueryString(builder.Query);
                foreach (var queryParameter in queryParameters)
                {
                    query[queryParameter.Key] = queryParameter.Value;
                }
                builder.Query = query.ToString();
                url = builder.Uri.AbsoluteUri.ToString();
            }

            return $"{url}";
        }

        #region Authorization    
        private async Task<TokenResponse> AuthorizeAsync()
        {
            _logger.LogDebug("Calling CS authorization endpoint at {Now} UTC", DateTime.UtcNow);

            // Set the request payload
            var payload = new Dictionary<string, object>()
            {
                { "grantType",  _apiConfiguration.GrantType },
                { "clientId", _apiConfiguration.ClientId },
                { "clientSecret", _apiConfiguration.ClientSecret },
                { "scope" , _apiConfiguration.Scope},
            };

            if (payload.Any(v => v.Value is null))
            {
                throw new Exception("Api credentials missing");
            }

            var payloadContent = new StringContent(Serializer.Serialize(payload), Encoding.UTF8, "application/json");

            // Send the request
            var response = await _httpClient.PostAsync(TokenUrl, payloadContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

            // Parse the response
            var responseString = await response.Content.ReadAsStringAsync();
            var tokenResponse = Serializer.Deserialize<TokenResponse>(responseString);
            if (responseString == null || tokenResponse == null)
            {
                throw new NullReferenceException("Authorization failed");
            }

            return tokenResponse ?? throw new Exception("Couldn't deserialize CS authorization token response");
        }
        private async Task<string> GetAccessTokenAsync()
        {
            const string cacheKey = "csAccessToken";
            const int safetyMarginSeconds = 60;

            if (_memoryCache.TryGetValue<TokenResponse>(cacheKey, out var cachedToken))
            {
                var expiresAt = cachedToken!.IssuedAtUtc.AddSeconds(cachedToken.ExpiresIn);
                var secondsRemaining = (int)(expiresAt - DateTime.UtcNow).TotalSeconds;

                if (secondsRemaining > safetyMarginSeconds)
                {
                    _logger.LogDebug("Reusing cached CS token at {Now} UTC. Expires at {Expiration} UTC (in {Remaining} seconds).", DateTime.UtcNow, expiresAt, secondsRemaining);
                    return cachedToken.AccessToken!;
                }

                _logger.LogInformation("CS token near expiry or expired. Issued at {IssuedAt} UTC. Remaining: {Remaining} seconds. Refreshing...", cachedToken.IssuedAtUtc, secondsRemaining);
            }

            // Token is missing or stale — refresh it
            var newToken = await AuthorizeAsync();
            newToken.IssuedAtUtc = DateTime.UtcNow;

            var cacheDuration = TimeSpan.FromSeconds(newToken.ExpiresIn - safetyMarginSeconds);
            if (cacheDuration <= TimeSpan.Zero)
            {
                _logger.LogInformation("CS token lifetime too short ({ExpiresIn}s). Using minimum cache duration of 30 seconds.", newToken.ExpiresIn);
                cacheDuration = TimeSpan.FromSeconds(30);
            }

            _memoryCache.Set(cacheKey, newToken, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            });

            _logger.LogInformation("New CS access token issued at {IssuedAt} UTC. Expires in {ExpiresIn} seconds.", newToken.IssuedAtUtc, newToken.ExpiresIn);
            return newToken.AccessToken!;
        }
        #endregion

        #region Requests
        private async Task<JObject?> PerformHttpRequestAsync(HttpMethod method, string endpoint, HttpContent? payload = null, Dictionary<string, string>? queryParameters = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            var accessToken = await GetAccessTokenAsync();
            if (accessToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var url = GetUrl(endpoint, queryParameters);

            var request = new HttpRequestMessage(method, url);
            if (payload != null)
            {
                request.Content = payload;
            }
            request.Headers.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || responseString.Contains("Unauthorized"))
            {
                throw new Exception(responseString);
            }

            if (string.IsNullOrEmpty(responseString))
            {
                return new JObject();
            }

            var responseObject = Serializer.Deserialize<dynamic>(responseString)!;
            if (responseObject is JObject)
            {
                return responseObject;
            }

            JObject responseData = Serializer.Deserialize<dynamic>(responseObject)!;
            return responseData;
        }

        public async Task<JObject?> GetAsync(string endpoint, Dictionary<string, string>? queryParameters = null)
        {
            return await PerformHttpRequestAsync(HttpMethod.Get, endpoint, queryParameters: queryParameters);
        }
        public async Task<JObject?> PostAsync(string endpoint, string serializedPayload)
        {
            return await PerformHttpRequestAsync(HttpMethod.Post, endpoint, payload: new StringContent(serializedPayload, Encoding.UTF8, "application/json"));
        }
        public async Task<JObject?> PutAsync(string endpoint, MultipartFormDataContent? formData = null)
        {
            return await PerformHttpRequestAsync(HttpMethod.Put, endpoint, payload: formData);
        }
        public async Task<JObject?> DeleteAsync(string endpoint)
        {
            return await PerformHttpRequestAsync(HttpMethod.Delete, endpoint);
        }
        public async Task<JObject?> PatchAsync(string endpoint, string serializedPayload)
        {
            return await PerformHttpRequestAsync(HttpMethod.Patch, endpoint, payload: new StringContent(serializedPayload, Encoding.UTF8, "application/json"));
        }
        #endregion
    }
}
