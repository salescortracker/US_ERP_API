using System;
namespace Usine_Core.Models
{
    public class crmleadcity
    {
        public int Id { get; set; }                // Primary key
        public string Description { get; set; }    // Description of the call type
        public string Branch_Id { get; set; }
        public int customer_code { get; set; }
        public DateTime? created_at { get; set; }   // Timestamp for record creation
      
        public DateTime? modified_at { get; set; }  // Timestamp for record modification
      
    }
}
