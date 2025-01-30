using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiAcmdet
    {
        public int? RecordId { get; set; }
        public int? Sno { get; set; }
        public int? EquipId { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }

        public virtual MaiInsurancesUni Record { get; set; }
    }
}
