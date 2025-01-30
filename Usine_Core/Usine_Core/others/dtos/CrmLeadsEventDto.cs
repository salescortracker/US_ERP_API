using System;
namespace Usine_Core.others.dtos
{
    public class CrmLeadsEventDto
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public DateTime? EventTime { get; set; }
        public string EventGuests { get; set; }
        public string MeetingLink { get; set; }
        public string MeetingLocation { get; set; }
        public int leadid { get; set; }
        public string branchid { get; set; }
        public int customercode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int customer_id { get; set; }
        public string? CustomerName { get; set; }
        public string? LeadName { get; set; }
    }
}
