using System;
namespace Usine_Core.others.dtos
{
    public class LeadManagementDto
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        public string Customer { get; set; }
        public int? BranchId { get; set; }
        public int? CustomerCode { get; set; }
        public int? LeadGroup { get; set; }
        public int? Status { get; set; }
        public int? LeadOwner { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; } // Changed to varchar(100)
        public string BusinessEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public int? PhoneNumber { get; set; }
        public int? AlternateNumber { get; set; }
        public int? LeadStatus { get; set; }
        public int? LeadSource { get; set; }
        public int? LeadStage { get; set; }
        public string Website { get; set; }
        public int Industry { get; set; }
        public int? NumberOfEmployees { get; set; }
        public decimal? AnnualRevenue { get; set; }
        public string Rating { get; set; }
        public string EmailOutputFormat { get; set; }
        public string SkypeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string LeadOwnerName { get; set; }

        public string LeadStatusName { get; set; }

        public string LeadSourceName { get; set; }

        public string LeadStageName { get; set; }

        public string IndustryName { get; set; }

        public string LeadGroups { get; set; }
    }
}
