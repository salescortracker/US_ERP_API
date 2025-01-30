using System;
namespace Usine_Core.others.dtos
{
    public class InvLabDto
    {
        public int? RecordID { get; set; }
        public string LabCode { get; set; }
        public string LabName { get; set; }
        public string ChemicalName { get; set; }
        public string Description { get; set; }
        public string LabIncharge { get; set; }
        public int? Customer { get; set; }
        public int? Status { get; set; }

        //public string StatusName { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
