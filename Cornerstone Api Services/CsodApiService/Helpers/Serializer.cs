using Newtonsoft.Json;

namespace CornerstoneApiServices.Helpers
{
    internal static class Serializer
    {
        private static JsonSerializerSettings _settings = new()
        {
            Formatting = Formatting.Indented
        };

        internal static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        internal static T? Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
