using System;
namespace Usine_Core.others.dtos
{
    public class CrmLeadStatusDto
    {
        public int Id { get; set; }
        public int? StageId { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
        public int? RecStatus { get; set; }
    }
}
