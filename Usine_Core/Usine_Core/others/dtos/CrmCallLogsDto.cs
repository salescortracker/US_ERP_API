using System;
namespace Usine_Core.others.dtos
{
    public class CrmCallLogsDto
    {
        public int Id { get; set; }           // Primary Key
        public int leadid { get; set; }
        public string ContactId { get; set; }   // Foreign Key to Contact
        public int? LeadOwnerId { get; set; } // Foreign Key to Lead Owner
        public int? CallTypes { get; set; }   // Call Type (Inbound/Outbound)
        public DateTime? CallDate { get; set; } // Date of the Call
        public string Comments { get; set; }  // Call-related Comments
        public string branchid { get; set; }
        public int customercode { get; set; }
        public string callernotes { get; set; }
        public string customernotes { get; set; }
        public int reasonforcall { get; set; }
        public DateTime? CreatedAt { get; set; } // Record Creation Timestamp
        public int? CreatedBy { get; set; }   // CreatedBy
        public DateTime? ModifiedAt { get; set; } // Modification Timestamp
        public int? ModifiedBy { get; set; }  // ModifiedBy

        public string LeadOwnerName { get; set; }
        public string reasonforcallname { get; set; }
        public int customer_id { get; set; }
        public string CallTypesName { get; set; }

    }

}
