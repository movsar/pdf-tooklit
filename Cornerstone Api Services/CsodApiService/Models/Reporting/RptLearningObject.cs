namespace CornerstoneApiServices.Models.Reporting
{
    using Newtonsoft.Json;
    using System;

    public class RptLearningObject
    {
        [JsonProperty("lo_object_id")]
        public string? LoObjectId { get; set; }

        [JsonProperty("object_type")]
        public string? ObjectType { get; set; }

        [JsonProperty("lo_type")]
        public string? LoType { get; set; }

        [JsonProperty("object_id")]
        public string? ObjectId { get; set; }

        [JsonProperty("lo_object_type")]
        public string? LoObjectType { get; set; }

        [JsonProperty("lo_hours")]
        public double? LoHours { get; set; }

        [JsonProperty("lo_size")]
        public string? LoSize { get; set; }

        [JsonProperty("lo_credit")]
        public double? LoCredit { get; set; }

        [JsonProperty("lo_max_score")]
        public string? LoMaxScore { get; set; }

        [JsonProperty("lo_mastery_score")]
        public string? LoMasteryScore { get; set; }

        [JsonProperty("lo_price")]
        public double LoPrice { get; set; }

        [JsonProperty("lo_root_id")]
        public string? LoRootId { get; set; }

        [JsonProperty("lo_start_dt")]
        public string? LoStartDt { get; set; }

        [JsonProperty("lo_end_dt")]
        public string? LoEndDt { get; set; }

        [JsonProperty("lo_locator")]
        public string? LoLocator { get; set; }

        [JsonProperty("lo_reg_min")]
        public string? LoRegMin { get; set; }

        [JsonProperty("lo_reg_max")]
        public string? LoRegMax { get; set; }

        [JsonProperty("lo_min_parts")]
        public string? LoMinParts { get; set; }

        [JsonProperty("lo_withdraw_dt")]
        public string? LoWithdrawDt { get; set; }

        [JsonProperty("lo_instructor_id")]
        public string? LoInstructorId { get; set; }

        [JsonProperty("lo_location_id")]
        public string? LoLocationId { get; set; }

        [JsonProperty("lo_provider_id")]
        public string? LoProviderId { get; set; }

        [JsonProperty("lo_version")]
        public string? LoVersion { get; set; }

        [JsonProperty("lo_contact")]
        public string? LoContact { get; set; }

        [JsonProperty("lo_contact_user_ref")]
        public string? LoContactUserRef { get; set; }

        [JsonProperty("lo_no_show")]
        public double LoNoShow { get; set; }

        [JsonProperty("lo_withdrawal_penalty")]
        public double LoWithdrawalPenalty { get; set; }

        [JsonProperty("lo_created_by_user_id")]
        public int? LoCreatedByUserId { get; set; }

        [JsonProperty("lo_end_registration")]
        public string? LoEndRegistration { get; set; }

        [JsonProperty("lo_adv_reg_deadline")]
        public string? LoAdvRegDeadline { get; set; }

        [JsonProperty("lo_eval_01_override")]
        public string? LoEval01Override { get; set; }

        [JsonProperty("lo_eval_02_override")]
        public string? LoEval02Override { get; set; }

        [JsonProperty("lo_eval_03_override")]
        public string? LoEval03Override { get; set; }

        [JsonProperty("lo_active")]
        public string? LoActive { get; set; }

        [JsonProperty("total_sco")]
        public string? TotalSco { get; set; }

        [JsonProperty("lo_create_dt")]
        public DateTime LoCreateDt { get; set; }

        [JsonProperty("related_lo_id")]
        public string? RelatedLoId { get; set; }

        [JsonProperty("classification_id")]
        public string? ClassificationId { get; set; }

        [JsonProperty("lo_language_id")]
        public int? LoLanguageId { get; set; }

        [JsonProperty("lo_status_type")]
        public string? LoStatusType { get; set; }

        [JsonProperty("lo_currency_id")]
        public int? LoCurrencyId { get; set; }

        [JsonProperty("lo_modified_by_user_id")]
        public int? LoModifiedByUserId { get; set; }

        [JsonProperty("lo_modified_dt")]
        public DateTime LoModifiedDt { get; set; }

        [JsonProperty("lo_seats_total")]
        public string? LoSeatsTotal { get; set; }

        [JsonProperty("lo_seats_taken")]
        public string? LoSeatsTaken { get; set; }

        [JsonProperty("lo_seats_available")]
        public string? LoSeatsAvailable { get; set; }

        [JsonProperty("lo_total_users_requests")]
        public string? LoTotalUsersRequests { get; set; }

        [JsonProperty("lo_publication_id")]
        public string? LoPublicationId { get; set; }

        [JsonProperty("lo_material_type_id")]
        public int? LoMaterialTypeId { get; set; }

        [JsonProperty("lo_interest_tracking_allowed")]
        public bool? LoInterestTrackingAllowed { get; set; }

        [JsonProperty("lo_multiple_sessions_allowed")]
        public bool? LoMultipleSessionsAllowed { get; set; }

        [JsonProperty("lo_waitlist_allowed")]
        public bool? LoWaitlistAllowed { get; set; }

        [JsonProperty("lo_waitlist_auto_register")]
        public bool? LoWaitlistAutoRegister { get; set; }

        [JsonProperty("lo_waitlist_auto_manage")]
        public bool? LoWaitlistAutoManage { get; set; }

        [JsonProperty("lo_owner_names")]
        public string? LoOwnerNames { get; set; }

        [JsonProperty("lo_timezone_id")]
        public string? LoTimezoneId { get; set; }

        [JsonProperty("lo_total_cost")]
        public string? LoTotalCost { get; set; }

        [JsonProperty("lo_test_graders_ids")]
        public string? LoTestGradersIds { get; set; }

        [JsonProperty("lo_test_attempts_allowed")]
        public string? LoTestAttemptsAllowed { get; set; }

        [JsonProperty("lo_test_max_time_allowed")]
        public int? LoTestMaxTimeAllowed { get; set; }

        [JsonProperty("lo_test_max_entries")]
        public string? LoTestMaxEntries { get; set; }

        [JsonProperty("lo_session_selection_allowed")]
        public string? LoSessionSelectionAllowed { get; set; }

        [JsonProperty("lo_admin_session_selection_allowed")]
        public string? LoAdminSessionSelectionAllowed { get; set; }

        [JsonProperty("lo_billing_entity")]
        public string? LoBillingEntity { get; set; }

        [JsonProperty("lo_product_code")]
        public string? LoProductCode { get; set; }

        [JsonProperty("lo_secondary_training_provider_id")]
        public string? LoSecondaryTrainingProviderId { get; set; }

        [JsonProperty("lo_is_part_of_curriculum")]
        public bool? LoIsPartOfCurriculum { get; set; }

        [JsonProperty("lo_connect_item_type_id")]
        public string? LoConnectItemTypeId { get; set; }

        [JsonProperty("training_version_effective_dt")]
        public string? TrainingVersionEffectiveDt { get; set; }

        [JsonProperty("training_version_start_dt")]
        public string? TrainingVersionStartDt { get; set; }

        [JsonProperty("training_version_end_dt")]
        public string? TrainingVersionEndDt { get; set; }

        [JsonProperty("lo_secondary_training_provider")]
        public string? LoSecondaryTrainingProvider { get; set; }

        [JsonProperty("lo_training_ref")]
        public string? LoTrainingRef { get; set; }

        [JsonProperty("lo_owners_ids")]
        public string? LoOwnersIds { get; set; }

        [JsonProperty("lo_type_id")]
        public int? LoTypeId { get; set; }

        [JsonProperty("lo_ref")]
        public string? LoRef { get; set; }

        [JsonProperty("lo_email_option_id")]
        public int? LoEmailOptionId { get; set; }

        [JsonProperty("lo_root_title")]
        public string? LoRootTitle { get; set; }

        [JsonProperty("lo_provider")]
        public string? LoProvider { get; set; }

        [JsonProperty("lo_provider_address1")]
        public string? LoProviderAddress1 { get; set; }

        [JsonProperty("lo_provider_address2")]
        public string? LoProviderAddress2 { get; set; }

        [JsonProperty("lo_provider_mailstop")]
        public string? LoProviderMailstop { get; set; }

        [JsonProperty("lo_provider_city")]
        public string? LoProviderCity { get; set; }

        [JsonProperty("lo_provider_state")]
        public string? LoProviderState { get; set; }

        [JsonProperty("lo_provider_zip")]
        public string? LoProviderZip { get; set; }

        [JsonProperty("lo_created_by")]
        public string? LoCreatedBy { get; set; }

        [JsonProperty("lo_created_by_fullname")]
        public string? LoCreatedByFullname { get; set; }

        [JsonProperty("lo_created_by_user_ref")]
        public string? LoCreatedByUserRef { get; set; }

        [JsonProperty("lo_modified_by")]
        public string? LoModifiedBy { get; set; }

        [JsonProperty("lo_modified_by_fullname")]
        public string? LoModifiedByFullname { get; set; }

        [JsonProperty("lo_modified_by_user_ref")]
        public string? LoModifiedByUserRef { get; set; }

        [JsonProperty("lo_timezone_code")]
        public string? LoTimezoneCode { get; set; }

        [JsonProperty("lo_provider_active")]
        public string? LoProviderActive { get; set; }

        [JsonProperty("lo_currency_code")]
        public string? LoCurrencyCode { get; set; }

        [JsonProperty("lo_currency_symbol")]
        public string? LoCurrencySymbol { get; set; }

        [JsonProperty("lo_language")]
        public string? LoLanguage { get; set; }

        [JsonProperty("lo_status")]
        public string? LoStatus { get; set; }

        [JsonProperty("classification")]
        public string? Classification { get; set; }

        [JsonProperty("lo_billing_entity_name")]
        public string? LoBillingEntityName { get; set; }

        [JsonProperty("lo_postwork_titles")]
        public string? LoPostworkTitles { get; set; }

        [JsonProperty("lo_prework_titles")]
        public string? LoPreworkTitles { get; set; }

        [JsonProperty("lo_skills")]
        public string? LoSkills { get; set; }

        [JsonProperty("lo_material_type_active")]
        public bool? LoMaterialTypeActive { get; set; }

        [JsonProperty("lo_publication_published")]
        public string? LoPublicationPublished { get; set; }

        [JsonProperty("lo_publication_created_by")]
        public string? LoPublicationCreatedBy { get; set; }

        [JsonProperty("lo_publication_created_dt")]
        public string? LoPublicationCreatedDt { get; set; }

        [JsonProperty("training_deactivation_dt")]
        public string? TrainingDeactivationDt { get; set; }

        [JsonProperty("lo_root_ref")]
        public string? LoRootRef { get; set; }

        [JsonProperty("keywords")]
        public string? Keywords { get; set; }

        [JsonProperty("lo_title")]
        public string? LoTitle { get; set; }

        [JsonProperty("lo_instructor")]
        public string? LoInstructor { get; set; }

        [JsonProperty("lo_eval_01")]
        public int? LoEval01 { get; set; }

        [JsonProperty("lo_eval_02")]
        public string? LoEval02 { get; set; }

        [JsonProperty("lo_eval_03")]
        public string? LoEval03 { get; set; }

        [JsonProperty("training_latest_version")]
        public string? TrainingLatestVersion { get; set; }

        [JsonProperty("electronic_signature_required")]
        public bool? ElectronicSignatureRequired { get; set; }

        [JsonProperty("course_code")]
        public string? CourseCode { get; set; }

        [JsonProperty("is_ojt_enabled")]
        public string? IsOjtEnabled { get; set; }

        [JsonProperty("lo_material_status")]
        public string? LoMaterialStatus { get; set; }

        [JsonProperty("lo_is_mobile_compatible")]
        public bool? LoIsMobileCompatible { get; set; }

        [JsonProperty("is_latest_training_version")]
        public bool? IsLatestTrainingVersion { get; set; }

        [JsonProperty("_last_touched_dt_utc")]
        public DateTime? LastTouchedDtUtc { get; set; }

        [JsonProperty("event_min_enrollment")]
        public string? EventMinEnrollment { get; set; }

        [JsonProperty("event_max_enrollment")]
        public string? EventMaxEnrollment { get; set; }

        [JsonProperty("is_available_offline")]
        public bool? IsAvailableOffline { get; set; }

        [JsonProperty("is_available_offline_network")]
        public bool? IsAvailableOfflineNetwork { get; set; }

        [JsonProperty("browser_compatibility_mode")]
        public string? BrowserCompatibilityMode { get; set; }

        [JsonProperty("online_course_protocol_id")]
        public string? OnlineCourseProtocolId { get; set; }

        [JsonProperty("is_excluded_from_recommendations")]
        public bool? IsExcludedFromRecommendations { get; set; }

    }
}
