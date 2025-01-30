using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class MaiInsurancesUni
    {
        public MaiInsurancesUni()
        {
            MaiAcmdet = new HashSet<MaiAcmdet>();
            MaiInsurancesDet = new HashSet<MaiInsurancesDet>();
        }

        public int? RecordId { get; set; }
        public DateTime? Dat { get; set; }
        public int? InsuranceSupplier { get; set; }
        public double? InsuredAmount { get; set; }
        public DateTime? InsuredFrom { get; set; }
        public DateTime? InsuredTo { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }

        public virtual ICollection<MaiAcmdet> MaiAcmdet { get; set; }
        public virtual ICollection<MaiInsurancesDet> MaiInsurancesDet { get; set; }
    }
}
