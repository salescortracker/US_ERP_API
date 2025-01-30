using System;

namespace Usine_Core.Models
{
    public class usrVisitorTrack
    {
        public int recordId { get; set; }
        public DateTime? customer_visit_date { get; set; }
        public string ip_address { get; set; }
        public string country { get; set; }     
        public string customer_code { get; set; }
    }
}
