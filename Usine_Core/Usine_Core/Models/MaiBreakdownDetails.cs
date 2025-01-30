using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiBreakdownDetails
    {
        public MaiBreakdownDetails()
        {
            MaiBreakdownServices = new HashSet<MaiBreakdownServices>();
            MaiBreakdownServicesTaxes = new HashSet<MaiBreakdownServicesTaxes>();
        }

        public long? RecordId { get; set; }
        public int? EquipId { get; set; }
        public long? RefEmployee { get; set; }
        public DateTime? Dat { get; set; }
        public DateTime? BreakDownDate { get; set; }
        public string ReportedUser { get; set; }
        public string BreakDownDescription { get; set; }
        public int? WorkDisturbanceCheck { get; set; }
        public int? ServiceAssignCheck { get; set; }
        public DateTime? ServiceAssignDate { get; set; }
        public int? ServiceProvider { get; set; }
        public string ServiceAssignUser { get; set; }
        public double? ServiceBaseAmount { get; set; }
        public double? ServiceDiscount { get; set; }
        public double? ServiceTaxes { get; set; }
        public double? ServiceOtherAmt { get; set; }
        public double? ServiceTotalAmount { get; set; }
        public int? ServiceClearanceCheck { get; set; }
        public DateTime? ServiceClearanceDate { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceClearanceReportedBy { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }

        public virtual ICollection<MaiBreakdownServices> MaiBreakdownServices { get; set; }
        public virtual ICollection<MaiBreakdownServicesTaxes> MaiBreakdownServicesTaxes { get; set; }
    }
}
