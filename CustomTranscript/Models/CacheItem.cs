using CornerstoneApiServices.Models.Learning;

namespace CustomTranscript.App.Models
{
    internal class CacheItem
    {
        internal List<LrnSession> Sessions { get; set; } = new();
        internal DateTimeOffset CachedAt { get; set; }
    }
}
