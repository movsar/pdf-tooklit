namespace CornerstoneApiServices.Models
{
    public class CapabilitieUserProfile
    {
        public string UserID { get; set; }
        public string CapabilityID { get; set; }
        public string Source { get; set; }
        public string ExternalID { get; set; }
        public string UserCapabilityStatus { get; set; }

        public CapabilitieUserProfile(string userId, string capabilityId, string source, string externalId, string userCapabilityStatus)
        {
            UserID = userId;
            CapabilityID = capabilityId;
            Source = source;
            ExternalID = externalId;
            UserCapabilityStatus = userCapabilityStatus;
        }
    }
}
