namespace Usine_Core.Models
{
    public class CrmLeadEnquiryLineDetail
    {
        public int LineId { get; set; }
        public int? ItemId { get; set; }
        public int? EnquiryId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        public int? Qty { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? UomId { get; set; }
        public string? UomName { get; set; }
        public int? LeadDays { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalAmt { get; set; }
        public int? CustomerCode { get; set; }
        public string? BranchCode { get; set; }
    }
}
