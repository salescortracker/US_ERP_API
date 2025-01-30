using System;
namespace Usine_Core.Models
{
    public class crmLeadContact
    {
        public int Id { get; set; }
        public int LeadId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string BranchId { get; set; }
        public int CustomerCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int customer_id{ get; set; }
        public bool? PrimaryContact { get; set; }
    }
}
