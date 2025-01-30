﻿using System;
using System.Collections.Generic;

namespace Usine_Core.Models
{
    public partial class PurQuotationUni
    {
        public PurQuotationUni()
        {
            PurQuotationDet = new HashSet<PurQuotationDet>();
            PurQuotationTaxes = new HashSet<PurQuotationTaxes>();
            PurQuotationTerms = new HashSet<PurQuotationTerms>();
        }

        public int? RecordId { get; set; }
        public string Seq { get; set; }
        public DateTime? Dat { get; set; }
        public DateTime? ApprovedDat { get; set; }
        public string Usr { get; set; }
        public string ApprovedUsr { get; set; }
        public string PurchaseType { get; set; }
        public int? RefQuotationId { get; set; }
        public string RefQuotation { get; set; }
        public DateTime? Validity { get; set; }
        public string Reference { get; set; }
        public int? Vendorid { get; set; }
        public string Vendorname { get; set; }
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
        public string Webid { get; set; }
        public double? Baseamt { get; set; }
        public double? Discount { get; set; }
        public double? Taxes { get; set; }
        public double? Others { get; set; }
        public double? TotalAmt { get; set; }
        public int? Pos { get; set; }
        public string BranchId { get; set; }
        public int? CustomerCode { get; set; }
        public string salesorder { get; set; }

        public virtual ICollection<PurQuotationDet> PurQuotationDet { get; set; }
        public virtual ICollection<PurQuotationTaxes> PurQuotationTaxes { get; set; }
        public virtual ICollection<PurQuotationTerms> PurQuotationTerms { get; set; }
    }
}
