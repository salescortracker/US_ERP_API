using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiAmcuni
    {
        public int? RecordId { get; set; }
        public DateTime? Dat { get; set; }
        public int? AmcSupplier { get; set; }
        public double? AmcAmount { get; set; }
        public DateTime? AmcFrom { get; set; }
        public DateTime? AmcTo { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
    }
}
