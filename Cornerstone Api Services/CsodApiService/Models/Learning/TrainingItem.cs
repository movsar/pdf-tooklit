namespace CornerstoneApiServices.Models.Learning
{
    public class TrainingItem
    {
        public string __type { get; set; }
        public string ObjectId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Provider { get; set; }
        public string Descr { get; set; }
        public string DeepLinkURL { get; set; }
        public List<object> Subjects { get; set; }
        public List<AvailableLanguage> AvailableLanguages { get; set; }
        public List<object> Recommendations { get; set; }
        public string MaterialType { get; set; }
        public string PriceCurrency { get; set; }
        public double PriceAmount { get; set; }
        public string Duration { get; set; }
        public string Url { get; set; }
        public List<Field> Fields { get; set; }
    }
}
