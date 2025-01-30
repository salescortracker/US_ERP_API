using System;

namespace Usine_Core.Models
{
    public class crmtelecallleadcust
    {
        public int id { get; set; }
        public int lead_Id { get; set; }
        public int customer_id { get; set; }
        public string branchid { get; set; }
        public int customercode { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdAt { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedAt { get; set; }
    }
}
