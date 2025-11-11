namespace CornerstoneApiServices.Models.Learning
{
    public class LrnSession
    {
        public string InstructorName { get; set; } = string.Empty;
        public string? LoId { get; set; }
        public string Location { get; set; } = string.Empty;
        public List<Part> Parts { get; set; } = new List<Part>();
        public string ProviderName { get; set; } = string.Empty;
        public string SessionNumber { get; set; } = string.Empty;
        public string SessionUrl { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = null;
        public string StartTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
