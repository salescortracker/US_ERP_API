using System;
namespace Usine_Core.Models
{
    public class CustomerInward
    {
        public int RecordId { get; set; }
        public int InwardNo { get; set; }
        public int GpNo { get; set; }
        public string NameOfCompany { get; set; }
        public string ItemDescription { get; set; }
        public decimal Size { get; set; }
        public int Process { get; set; }

        public decimal RecordedQuantity { get; set; }
        public int DispatchQuantity { get; set; }
        public int DispatchRegisterNo { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public int BalanceQuantity { get; set; }
        public string Signature { get; set; }
        public string BranchId { get; set; }
        public int CustomerCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
