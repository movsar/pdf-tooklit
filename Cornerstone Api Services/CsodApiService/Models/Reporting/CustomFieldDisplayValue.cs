using Newtonsoft.Json;
using System.Text.Json;

namespace CornerstoneApiServices.Models.Reporting
{
    public class CustomFieldDisplayValue
    {
        [JsonProperty("cfvl_field_id")]
        public int FieldId { get; set; }

        [JsonProperty("cfvl_value_id")]
        public int ValueId { get; set; }

        [JsonProperty("culture_id")]
        public int CultureId { get; set; }

        [JsonProperty("cfvl_title")]
        public string Title { get; set; } = string.Empty;
    }
}
