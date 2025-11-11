using CornerstoneApiServices.Models.Learning;
using CustomTranscript.App.Helpers;

namespace CustomTranscript.App.Models
{
    public class PilotLrnTranscriptCustomFieldsDto
    {
        public double CreditCEU { get; set; }
        public double CreditCPD { get; set; }
        public double CreditLU { get; set; }
        public double CreditPDH { get; set; }
        public double CreditLUHSW { get; set; }
        public string? CreditType { get; set; }
        public double? Duration { get; set; }
        public bool? Required { get; set; }
        public string? TargetAudience { get; set; }
        public PilotLrnTranscriptCustomFieldsDto(List<CustomField> fields)
        {
            SetValues(fields.ToDictionary(f => f.PublicName, f => f.Value));
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
                    case Constants.CreditType:
                        CreditType = keyValues[title];
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
