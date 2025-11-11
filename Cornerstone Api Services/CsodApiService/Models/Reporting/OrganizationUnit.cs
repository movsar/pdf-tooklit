using Newtonsoft.Json;

namespace CornerstoneApiServices.Models.Reporting
{
    public class OrganizationUnit
    {
        [JsonProperty("ou_id")]
        public int OuId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("owner_id")]
        public int? OwnerId { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("approver_id")]
        public int? ApproverId { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("_last_touched_dt_utc")]
        public DateTime LastTouchedDate { get; set; }
    }
}
