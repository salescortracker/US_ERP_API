using System;
namespace Usine_Core.Models
{
    public class CrmLeadSoDetail
    {
        public int SoId { get; set; }
        public string ContactName { get; set; }
        public int? ContactId { get; set; }
        public DateTime? SoDate { get; set; }
        public string? Comments { get; set; }
        public string? PaymentTerms { get; set; }
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
        public string? so_seq_id { get; set; }
    }
}
