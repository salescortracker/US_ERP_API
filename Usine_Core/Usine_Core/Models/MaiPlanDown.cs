using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiPlanDown
    {
        public int RecordId { get; set; }
        public string DownReportedUser { get; set; }
        public DateTime? Dat { get; set; }
        public DateTime? DownFrom { get; set; }
        public DateTime? DownTo { get; set; }
        public string Reason { get; set; }
        public string Descriptio { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
    }
}
