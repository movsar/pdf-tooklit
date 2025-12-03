namespace CustomTranscript.App.Models
{
    public class ReportRow
    {
        public DateTime? DateCompleted { get; set; }

        public string CourseName { get; set; } = null!;
        public string ProviderName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;

        public double CEUs { get; set; } = 0;
        public double CPDs { get; set; } = 0;
        public double LUs { get; set; } = 0;
        public double PDHs { get; set; } = 0;
        public double HSWs { get; set; } = 0;
    }
}
