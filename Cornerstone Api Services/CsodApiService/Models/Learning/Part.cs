namespace CornerstoneApiServices.Models.Learning
{
    public class Part
    {
        public bool IsPartLocationConfirmationRequired { get; set; }
        public List<PartBreak> PartBreaks { get; set; }
        public string PartDescription { get; set; }
        public DateTimeOffset PartEndDateTime { get; set; }
        public List<PartEquipment> PartEquipments { get; set; }
        public List<PartInstructor> PartInstructors { get; set; }
        public string PartLocation { get; set; }
        public string PartId { get; set; }
        public string PartName { get; set; }
        public string PartOccurrence { get; set; }
        public DateTimeOffset PartStartDateTime { get; set; }
        public string ScheduleID { get; set; }
        public Guid ScheduleId { get; set; }
        public string PartDuration { get; set; }
        public DateTimeOffset PartStartDate { get; set; }
        public string PartStartTime { get; set; }
        public DateTimeOffset PartEndDate { get; set; }
        public string PartEndTime { get; set; }
        public string PartTimeZone { get; set; }
        public string TimezoneDescription { get; set; }
        public string TotalPartBreak { get; set; }
    }
    public class PartInstructor
    {
        public string FirstName { get; set; }
        public string HomeLocation { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
    }
    public class PartBreak
    {
        public string Title { get; set; }
        public long Duration { get; set; }
    }
    public class PartEquipment
    {
        public bool IsEquipmentConfirm { get; set; }
        public bool IsQuantityPerStudent { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

}
