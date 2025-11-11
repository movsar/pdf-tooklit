namespace CornerstoneApiServices.Models.Learning
{
    public class PricingItem
    {
        public int Delivery { get; set; }
        public int Discount { get; set; }
        public string EffectiveFromDate { get; set; }
        public string EffectiveToDate { get; set; }
        public List<LoPricingItem> LoPricingItems { get; set; }
        public string PriceCurrency { get; set; }
        public string PriceType { get; set; }
        public int Registration { get; set; }
        public int UnitPrice { get; set; }
    }

    public class LoPricingItem
    {
        public bool IncludeSubordinateOus { get; set; }
        public string OuId { get; set; }
        public string OuTypeId { get; set; }
    }

    public class TrainingUnitPricingItem
    {
        public List<LoPricingItem> LoTrainingUnitPricingItems { get; set; }
        public string TrainingUnitPriceType { get; set; }
    }
}
