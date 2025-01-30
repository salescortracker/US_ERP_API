using System;
namespace Usine_Core.others.dtos
{
    public class crmtelecallleadcustdto
    {
        public int id { get; set; }
        public int lead_Id { get; set; }
        public int customer_id { get; set; }
        public string branchid { get; set; }
        public int customercode { get; set; }
        public int contactid { get; set; }
        public int reasonforcall { get; set; }
        public int calltypes { get; set; }
        public string callernotes { get; set; }
        public string customernotes { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdAt { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedAt { get; set; }
    }
}
