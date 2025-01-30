namespace Usine_Core.others.dtos
{
    public class CrmLeadCustEnquiryLineItemdto
    {
        public int RecordId { get; set; }
        public int sno { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public double qty { get; set; }
        public string um { get; set; }
        public int leaddays { get; set; }
        public string branchid { get; set; }
        public int customercode { get; set; }
        public int value { get; set; }
        public int discount { get; set; }
        public int rate { get; set; }
        public int lead_id { get; set; }
        public int customer_id { get; set; }
        public string leaditemname { get; set; }
    }

}
