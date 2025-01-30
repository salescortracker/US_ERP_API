
namespace Usine_Core.others.dtos
{
    public class crmLeadQuotationsDto
    {
        public int Id { get; set; }
        public int? LeadId { get; set; }
        public int? ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal? Qty { get; set; }
        public string Um { get; set; }
        public int? LeadDays { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Disper { get; set; }
        public int? Tax { get; set; }
        public decimal? BaseAmt { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Others { get; set; }
        public decimal? TotalAmt { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
        public string leadquotename { get; set; }
        public int? customer_id { get; set; }
    }
}
