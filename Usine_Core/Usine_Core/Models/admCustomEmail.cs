namespace Usine_Core.Models
{
    public class admCustomEmail
    {
        public int RecordId { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string EmailCc { get; set; }
        public string EmailTo { get; set; }
        public string EmailSmtp { get; set; }
        public int? EmailPort { get; set; }
        public bool? EmailDefault { get; set; }
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
    }
}
