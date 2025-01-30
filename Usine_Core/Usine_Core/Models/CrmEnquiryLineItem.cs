namespace Usine_Core.Models
{
    public class CrmEnquiryLineItem
    {
        public int? RecordId { get; set; }
        public int Sno { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public double? Qty { get; set; }
        public string? Um { get; set; }
        public int? Value { get; set; }
        public int? Discount { get; set; }
        public int? LeadDays { get; set; }
        public string? BranchId { get; set; }
        public int? CustomerCode { get; set; }
        public int? Rate { get; set; }
    }
}
