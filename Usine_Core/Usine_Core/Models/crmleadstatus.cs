using System;
namespace Usine_Core.Models
{
    public class crmleadstatus
    {
        public int Id { get; set; }                    // Primary Key
        public int? StageId { get; set; }              // Foreign Key to Lead Stage
        public string Description { get; set; }        // Lead status description
        public int? CreatedBy { get; set; }            // User who created the record
        public DateTime? CreatedAt { get; set; }       // Record creation timestamp
        public int? ModifiedBy { get; set; }           // User who last modified the record
        public DateTime? ModifiedAt { get; set; }      // Last modified timestamp
        public string BranchId { get; set; }           // Branch ID
        public string CustomerCode { get; set; }       // Customer Code
        public int? RecStatus { get; set; }            // Record status (active/inactive)
    }
}
