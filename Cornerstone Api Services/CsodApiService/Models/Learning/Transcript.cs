namespace CornerstoneApiServices.Models.Learning
{
    public class Transcript
    {
        public string UserId { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public string UserGuid { get; set; } = null!;
        public string LearningObjectId { get; set; } = null!;
        public string? LearningTitle { get; set; }
        public string? TrainingType { get; set; }
        public int? RegistrationNumber { get; set; }
        public bool IsGreatestRegistrationNumber { get; set; }
        public string? Status { get; set; }
        public string? StatusGroup { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public DateTime? AssignedDate { get; set; }
        public int? AssignedByUserId { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? TrainingPurpose { get; set; }
        public DateTime? AvailableDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool? IsSuggested { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsStandalone { get; set; }
        public int? TrainingVersion { get; set; }
        public string? ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public bool? Passed { get; set; }
        public int? Score { get; set; }
        public int? AttemptsLeft { get; set; }
        public string? TrainingHours { get; set; }
        public string? ObservedTrainingHours { get; set; }
        public bool? IsRemoved { get; set; }
        public string? Badge { get; set; }
        public int? BadgePoints { get; set; }
        public int? TrainingPoints { get; set; }
        public bool? IsArchived { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
