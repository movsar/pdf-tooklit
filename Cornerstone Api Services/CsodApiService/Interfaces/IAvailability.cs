using Newtonsoft.Json;

namespace CornerstoneApiServices.Interfaces
{
    public interface IAvailability
    {
        //string Id { get; set; }
        [JsonProperty("includeSubs")]
        bool IncludeSubs { get; set; }
    }
}