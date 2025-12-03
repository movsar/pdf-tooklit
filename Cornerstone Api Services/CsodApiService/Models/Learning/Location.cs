using Newtonsoft.Json;
using CornerstoneApiServices.Enums;

namespace CornerstoneApiServices.Models.Learning
{
    public class Location
    {
        [JsonProperty("FacilityType")]
        public string FacilityType { get; set; }

        [JsonProperty("CountryCode")]
        public CountryCode CountryCode { get; set; }

        [JsonProperty("Line1")]
        public string Line1 { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Province")]
        public string Province { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("TimeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("Contact")]
        public string Contact { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Fax")]
        public string Fax { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Zip")]
        public object Zip { get; set; }
    }
}
