using Newtonsoft.Json;

namespace CornerstoneApiServices.Models.Reporting
{
    public class Group
    {
        [JsonProperty("group__user_id")]
        public int UserId { get; set; }

        [JsonProperty("group__ou_id")]
        public int Ouid { get; set; }

        [JsonProperty("group__active")]
        public bool Active { get; set; }

        [JsonProperty("group__ref")]
        public string Ref { get; set; }

        [JsonProperty("group__is_frozen")]
        public bool IsFrozen { get; set; }

        [JsonProperty("group__owner_id")]
        public int? OwnerId { get; set; }

        [JsonProperty("group__owner_name")]
        public string OwnerName { get; set; }

        [JsonProperty("group__owner_ref")]
        public string OwnerRef { get; set; }

        [JsonProperty("group__parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("group__parent_ref")]
        public string ParentRef { get; set; }
    }
}
