using CornerstoneApiServices.Models;
using CornerstoneApiServices.Models.Learning;
using CustomTranscript.App.Helpers;

namespace CustomTranscript.App.Models
{
    public class PilotCustomFieldsDto
    {
        public string? ContentOwnerEmail { get; set; }
        public string? ContentOwnerName { get; set; }
        public double CreditCEU { get; set; }
        public double CreditCPD { get; set; }
        public double CreditLU { get; set; }
        public double CreditPDH { get; set; }
        public double CreditLUHSW { get; set; }
        public string? CreditExpiration { get; set; }
        public string? CreditType { get; set; }
        public double? Duration { get; set; }
        public string? ExternalVendor { get; set; }
        public string? HDRVersionNumber { get; set; }
        public string? LinksAttachedResources { get; set; }
        public string? MainCategory { get; set; }
        public string? PeopleReferencedByName { get; set; }
        public string? QC360ReviewLink { get; set; }
        public string? QCChangesImplemented { get; set; }
        public string? QCLastReviewDate { get; set; }
        public bool? Required { get; set; }
        public string? TargetAudience { get; set; }
        private PilotCustomFieldsDto() { }
        public PilotCustomFieldsDto(List<Field> fields)
        {
            SetValues(fields.ToDictionary(f => f.Title, f => f.Value));
        }
      
        private void SetValues(Dictionary<string, string> keyValues)
        {
            foreach (var title in keyValues.Keys)
            {
                switch (title)
                {
                    case Constants.CreditLU:
                        CreditLU = ReportingHelper.GetDouble(keyValues[title]);
                        break;
                    case Constants.CreditCEU:
                        CreditCEU = ReportingHelper.GetDouble(keyValues[title]);
                        break;
                    case Constants.CreditCPD:
                        CreditCPD = ReportingHelper.GetDouble(keyValues[title]);
                        break;
                    case Constants.CreditLUHSW:
                        CreditLUHSW = ReportingHelper.GetDouble(keyValues[title]);
                        break;
                    case Constants.CreditPDH:
                        CreditPDH = ReportingHelper.GetDouble(keyValues[title]);
                        break;
                    case Constants.ContentOwnerEmail:
                        ContentOwnerEmail = keyValues[title];
                        break;
                    case Constants.ContentOwnerName:
                        ContentOwnerName = keyValues[title];
                        break;
                    case Constants.CreditExpiration:
                        CreditExpiration = keyValues[title];
                        break;
                    case Constants.CreditType:
                        CreditType = keyValues[title];
                        break;
                    case Constants.ExternalVendor:
                        ExternalVendor = keyValues[title];
                        break;
                    case Constants.HDRVersionNumber:
                        HDRVersionNumber = keyValues[title];
                        break;
                    case Constants.LinksAttachedResources:
                        LinksAttachedResources = keyValues[title];
                        break;
                    case Constants.MainCategory:
                        MainCategory = keyValues[title];
                        break;
                    case Constants.PeopleReferencedByName:
                        PeopleReferencedByName = keyValues[title];
                        break;
                    case Constants.QC360ReviewLink:
                        QC360ReviewLink = keyValues[title];
                        break;
                    case Constants.QCChangesImplemented:
                        QCChangesImplemented = keyValues[title];
                        break;
                    case Constants.QCLastReviewDate:
                        QCLastReviewDate = keyValues[title];
                        break;
                    case Constants.Required:
                        Required = ReportingHelper.GetBoolean(keyValues[title]);
                        break;
                    case Constants.TargetAudience:
                        TargetAudience = keyValues[title];
                        break;
                    case Constants.Duration:
                        Duration = ReportingHelper.GetDouble(keyValues[title]);
                        break;
                    default:
                        // Handle unrecognized field names if needed
                        break;
                }
            }
        }
    }
}
