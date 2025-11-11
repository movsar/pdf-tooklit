using Newtonsoft.Json;

namespace CustomTranscript.App.Models
{
    public class PilotRptTranscriptCfs
    {
        [JsonProperty("transc_cf_user_id")]
        public int UserId { get; set; }

        [JsonProperty("transc_cf_object_id")]
        public string LearningObjectId { get; set; } = null!;

        [JsonProperty("user_lo_custom_field_00001")]
        public string? ContentOwnerEmail { get; set; }

        [JsonProperty("user_lo_custom_field_00002")]
        public string? ContentOwnerName { get; set; }

        [JsonProperty("user_lo_custom_field_00003")]
        public string? CreditExpiration { get; set; }

        [JsonProperty("user_lo_custom_field_00004")]
        public string? CreditType { get; set; }

        [JsonProperty("user_lo_custom_field_00011")]
        public int? ExternalTrainingType { get; set; }

        [JsonProperty("user_lo_custom_field_00012")]
        public string? ExternalVendor { get; set; }

        [JsonProperty("user_lo_custom_field_00013")]
        public string? HDRDeliveryMethod { get; set; }

        [JsonProperty("user_lo_custom_field_00014")]
        public string? HDRVersionNumber { get; set; }

        [JsonProperty("user_lo_custom_field_00015")]
        public string? InAnnualReports { get; set; }

        [JsonProperty("user_lo_custom_field_00016")]
        public string? Instructor { get; set; }

        [JsonProperty("user_lo_custom_field_00017")]
        public string? LODODSharePointID { get; set; }

        [JsonProperty("user_lo_custom_field_00018")]
        public string? LinksAttachedResources { get; set; }

        [JsonProperty("user_lo_custom_field_00019")]
        public string? Location { get; set; }

        [JsonProperty("user_lo_custom_field_00020")]
        public string? MainCategory { get; set; }

        [JsonProperty("user_lo_custom_field_00021")]
        public string? PeopleReferencedByName { get; set; }

        [JsonProperty("user_lo_custom_field_00022")]
        public string? QC360ReviewLink { get; set; }

        [JsonProperty("user_lo_custom_field_00023")]
        public string? QCChangesImplemented { get; set; }

        [JsonProperty("user_lo_custom_field_00024")]
        public string? QCLastReviewDate { get; set; }

        [JsonProperty("user_lo_custom_field_00025")]
        public string? Required { get; set; }

        [JsonProperty("user_lo_custom_field_00026")]
        public string? TargetAudience { get; set; }

        [JsonProperty("user_lo_custom_field_00027")]
        public string? Duration { get; set; }

        [JsonProperty("user_lo_custom_field_00028")]
        public string? ExpirationDate { get; set; }

        [JsonProperty("user_lo_custom_field_00029")]
        public string? WebexLinkToJoin { get; set; }

        [JsonProperty("user_lo_custom_field_00030")]
        public string? LearnSessionId { get; set; }

        [JsonProperty("user_lo_custom_field_00036")]
        public string? CreditCEU { get; set; }

        [JsonProperty("user_lo_custom_field_00037")]
        public string? CreditCPD { get; set; }

        [JsonProperty("user_lo_custom_field_00038")]
        public string? CreditLU { get; set; }

        [JsonProperty("user_lo_custom_field_00039")]
        public string? CreditLUHSW { get; set; }

        [JsonProperty("user_lo_custom_field_00040")]
        public string? CreditPDH { get; set; }
    }
}
