namespace Usine_Core.others.dtos
{
    public class custom_emailbodyDto
    {
        public int RecordId { get; set; }
        public int admCustomEmailId { get; set; }
        public string admCustomEmail { get; set; }
        public string titleName { get; set; }
        public string customSubject { get; set; }
        public string customBody { get; set; }
        public bool? IsDefault { get; set; }
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
    }
}
