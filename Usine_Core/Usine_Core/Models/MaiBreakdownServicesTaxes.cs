using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiBreakdownServicesTaxes
    {
        public long? RecordId { get; set; }
        public int? Sno { get; set; }
        public string Taxid { get; set; }
        public double? Taxper { get; set; }
        public double? TaxValue { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }

        public virtual MaiBreakdownDetails Record { get; set; }
    }
}
