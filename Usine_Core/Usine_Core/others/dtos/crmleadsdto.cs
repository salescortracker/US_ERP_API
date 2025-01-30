using System;

namespace Usine_Core.others.dtos
{
    public class crmleadsdto
    {
        public int id { get; set; }
        public int code { get; set; }
        public string customer { get; set; }
        public string branch_id { get; set; }
        public int customer_code { get; set; }
        public string lead_group { get; set; }
        public string status { get; set; }
        public int? lead_owner { get; set; }
        public string company { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string description { get; set; }
        public string business_email { get; set; }
        public string secondary_email { get; set; }
        public string phonenumber { get; set; }
        public string alternate_number { get; set; }
        public int? lead_status { get; set; }
        public int? lead_source { get; set; }
        public int? lead_stage { get; set; }
        public string website { get; set; }
        public string industry { get; set; }
        public string numberofemployees { get; set; }
        public decimal annual_revenue { get; set; }
        public string rating { get; set; }
        public string emailoutputformat { get; set; }
        public string skypeid { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string twitter { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public string LeadOwnerName { get; set; }
        public string LeadStatusName { get; set; }
        public string LeadSourceName { get; set; }
        public string LeadStageName { get; set; }
        public string IndustryName { get; set; }
        public string type_lead_cus { get; set; }
        public int? customer_id { get; set; }
        public bool? convert_customer { get; set; }
        public string customerconverted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public int referencecustomer_id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? modified_at { get; set; }
    }

}
