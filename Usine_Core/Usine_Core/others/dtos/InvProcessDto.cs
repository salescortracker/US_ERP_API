using System;
namespace Usine_Core.others.dtos
{
    public class InvProcessDto
    {
        public int recordId { get; set; }

        public string processName { get; set; }
        public string branchId { get; set; }
        public int customerCode { get; set; }
        public int created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
