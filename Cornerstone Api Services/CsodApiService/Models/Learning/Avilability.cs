using CornerstoneApiServices.Interfaces;

namespace CornerstoneApiServices.Models.Learning
{
    public class Availability : IAvailability
    {
        public string Type { get; set; }
        public bool IncludeSubs { get; set; }
        public bool PreApproved { get; set; }
        public bool RegisterUponApproval { get; set; }
        public string Id { get; set; }
        public string SubType { get; set; }
    }

    public class UserAvailability : IAvailability
    {
        public string __type { get; set; }
        public string Id { get; set; }
        public bool IncludeSubs { get; set; }
        public bool PreApproved { get; set; }
        public bool RegisterUponApproval { get; set; }
    }

    public class GeneralAvailability : IAvailability
    {
        //public string __type { get; set; }
        public bool IncludeSubs { get; set; }
    }
}
