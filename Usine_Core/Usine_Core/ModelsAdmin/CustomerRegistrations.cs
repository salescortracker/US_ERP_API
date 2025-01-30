using System;
using System.Collections.Generic;

namespace Usine_Core.ModelsAdmin
{
    public partial class CustomerRegistrations
    {
        public CustomerRegistrations()
        {
            CrmTickets = new HashSet<CrmTickets>();
            CustomerReceiptsUni = new HashSet<CustomerReceiptsUni>();
        }

        public int CustomerId { get; set; }
        public string Seq { get; set; }
        public DateTime? Dat { get; set; }
        public string Customer { get; set; }
        public string Addr { get; set; }
        public string Country { get; set; }
        public string Stat { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string ProductId { get; set; }
        public int? VendorId { get; set; }
        public int? MaxBranches { get; set; }
        public int? MaxOutlets { get; set; }
        public string DefaultUser { get; set; }
        public string CurrencySymbol { get; set; }
        public string Currency { get; set; }
        public string Coins { get; set; }
        public string Fiscal { get; set; }
        public string Schem { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string servername { get; set; }
        public string databasename { get; set; }
        public string companyName { get; set; }
        public string ownerName { get; set; }
        public string pocName { get; set; }
        public string pocEmail { get; set; }
        public string ownerEmail { get; set; }
        public string headCount { get; set; }
        public string websiteURL { get; set; }
        public string industry { get; set; }
        public string location { get; set; }
        public string contactNo { get; set; }
        public string urldet { get; set; }
        public string dispatch_email { get; set; }
        public string mode { get; set; }
        public string noofLogins { get; set; }
        public string other { get; set; }
        public virtual ProductDetails Product { get; set; }
        public virtual ICollection<CrmTickets> CrmTickets { get; set; }
        public virtual ICollection<CustomerReceiptsUni> CustomerReceiptsUni { get; set; }
    }
}
