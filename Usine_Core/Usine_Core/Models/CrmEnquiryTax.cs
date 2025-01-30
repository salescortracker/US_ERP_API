namespace Usine_Core.Models
{
    public class CrmEnquiryTax
    {
        public int RecordId { get; set; }
        public int Sno { get; set; }
        public string? TaxCode { get; set; }
        public double? TaxPer { get; set; }
        public double? TaxValue { get; set; }
        public string? BranchId { get; set; }
        public int? CustomerCode { get; set; }
        public int? LineId { get; set; }
    }
}
