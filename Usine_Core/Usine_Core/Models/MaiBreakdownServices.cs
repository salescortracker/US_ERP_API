using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiBreakdownServices
    {
        public long? RecordId { get; set; }
        public int? Sno { get; set; }
        public string Servic { get; set; }
        public double? Amt { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }

        public virtual MaiBreakdownDetails Record { get; set; }
    }
}
