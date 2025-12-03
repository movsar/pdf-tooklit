namespace CornerstoneApiServices.Models.Learning
{
    internal class ErrorResponse
    {
        public Guid ErrorId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public object Details { get; set; }
        public List<object> Fields { get; set; }

    }
}
