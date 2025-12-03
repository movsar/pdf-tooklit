using Newtonsoft.Json;

namespace CornerstoneApiServices.Models
{
    internal class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonIgnore]
        public DateTime IssuedAtUtc { get; set; }
    }
}
