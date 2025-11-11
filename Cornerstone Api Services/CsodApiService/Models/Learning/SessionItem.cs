namespace CornerstoneApiServices.Models.Learning
{
    public class SessionItem
    {
        public string EventID { get; set; }
        public string EventLoRef { get; set; }
        public string InstructorName { get; set; }
        public string LoID { get; set; }
        public int Locator { get; set; }
        public int MaximumRegistration { get; set; }
        public int MinimumRegistration { get; set; }
        public List<Part> Parts { get; set; }
        public int RegistrationDeadlineNumber { get; set; }
        public string RegistrationDeadlinePeriod { get; set; }
        public string RegistrationDeadlineType { get; set; }
        public int RegistrationDeadlineTypeEnum { get; set; }
        public string SessionID { get; set; }
        public string SessionOccurrence { get; set; }
        public string TrainingContactId { get; set; }
    }
}
