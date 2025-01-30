using System;
namespace Usine_Core.Models
{
    public class Feedbackform
    {
        public int RecordId { get; set; }

        public string From { get; set; }

        public string Details { get; set; }

        public DateTime? Date { get; set; }

        public string QualityOfProduct { get; set; }

        public string DeliverySchedule { get; set; }

        public string Communication { get; set; }

        public string RequestElaborateAreas { get; set; }

        public string Signature { get; set; }

        public string Designation { get; set; }

        public int AverageScore { get; set; }

        public string SignatureOfMarketingManager { get; set; }

        public int BranchId { get; set; }

        public int CustomerCode { get; set; }
    }
}
