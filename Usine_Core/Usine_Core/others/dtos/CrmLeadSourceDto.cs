using System;
namespace Usine_Core.others.dtos
{
    public class CrmLeadSourceDto
    {
        public int Id { get; set; }                   // ID of the Lead Source
        public string Description { get; set; }       // Description of the Lead Source
        public int? RecStatus { get; set; }           // Record Status (active/inactive)
        public string BranchId { get; set; }          // Branch ID
        public string CustomerCode { get; set; }      // Customer Code
        public int? CreatedBy { get; set; }           // Created By (user ID)
        public DateTime? CreatedAt { get; set; }      // Created At (timestamp)
        public int? ModifiedBy { get; set; }          // Modified By (user ID)
        public DateTime? ModifiedAt { get; set; }     // Modified At (timestamp)
    }
}
