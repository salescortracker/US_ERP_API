using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiPreventiveMaintenanceHistory
    {
        public long RecordId { get; set; }
        public int? EquipId { get; set; }
        public int? PrevMaiId { get; set; }
        public DateTime? Dat { get; set; }
        public DateTime? TransDate { get; set; }
        public string Descr { get; set; }
        public string UsrName { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
    }
}
