using System;
using System.Drawing.Printing;
namespace Usine_Core.Models
{
    public class CrmLeadEnquiryDetail
    {
        public int EnquiryId { get; set; }
        public string? ContactName { get; set; }
        public int? ContactId { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string? Comments { get; set; }
        public DateTime? FollowupDate { get; set; }
        public decimal? AdditionalCharges { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Subtotal { get; set; }
        public int? LeadId { get; set; }
        public int? customer_id { get; set; }
        public int? CustomerCode { get; set; }
        public string? BranchCode { get; set; }
        public string? enquiry_seq_id { get; set; }

    }
}
