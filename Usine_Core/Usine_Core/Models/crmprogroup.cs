using System;

namespace Usine_Core.Models
{
    public class crmprogroup
    {
        public int RecordId { get; set; }
        public string groupName { get; set; }       
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
