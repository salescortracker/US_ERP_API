using System;
namespace Usine_Core.Models
{
    public class CrmLeadsEvent
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Column("eventTitle")]
        //[MaxLength(255)]
        public string EventTitle { get; set; }

        //[Column("eventTime")]
        public DateTime? EventTime { get; set; }

        //[Column("eventGuests")]
        //[MaxLength(255)]
        public string EventGuests { get; set; }

        //[Column("meetingLink")]
        //[MaxLength(255)]
        public string MeetingLink { get; set; }

        //[Column("meetingLocation")]
        //[MaxLength(255)]
        public string MeetingLocation { get; set; }
        public int leadid { get; set; }
        public string branchid { get; set; }
        public int customercode { get; set; }
        //[Column("createdBy")]
        public int? CreatedBy { get; set; }

        //[Column("createdAt")]
        public DateTime? CreatedAt { get; set; }

        //[Column("modifiedBy")]
        public int? ModifiedBy { get; set; }

        //[Column("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }
        public int customer_id{get;set;}
    }
}
