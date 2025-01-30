using System;
namespace Usine_Core.others.dtos
{
    public class crmleadcustSaleOrderDetdto
    {
        public int recordId { get; set; }
        public int sno { get; set; }
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemDescription { get; set; }
        public double qty { get; set; }
        public int um { get; set; }
        public double rat { get; set; }
        public string tax { get; set; }
        public float baseorder { get; set; }
        public float discount { get; set; }
        public float taxes { get; set; }
        public float others { get; set; }
        public float total { get; set; }
        public double discPer { get; set; }
        public DateTime reqdBy { get; set; }
        public int order_status { get; set; }
        public int mode_of_payment { get; set; }
        public string order_description { get; set; }
        public DateTime delivery_date { get; set; }
        public string branchId { get; set; }
        public int customerCode { get; set; }
        public int lead_id { get; set; }
        public int customer_id { get; set; }
    }
}
