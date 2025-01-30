using System;
namespace Usine_Core.Models
{
    public class CrmEnquiryRegister
    {
        public int RecordId { get; set; }
        public DateTime? Date { get; set; }
        public string EnquiryFrom { get; set; }
        public string EnquiryDetails { get; set; }
        public string ProductRange { get; set; }
        public int? Process { get; set; }
        public string processName { get; set; }
        public string QuotationSubmissionDetails { get; set; }
        public string NegotiationDetails { get; set; }
        public string? OrderAcceptanceDetails { get; set; }
        public DateTime? ExpectedDateOfDelivery { get; set; }
        public DateTime? ActualDateOfDelivery { get; set; }
        public string Remarks { get; set; }
        public string PreparedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
    }
}
