using System;
namespace Usine_Core.Models
{
    public class CrmCallTypes
    {
        public int Id { get; set; }                // Primary key
        public string Description { get; set; }    // Description of the call type
        public string BranchId { get; set; }
        public int customercode { get; set; }
        public DateTime? CreatedAt { get; set; }   // Timestamp for record creation
        public int? CreatedBy { get; set; }        // ID of the user who created the record
        public DateTime? ModifiedAt { get; set; }  // Timestamp for record modification
        public int? ModifiedBy { get; set; }       // ID of the user who modified the record
    }
}
