using System;
namespace Usine_Core.Models
{
    public class CrmLeadQuotationDetail
    {
        public int QuotationId { get; set; }
        public string? ContactName { get; set; }
        public int? ContactId { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string? Comments { get; set; }
        public DateTime? QuotationValidity { get; set; }
        public string? Terms { get; set; }
        public string? DeliveryTerms { get; set; }
        public string? BillingAddress { get; set; }
        public string? ShippingAddress { get; set; }
        public decimal? AdditionalCharges { get; set; }
        public decimal? FreightCharge { get; set; }
        public decimal? CustomsDuties { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Subtotal { get; set; }
        public int? LeadId { get; set; }
        public int? customer_id { get; set; }
        public int? CustomerCode { get; set; }
        public string? BranchCode { get; set; }
        public string? quotation_seq_id { get; set; }
    }
}
