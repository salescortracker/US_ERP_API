using System;
namespace Usine_Core.Models
{
    public class CrmRemainder
    {
        public int RecordId { get; set; }
        public int leadid { get; set; }
        public string ReminderName { get; set; }
        public DateTime ReminderDate { get; set; }
        public TimeSpan ReminderTime { get; set; }
        public int? reminder_type { get; set; }
        public string Notes { get; set; }
        public string BranchId { get; set; }
        public string CustomerCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int customer_id { get; set; }
    }
}
