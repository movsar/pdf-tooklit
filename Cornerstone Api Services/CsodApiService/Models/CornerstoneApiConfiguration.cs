
namespace CornerstoneApiServices.Models
{
    public class CornerstoneApiConfiguration
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string GrantType { get; set; } = "client_credentials";
        public string Scope { get; set; } = "all";

        public void Validate()
        {
            if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
            {
                throw new Exception("Csod config is not specified");
            }
        }
    }
}
