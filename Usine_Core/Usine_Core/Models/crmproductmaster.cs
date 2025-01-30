using System;

namespace Usine_Core.Models
{
    public class crmproductmaster
    {
        public int RecordId { get; set; }
        public int crmprogroupId { get; set; }
        public int crmprocategoryId { get; set; }
        public string serviceName { get; set; }
        public string productCode { get; set; }
        public string productDesc { get; set; }
        public string uom { get; set; }
        public string unit { get; set; }
        public decimal unitPrice { get; set; }
        public decimal dealerPrice { get; set; }
        public decimal wholesalePrice { get; set; }      
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
