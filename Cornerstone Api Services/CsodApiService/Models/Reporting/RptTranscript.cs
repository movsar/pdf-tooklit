using Newtonsoft.Json;

namespace CornerstoneApiServices.Models.Reporting
{
    public class RptTranscript {
        [JsonProperty("reg_num")]
        public int? RegNum { get; set; }

        [JsonProperty("user_lo_status_id")]
        public int? UserLoStatusId { get; set; }

        [JsonProperty("user_lo_score")]
        public int? UserLoScore { get; set; }

        [JsonProperty("user_lo_create_dt")]
        public DateTime? UserLoCreateDt { get; set; }

        [JsonProperty("user_lo_reg_dt")]
        public DateTime? UserLoRegDt { get; set; }

        [JsonProperty("user_lo_start_dt")]
        public DateTime? UserLoStartDt { get; set; }

        [JsonProperty("user_lo_comp_dt")]
        public DateTime? UserLoCompDt { get; set; }

        [JsonProperty("user_lo_last_access_dt")]
        public DateTime? UserLoLastAccessDt { get; set; }

        [JsonProperty("user_lo_minutes_participated")]
        public int? UserLoMinutesParticipated { get; set; }

        [JsonProperty("user_lo_num_attempts")]
        public int? UserLoNumAttempts { get; set; }

        [JsonProperty("user_lo_assignor_id")]
        public int? UserLoAssignorId { get; set; }

        [JsonProperty("user_lo_assignor_ref")]
        public string? UserLoAssignorRef { get; set; }

        [JsonProperty("user_lo_assignor")]
        public string? UserLoAssignor { get; set; }

        [JsonProperty("user_lo_comment")]
        public string? UserLoComment { get; set; }

        [JsonProperty("user_lo_min_due_date")]
        public DateTime? UserLoMinDueDate { get; set; }

        [JsonProperty("is_removed")]
        public bool? IsRemoved { get; set; }

        [JsonProperty("user_lo_removed_reason_id")]
        public int? UserLoRemovedReasonId { get; set; }

        [JsonProperty("user_lo_removed_comments")]
        public string? UserLoRemovedComments { get; set; }

        [JsonProperty("user_lo_removed_dt")]
        public DateTime? UserLoRemovedDt { get; set; }

        [JsonProperty("completed_sco")]
        public int? CompletedSco { get; set; }

        [JsonProperty("archived")]
        public bool? Archived { get; set; }

        [JsonProperty("user_lo_assigned_comments")]
        public string? UserLoAssignedComments { get; set; }

        [JsonProperty("user_lo_assigned_dt")]
        public DateTime? UserLoAssignedDt { get; set; }

        [JsonProperty("training_purpose")]
        public string? TrainingPurpose { get; set; }

        [JsonProperty("training_purpose_category")]
        public string? TrainingPurposeCategory { get; set; }

        [JsonProperty("user_lo_last_action_dt")]
        public DateTime? UserLoLastActionDt { get; set; }

        [JsonProperty("user_lo_pct_complete")]
        public double? UserLoPctComplete { get; set; }

        [JsonProperty("exemptor_id")]
        public int? ExemptorId { get; set; }

        [JsonProperty("exempt_comment")]
        public string? ExemptComment { get; set; }

        [JsonProperty("approver_exempt_comment")]
        public string? ApproverExemptComment { get; set; }

        [JsonProperty("exempt_dt")]
        public DateTime? ExemptDt { get; set; }

        [JsonProperty("exempt_reason_id")]
        public int? ExemptReasonId { get; set; }

        [JsonProperty("exempt_approver_reason_id")]
        public int? ExemptApproverReasonId { get; set; }

        [JsonProperty("exempt_reason")]
        public string? ExemptReason { get; set; }

        [JsonProperty("exempt_approver_reason")]
        public string? ExemptApproverReason { get; set; }

        [JsonProperty("is_assigned")]
        public bool? IsAssigned { get; set; }

        [JsonProperty("is_suggested")]
        public bool? IsSuggested { get; set; }

        [JsonProperty("is_required")]
        public bool? IsRequired { get; set; }

        [JsonProperty("is_latest_reg_num")]
        public int? IsLatestRegNum { get; set; }

        [JsonProperty("is_archive")]
        public int? IsArchive { get; set; }

        [JsonProperty("user_lo_pass")]
        public bool? UserLoPass { get; set; }

        [JsonProperty("user_lo_cancellation_reason_id")]
        public int? UserLoCancellationReasonId { get; set; }

        [JsonProperty("user_lo_cancellation_reason")]
        public string? UserLoCancellationReason { get; set; }

        [JsonProperty("user_lo_withdrawal_reason_id")]
        public int? UserLoWithdrawalReasonId { get; set; }

        [JsonProperty("user_lo_withdrawal_reason")]
        public string? UserLoWithdrawalReason { get; set; }

        [JsonProperty("user_lo_from_training_plan")]
        public string? UserLoFromTrainingPlan { get; set; }

        [JsonProperty("user_lo_available_dt")]
        public DateTime? UserLoAvailableDt { get; set; }

        [JsonProperty("user_lo_training_link_expiration_date")]
        public DateTime? UserLoTrainingLinkExpirationDate { get; set; }

        [JsonProperty("user_lo_timezone_code")]
        public string? UserLoTimezoneCode { get; set; }

        [JsonProperty("user_lo_withdrawal_date")]
        public DateTime? UserLoWithdrawalDate { get; set; }

        [JsonProperty("transcript_badge_id")]
        public string? TranscriptBadgeId { get; set; }

        [JsonProperty("transcript_badge_points")]
        public int? TranscriptBadgePoints { get; set; }

        [JsonProperty("transcript_training_points")]
        public int? TranscriptTrainingPoints { get; set; }

        [JsonProperty("transc_user_id")]
        public int? TranscUserId { get; set; }

        [JsonProperty("transc_object_id")]
        public string? TranscObjectId { get; set; }

        [JsonProperty("user_lo_status_group_id")]
        public int? UserLoStatusGroupId { get; set; }

        [JsonProperty("is_latest_version_on_transcript")]
        public bool? IsLatestVersionOnTranscript { get; set; }

        [JsonProperty("user_lo_last_modified_dt")]
        public DateTime? UserLoLastModifiedDt { get; set; }

        [JsonProperty("_last_touched_dt_utc")]
        public DateTime? _LastTouchedDtUtc { get; set; }

        [JsonProperty("is_express_class")]
        public bool? IsExpressClass { get; set; }

        [JsonProperty("user_lo_equivalent_object_id")]
        public string? UserLoEquivalentObjectId { get; set; }

        [JsonProperty("user_lo_equivalency_type")]
        public string? UserLoEquivalencyType { get; set; }

        [JsonProperty("user_lo_delivery_method_id")]
        public int? UserLoDeliveryMethodId { get; set; }

        [JsonProperty("is_standalone")]
        public bool? IsStandalone { get; set; }

        [JsonProperty("user_lo_remover_id")]
        public int? UserLoRemoverId { get; set; }
    }

}
