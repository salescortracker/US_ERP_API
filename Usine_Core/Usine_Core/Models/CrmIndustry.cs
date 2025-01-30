using System;
namespace Usine_Core.Models
{
    public class CrmIndustry
    {
        public int Id { get; set; }                    // Primary Key
        public string Description { get; set; }         // Industry description
        public int? RecStatus { get; set; }             // Record status (active/inactive)
        public string BranchId { get; set; }            // Branch ID
        public string CustomerCode { get; set; }        // Customer Code
        public int? CreatedBy { get; set; }             // User who created the record
        public DateTime? CreatedAt { get; set; }        // Record creation timestamp
        public int? ModifiedBy { get; set; }            // User who last modified the record
        public DateTime? ModifiedAt { get; set; }       // Last modified timestamp
    }
}
