using Newtonsoft.Json;

namespace CustomTranscript.App.Models
{
    public class PilotRptTrainingCfs
    {
        [JsonProperty("lo_custom_field_00001")]
        public string? ContentOwnerEmail { get; set; }

        [JsonProperty("lo_custom_field_00002")]
        public string? ContentOwnerName { get; set; }

        [JsonProperty("lo_custom_field_00003")]
        public string? CreditCEU { get; set; }

        [JsonProperty("lo_custom_field_00004")]
        public string? CreditCPD { get; set; }

        [JsonProperty("lo_custom_field_00005")]
        public string? CreditExpiration { get; set; }

        [JsonProperty("lo_custom_field_00006")]
        public string? CreditType { get; set; }

        [JsonProperty("lo_custom_field_00007")]
        public string? Duration { get; set; }

        [JsonProperty("lo_custom_field_00009")]
        public string? ExternalTrainingType { get; set; }

        [JsonProperty("lo_custom_field_00010")]
        public string? ExternalVendor { get; set; }

        [JsonProperty("lo_custom_field_00011")]
        public string? HDRDeliveryMethod { get; set; }

        [JsonProperty("lo_custom_field_00012")]
        public string? InAnnualReports { get; set; }

        [JsonProperty("lo_custom_field_00013")]
        public string? Instructor { get; set; }

        [JsonProperty("lo_custom_field_00014")]
        public string? LODODSharePointID { get; set; }

        [JsonProperty("lo_custom_field_00015")]
        public string? CreditLU { get; set; }

        [JsonProperty("lo_custom_field_00016")]
        public string? CreditLUHSW { get; set; }

        [JsonProperty("lo_custom_field_00017")]
        public string? LinksAttachedResources { get; set; }

        [JsonProperty("lo_custom_field_00018")]
        public string? Location { get; set; }

        [JsonProperty("lo_custom_field_00019")]
        public string? MainCategory { get; set; }

        [JsonProperty("lo_custom_field_00020")]
        public string? PeopleReferencedByName { get; set; }

        [JsonProperty("lo_custom_field_00021")]
        public string? CreditPDH { get; set; }

        [JsonProperty("lo_custom_field_00022")]
        public string? QC360ReviewLink { get; set; }

        [JsonProperty("lo_custom_field_00023")]
        public string? QCChangesImplemented { get; set; }

        [JsonProperty("lo_custom_field_00024")]
        public string? QCLastReviewDate { get; set; }

        [JsonProperty("lo_custom_field_00025")]
        public string? Required { get; set; }

        [JsonProperty("lo_custom_field_00026")]
        public string? TargetAudience { get; set; }

        [JsonProperty("lo_custom_field_00027")]
        public string? HDRVersionNumber { get; set; }

        [JsonProperty("lo_custom_field_00028")]
        public string? WebexLinkToJoin { get; set; }

        [JsonProperty("lo_custom_field_00029")]
        public string? ExpirationDate { get; set; }

        [JsonProperty("train_cf_object_id")]
        public string? CfLearningObjectId { get; set; }
    }
}
