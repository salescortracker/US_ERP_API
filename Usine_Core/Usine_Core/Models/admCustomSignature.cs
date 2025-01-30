namespace Usine_Core.Models
{
    public class admCustomSignature
    {
        public int RecordId { get; set; }
        public int admCustomEmailId { get; set; }
        public string customSignature { get; set; }
        public string customSignatureText { get; set; }
        public bool? IsDefault { get; set; }
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
    }
}
