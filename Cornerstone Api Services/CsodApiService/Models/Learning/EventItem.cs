namespace CornerstoneApiServices.Models.Learning
{
    public class EventItem
    {
        public string AbilityToSelectSessions { get; set; }
        public bool AdvanceRegistrationPreApproved { get; set; }
        public bool AdvanceRegistrationRegister { get; set; }
        public int AdvancedRegistrationPeriod { get; set; }
        public bool AllowAdvanceRegistration { get; set; }
        public bool AllowWaitlist { get; set; }
        public bool CopyAvailabilityToNewSessions { get; set; }
        public int DefaultPrice { get; set; }
        public string DefaultPriceCurrency { get; set; }
        public string Description { get; set; }
        public string EventName { get; set; }
        public string EventNumber { get; set; }
        public string Keywords { get; set; }
        public string LoTrainPriceType { get; set; }
        public int MaximumRegistration { get; set; }
        public int MinimumRegistration { get; set; }
        public string Objectives { get; set; }
        public bool OptionAllowInterestTracking { get; set; }
        public bool OptionAllowUsers { get; set; }
        public bool OptionsActive { get; set; }
        public List<PricingItem> PricingItems { get; set; }
        public string PrimaryProviderName { get; set; }
        public int RegistrationDeadlineNumber { get; set; }
        public string RegistrationDeadlinePeriod { get; set; }
        public string RegistrationDeadlineType { get; set; }
        public string SecondaryProviderName { get; set; }
        public int StudentGrantedDays { get; set; }
        public int StudentGrantedHours { get; set; }
        public string TrainingContactId { get; set; }
        public int TrainingUnitDefaultPrice { get; set; }
        public List<TrainingUnitPricingItem> TrainingUnitPricingItems { get; set; }
        public int WaitlistExpiresDays { get; set; }
        public int WaitlistExpiresHours { get; set; }
    }
}
