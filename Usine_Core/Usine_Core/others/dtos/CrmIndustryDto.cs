using System;
namespace Usine_Core.others.dtos
{
    public class CrmIndustryDto
    {
        public int Id { get; set; }                    // Primary Key
        public string Description { get; set; }         // Industry description
        public int? RecStatus { get; set; }             // Record status
        public string BranchId { get; set; }            // Branch ID
        public string CustomerCode { get; set; }        // Customer Code
        public int? CreatedBy { get; set; }             // Created By user ID
        public DateTime? CreatedAt { get; set; }        // Creation timestamp
        public int? ModifiedBy { get; set; }            // Modified By user ID
        public DateTime? ModifiedAt { get; set; }       // Modified timestamp
    }
}
