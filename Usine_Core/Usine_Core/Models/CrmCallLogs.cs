using System;
namespace Usine_Core.Models
{
    public class CrmCallLogs
    {
        public int Id { get; set; }           // Primary Key
        public int leadid { get; set; }
        public string ContactId { get; set; }   // Nullable Foreign Key to Contact
        public int? LeadOwnerId { get; set; } // Nullable Foreign Key to Lead Owner
        public int? CallTypes { get; set; }   // Nullable Call Type (Inbound/Outbound)
        public DateTime? CallDate { get; set; } // Nullable Date of the Call
        public string Comments { get; set; }  // Nullable Call-related Comments
        public string branchid { get; set; }
        public int customercode { get; set; }
        public string callernotes { get; set; }
        public string customernotes { get; set; }
        public int reasonforcall { get; set; }
        public DateTime? CreatedAt { get; set; } // Record Creation Timestamp
        public int? CreatedBy { get; set; }   // Nullable CreatedBy field
        public DateTime? ModifiedAt { get; set; } // Nullable Modification Timestamp
        public int? ModifiedBy { get; set; }  // Nullable ModifiedBy field
        public int customer_id { get; set; }
    }
}
