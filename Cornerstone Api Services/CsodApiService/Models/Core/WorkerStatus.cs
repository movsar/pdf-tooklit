namespace CornerstoneApiServices.Models.Core
{
    public class WorkerStatus
    {
        public bool Absent { get; set; }
        public bool Active { get; set; }
        public string LastHireDate { get; set; }
        public string OriginalHireDate { get; set; }
    }
}
