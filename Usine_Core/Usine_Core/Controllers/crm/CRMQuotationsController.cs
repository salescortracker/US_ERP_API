using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usine_Core.Controllers.Accounts;
using Usine_Core.Controllers.Admin;

using Usine_Core.Controllers.Inventory;
using Usine_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;



namespace Usine_Core.Controllers.crm
{
    public class CRMQuotationTotal
    {
        public CrmQuotationUni header { get; set; }
        public List<CrmQuotationDet> lines { get; set; }
        public List<CrmQuotationTerms> terms { get; set; }
        public List<CrmQuotationTaxes> taxes { get; set; }
         public int? traCheck { get; set; }
        public String result { get; set; }
        public UserInfo usr { get; set; }

    }
    public class CompleteCRMQuotationRequirements
    {
        public String seq { get; set; }
        public string dat { get; set; }
        public List<PartyDetails> suppliers { get; set; }
        public List<PartyAddresses> addresses { get; set; }
        public List<InvMaterialDetails> materials { get; set; }
        public List<AdmTaxes> taxes { get; set; }
        public List<InvMaterialsUnitsDetails> units { get; set; }
        public List<PurTerms> terms { get; set; }
        public List<CrmPriceListUni> pricesuni { get; set; }
        public List<CrmPriceListDet> pricesdet { get; set; }
        public List<CrmDiscountListUni> discsuni { get; set; }
        public List<CrmDiscountListDet> discsdet { get; set; }
        public List<CrmTaxAssigningUni> taxesuni { get; set; }
        public List<CrmTaxAssigningDet> taxesdet { get; set; }
        public List<HrdEmployees> employees { get; set; }
         
    }

    public class CRMQuotationsController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();
        private readonly IHostingEnvironment ho;
        public CRMQuotationsController(IHostingEnvironment hostingEnvironment)
        {

            ho = hostingEnvironment;
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/GetSaleQuotationRequirements")]
        public CompleteCRMQuotationRequirements GetSaleQuotationRequirements([FromBody] UserInfo usr)
        {
            CompleteCRMQuotationRequirements tot = new CompleteCRMQuotationRequirements();
            tot.seq = findSeq(usr);

            tot.suppliers =
                  db.PartyDetails.Where(a => a.PartyType == "CUS" && a.CustomerCode == usr.cCode).OrderBy(b => b.PartyName).ToList();
                   

            InvMaterialsController iv = new InvMaterialsController();
            AdminTaxesController ax = new AdminTaxesController();
            tot.addresses = db.PartyAddresses.Where(a => a.CustomerCode == usr.cCode).OrderBy(b => b.RecordId).ThenBy(c => c.Sno).ToList();
            tot.dat = ac.strDate(ac.getPresentDateTime());
            tot.materials = iv.GetInvMaterials(usr);
            tot.taxes = ax.GetTaxes(usr);
            tot.units = iv.GetInvMaterialUnits(usr);
            tot.terms = db.PurTerms.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).OrderBy(b => b.Slno).ToList();
            tot.pricesuni = db.CrmPriceListUni.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();
            tot.employees = (from a in db.HrdEmployees.Where(a => a.Branchid == usr.bCode && a.CustomerCode == usr.cCode)
                             join
b in db.HrdDesignations.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode) on a.Designation equals b.RecordId
                             select new HrdEmployees
                             {
                                 RecordId=a.RecordId,
                                 Empname=a.Empname,
                                 Mobile=a.Mobile,
                                 Tel=b.Designation
                             }).OrderBy(c => c.Empname).ToList();
            tot.pricesdet = (from a in db.CrmPriceListDet.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode)
                             select new CrmPriceListDet
                             {
                                 RecordId = a.RecordId,
                                 Sno = a.Sno,
                                 ProductId = a.ProductId,
                                 Price = a.Price,
                                 TaxId=a.TaxId

                             }).ToList();
            tot.discsuni = db.CrmDiscountListUni.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();
            tot.discsdet = (from a in db.CrmDiscountListDet.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode)
                            select new CrmDiscountListDet
                            {
                                RecordId=a.RecordId,
                                Sno=a.Sno,
                                ProductId=a.ProductId,
                                Discount=a.Discount
                            }).ToList();
            tot.taxesuni = db.CrmTaxAssigningUni.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();
            tot.taxesdet = (from a in db.CrmTaxAssigningDet.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode)
                            select new CrmTaxAssigningDet
                            {
                                RecordId=a.RecordId,
                                Sno=a.Sno,
                                TaxCode=a.TaxCode,
                                Taxper=a.Taxper
                            }).ToList();

            return tot;
        }

        private string findSeq(UserInfo usr)
        {
            int x = 0;
            General g = new General();
            string str = "SQ" + g.right(DateTime.Now.Year.ToString(), 2) + "-";
            var det = db.CrmQuotationUni.Where(a => a.Seq.Contains(str) && a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).Max(b => b.Seq);
            if (det != null)
            {
                x = g.valInt(g.right(det, 5));
            }
            x++;

            return str + g.zeroMake(x, 5);
        }


        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/GetCRMQuotations")]
        public List<CrmQuotationUni> GetCRMQuotations([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = DateTime.Parse(inf.toDate).AddDays(1);
            return db.CrmQuotationUni.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).OrderBy(b => b.Dat).ThenBy(c => c.RecordId).ToList();
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/GetCRMQuotationsForApprovals")]
        public List<CrmQuotationUni> GetCRMQuotationsForApprovals([FromBody] UserInfo usr)
        {
            return db.CrmQuotationUni.Where(a => a.Pos == 1 && a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).OrderBy(b => b.Dat).ThenBy(c => c.RecordId).ToList();
        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/GetCRMQuotation")]
        public CRMQuotationTotal GetCRMQuotation([FromBody] GeneralInformation inf)
        {
            CRMQuotationTotal tot = new CRMQuotationTotal();
            tot.header = db.CrmQuotationUni.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).FirstOrDefault();
            tot.lines = (from a in db.CrmQuotationDet.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                         join b in db.InvUm.Where(a => a.CustomerCode == inf.usr.cCode)
                         on a.Um equals b.RecordId
                         select new CrmQuotationDet
                         {
                             RecordId = a.RecordId,
                             Sno = a.Sno,
                             ItemId = a.ItemId,
                             ItemName = a.ItemName,
                             ItemDescription = a.ItemDescription,
                             Qty = a.Qty,
                             LeadDays = a.LeadDays,
                             DiscPer=a.DiscPer,
                             Um = a.Um,
                             Rat = a.Rat,
                             BranchId = b.Um
                         }).OrderBy(b => b.Sno).ToList();
            tot.taxes = (from a in db.CrmQuotationTaxes.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                         select new CrmQuotationTaxes
                         {
                             RecordId = a.RecordId,
                             Sno = a.Sno,
                             TaxCode = a.TaxCode,
                             TaxPer = a.TaxPer,
                             TaxValue = a.TaxValue,
                             LineId=a.LineId
                         }).OrderBy(b => b.Sno).ToList();

            tot.terms = (from a in db.CrmQuotationTerms.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                         select new CrmQuotationTerms
                         {
                             RecordId = a.RecordId,
                             Sno = a.Sno,
                             Term = a.Term
                         }).OrderBy(b => b.Sno).ToList();
            tot.result = "OK";
           
            return tot;
        }



        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/setCRMQuotation")]
        public TransactionResult setCRMQuotation([FromBody] CRMQuotationTotal tot)
        {
            string msg = "";
            if (ac.screenCheck(tot.usr, 7, 2, 3, (int)tot.traCheck))
            {
                DataBaseContext ggg = new DataBaseContext();
                General gg = new General();
                int? sno = 1;
                try
                {
                    var statuscheck = db.CrmSetup.Where(a => a.SetupCode == "crm_quo" && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                    int? statu = 2;
                    if (statuscheck != null)
                    {
                        statu = gg.valInt(statuscheck.Pos) == 1 ? 2 : 1;
                    }
                    string tramsg = ac.transactionCheck("CRM", tot.header.Dat, tot.usr);
                    if (tramsg == "OK")
                    {
                        switch (tot.traCheck)
                        {
                            case 1:
                                using (var txn = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        tot.header.Seq = findSeq(tot.usr);
                                        tot.header.Dat = ac.DateAdjustFromFrontEnd(tot.header.Dat.Value);
                                        tot.header.Validity = ac.DateAdjustFromFrontEnd(tot.header.Validity.Value);
                                        tot.header.Pos = statu;
                                        tot.header.BranchId = tot.usr.bCode;
                                        tot.header.CustomerCode = tot.usr.cCode;
                                        tot.header.telecallingno = tot.header.telecallingno;
                                        tot.header.enquiryno = tot.header.enquiryno;
                                        tot.header.quotationstatus = tot.header.quotationstatus;
                                        tot.header.RefEmployee = null;
                                        db.CrmQuotationUni.Add(tot.header);
                                        db.SaveChanges();


                                        foreach (var line in tot.lines)
                                        {
                                            line.RecordId = tot.header.RecordId;
                                            line.Sno = sno;
                                            line.BranchId = tot.usr.bCode;
                                            line.CustomerCode = tot.usr.cCode;
                                            sno++;
                                        }
                                        db.CrmQuotationDet.AddRange(tot.lines);

                                        sno = 1;
                                        if (tot.taxes != null)
                                        {
                                            if (tot.taxes.Count() > 0)
                                            {
                                                foreach (var tax in tot.taxes)
                                                {
                                                    tax.RecordId = tot.header.RecordId;
                                                    tax.Sno = sno;
                                                    tax.BranchId = tot.usr.bCode;
                                                    tax.CustomerCode = tot.usr.cCode;
                                                    sno++;
                                                }
                                                db.CrmQuotationTaxes.AddRange(tot.taxes);
                                            }
                                        }
                                        sno = 1;
                                        if (tot.terms != null)
                                        {
                                            if (tot.terms.Count() > 0)
                                            {
                                                foreach (var term in tot.terms)
                                                {
                                                    term.RecordId = tot.header.RecordId;
                                                    term.Sno = sno;
                                                    term.BranchId = tot.usr.bCode;
                                                    term.CustomerCode = tot.usr.cCode;
                                                    sno++;
                                                }
                                                db.CrmQuotationTerms.AddRange(tot.terms);
                                            }
                                        }

                                        db.SaveChanges();

                                        TransactionsAudit audit = new TransactionsAudit();
                                        audit.TraId = (int)tot.header.RecordId;
                                        audit.Descr = "A new Quotation created with Id of " + tot.header.Seq + " from the customer " + tot.header.PartyName;
                                        audit.Usr = tot.usr.uCode;
                                        audit.Tratype = 1;
                                        audit.Transact = "CRM_QUO";
                                        audit.TraModule = "CRM";
                                        audit.Syscode = "Sale Quotation Quotation";
                                        audit.BranchId = tot.usr.bCode;
                                        audit.CustomerCode = tot.usr.cCode;
                                        audit.Dat = ac.getPresentDateTime();
                                        db.TransactionsAudit.Add(audit);
                                        db.SaveChanges();

                                           txn.Commit();
                                        msg = "OK";

                                    }
                                    catch (Exception ex)
                                    {
                                        msg = ex.Message;
                                        txn.Rollback();
                                    }
                                }
                                break;
                            case 2:
                                var headerupd = db.CrmQuotationUni.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                if (headerupd != null)
                                {
                                    headerupd.Dat = ac.DateAdjustFromFrontEnd(tot.header.Dat.Value);
                                    headerupd.Usr = tot.usr.uCode;
                                    headerupd.SaleType = tot.header.SaleType;
                                   
                                   // headerupd.RefQuotation = tot.header.RefQuotation;
                                    headerupd.Validity = tot.header.Validity;
                                    headerupd.Reference = tot.header.Reference;
                                    headerupd.PartyId = tot.header.PartyId;
                                    headerupd.PartyName = tot.header.PartyName;
                                    headerupd.Addr = tot.header.Addr;
                                    headerupd.Country = tot.header.Country;
                                    headerupd.Stat = tot.header.Stat;
                                    headerupd.District = tot.header.District;
                                    headerupd.City = tot.header.City;
                                    headerupd.Zip = tot.header.Zip;
                                    headerupd.Mobile = tot.header.Mobile;
                                    headerupd.Tel = tot.header.Tel;
                                    headerupd.Fax = tot.header.Fax;
                                    headerupd.Email = tot.header.Email;
                                    headerupd.Webid = tot.header.Webid;
                                    headerupd.Baseamt = tot.header.Baseamt;
                                    headerupd.Discount = tot.header.Discount;
                                    headerupd.Taxes = tot.header.Taxes;
                                    headerupd.Others = tot.header.Others;
                                    headerupd.TotalAmt = tot.header.TotalAmt;
                                    headerupd.telecallingno = tot.header.telecallingno;
                                    headerupd.enquiryno = tot.header.enquiryno;
                                    headerupd.quotationstatus = tot.header.quotationstatus;
                                    var linesupd = db.CrmQuotationDet.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                                    var termsupd = db.CrmQuotationTerms.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                                    var taxesupd = db.CrmQuotationTaxes.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                                    if (linesupd.Count() > 0)
                                    {
                                        sno = linesupd.Max(a => a.Sno);
                                    }
                                    else
                                    {
                                        sno = 0;
                                    }
                                    sno++;

                                    foreach (var line in tot.lines)
                                    {
                                        line.RecordId = tot.header.RecordId;
                                        line.Sno = sno;
                                        line.BranchId = tot.usr.bCode;
                                        line.CustomerCode = tot.usr.cCode;
                                        sno++;
                                    }


                                    if (taxesupd.Count() > 0)
                                    {
                                        sno = taxesupd.Max(a => a.Sno);
                                    }
                                    else
                                    {
                                        sno = 0;
                                    }
                                    sno++;
                                    if (tot.taxes != null)
                                    {
                                        if (tot.taxes.Count() > 0)
                                        {
                                            foreach (var tax in tot.taxes)
                                            {
                                                tax.RecordId = tot.header.RecordId;
                                                tax.Sno = sno;
                                                tax.BranchId = tot.usr.bCode;
                                                tax.CustomerCode = tot.usr.cCode;
                                                sno++;
                                            }

                                        }
                                    }
                                    if (termsupd.Count() > 0)
                                    {
                                        sno = termsupd.Max(a => a.Sno);
                                    }
                                    else
                                    {
                                        sno = 0;
                                    }
                                    sno++;
                                    if (tot.terms != null)
                                    {
                                        if (tot.terms.Count() > 0)
                                        {
                                            foreach (var term in tot.terms)
                                            {
                                                term.RecordId = tot.header.RecordId;
                                                term.Sno = sno;
                                                term.BranchId = tot.usr.bCode;
                                                term.CustomerCode = tot.usr.cCode;
                                                sno++;
                                            }

                                        }
                                    }
                                    if (linesupd.Count() > 0)
                                    {
                                        db.CrmQuotationDet.RemoveRange(linesupd);
                                    }
                                    if (taxesupd.Count() > 0)
                                    {
                                        db.CrmQuotationTaxes.RemoveRange(taxesupd);
                                    }
                                    if (termsupd.Count() > 0)
                                    {
                                        db.CrmQuotationTerms.RemoveRange(termsupd);
                                    }

                                    db.CrmQuotationDet.AddRange(tot.lines);
                                    if (tot.taxes.Count() > 0)
                                    {
                                        db.CrmQuotationTaxes.AddRange(tot.taxes);
                                    }
                                    if (tot.terms.Count() > 0)
                                    {
                                        db.CrmQuotationTerms.AddRange(tot.terms);
                                    }
                                    db.SaveChanges();

                                    TransactionsAudit audit = new TransactionsAudit();
                                    audit.TraId = (int)tot.header.RecordId;
                                    audit.Descr = "A new Quotation modified with Id of " + tot.header.Seq + " from the customer " + tot.header.PartyName;
                                    audit.Usr = tot.usr.uCode;
                                    audit.Tratype = 2;
                                    audit.Transact = "CRM_QUO";
                                    audit.TraModule = "CRM";
                                    audit.Syscode = "Sale Quotation";
                                    audit.BranchId = tot.usr.bCode;
                                    audit.CustomerCode = tot.usr.cCode;
                                    audit.Dat = ac.getPresentDateTime();
                                    db.TransactionsAudit.Add(audit);
                                    db.SaveChanges();
                                    msg = "OK";
                                }
                                else
                                {
                                    msg = "No record found";
                                }
                                break;
                            case 3:
                                var headerdel = db.CrmQuotationUni.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                if (headerdel != null)
                                {
                                    var linesdel = db.CrmQuotationDet.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                                    var termsdel = db.CrmQuotationTerms.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                                    var taxesdel = db.CrmQuotationTaxes.Where(a => a.RecordId == tot.header.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                                    if (linesdel.Count() > 0)
                                    {
                                        db.CrmQuotationDet.RemoveRange(linesdel);
                                    }
                                    if (termsdel.Count() > 0)
                                    {
                                        db.CrmQuotationTerms.RemoveRange(termsdel);
                                    }
                                    if (taxesdel.Count() > 0)
                                    {
                                        db.CrmQuotationTaxes.RemoveRange(taxesdel);
                                    }
                                    db.CrmQuotationUni.Remove(headerdel);
                                    db.SaveChanges();

                                    TransactionsAudit audit = new TransactionsAudit();
                                    audit.TraId = (int)tot.header.RecordId;
                                    audit.Descr = "A new Quotation deleted with Id of " + tot.header.Seq + " from the customer " + tot.header.PartyName;
                                    audit.Usr = tot.usr.uCode;
                                    audit.Tratype = 3;
                                    audit.Transact = "CRM_QUO";
                                    audit.TraModule = "CRM";
                                    audit.Syscode = "Sale Quotation";
                                    audit.BranchId = tot.usr.bCode;
                                    audit.CustomerCode = tot.usr.cCode;
                                    audit.Dat = ac.getPresentDateTime();
                                    db.TransactionsAudit.Add(audit);
                                    db.SaveChanges();
                                    msg = "OK";
                                }
                                else
                                {
                                    msg = "No record found";
                                }
                                break;
                        }
                    }
                    else
                    {
                        msg = tramsg;
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

            }
            else
            {
                msg = "You are not authorised for this transaction";
            }

            TransactionResult result = new TransactionResult();
            result.result = msg;
            return result;



        }


        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/SetCRMQuotationForApproval")]
        public TransactionResult SetCRMQuotationForApproval([FromBody] GeneralInformation inf)
        {
            TransactionResult result = new TransactionResult();
            string msg = "";
            try
            {
                if (ac.screenCheck(inf.usr, 7, 2, 3, 98))
                {
                    var header = db.CrmQuotationUni.Where(a => a.RecordId == inf.recordId && a.Pos == 1 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).FirstOrDefault();
                    if (header != null)
                    {
                        header.Pos = 3;
                        header.ApprovedUsr = inf.usr.uCode;
                        header.ApprovedDat = ac.getPresentDateTime();
                        db.SaveChanges();
                        msg = "OK";
                    }
                    else
                    {
                        msg = "No record found";
                    }
                }
                else
                {
                    msg = "You are not authorised for this transaction";
                }
            }
            catch (Exception ee)
            {
                msg = ee.Message;
            }
            result.result = msg;
            return result;
        }







        //        [HttpPost]
        //        [Authorize]
        //        [Route("api/CRMQuotations/PrintSaleQuotation")]
        //        public VoucherResult PrintPurchaseEnquiry([FromBody] GeneralInformation inf)
        //        {
        //            VoucherResult result = new VoucherResult();
        //            string msg = "";
        //            string filename = "";
        //            String str = ho.WebRootPath + "     " + ho.ContentRootPath;
        //            DateTime dat = DateTime.Now;
        //            filename = inf.usr.uCode + "CRMQUOTATIOM" + dat.Second.ToString() + dat.Minute.ToString() + dat.Hour.ToString() + inf.usr.cCode + inf.usr.bCode + ".pdf";
        //            LoginControlController ll = new LoginControlController();
        //            UserAddress addr = ll.makeBranchAddress(inf.usr.bCode, inf.usr.cCode);
        //            int pagesize = 15;

        //            CrmQuotationUni header = db.CrmQuotationUni.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).FirstOrDefault();
        //            var lines = (from p in (from a in db.CrmQuotationDet.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
        //                                    join b in db.InvMaterialCompleteDetailsView.Where(a => a.Customercode == inf.usr.cCode) on a.ItemId equals b.RecordId
        //                                    select new
        //                                    {
        //                                        a.Sno,
        //                                        a.ItemId,
        //                                        b.Itemname,
        //                                        a.ItemDescription,
        //                                        a.Qty,
        //                                        a.Um,
        //                                        header.TotalAmt
        //                                    })
        //                         join
        //q in db.InvUm.Where(a => a.CustomerCode == inf.usr.cCode) on p.Um equals q.RecordId
        //                         select new
        //                         {
        //                             p.Sno,
        //                             p.ItemId,
        //                             p.Itemname,
        //                             p.ItemDescription,
        //                             p.Qty,
        //                             q.Um,
        //                             p.TotalAmt

        //                         }).OrderBy(b => b.Sno).ToList();


        //            using (FileStream ms = new FileStream(ho.WebRootPath + "\\Reps\\" + filename, FileMode.Append, FileAccess.Write))
        //            {
        //                Document document = new Document(PageSize.A4, 75f, 40f, 20f, 0f);
        //                PdfWriter writer = PdfWriter.GetInstance(document, ms);

        //                document.Open();
        //                PdfPTable ptot = new PdfPTable(1);
        //                float[] widths = new float[] { 550f };
        //                ptot.SetWidths(widths);
        //                ptot.TotalWidth = 550f;
        //                ptot.LockedWidth = true;
        //                iTextSharp.text.Font fn;
        //                fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 14, Font.BOLD));


        //                PdfPCell plC = new PdfPCell(makeHeader(addr, header, 1, 1));
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                ptot.AddCell(plC);

        //                PdfPTable pbody = new PdfPTable(5);
        //                pbody.SetWidths(new float[] { 50f, 290f, 90f, 60f,60f });
        //                pbody.TotalWidth = 550f;
        //                pbody.LockedWidth = true;

        //                fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.BOLD));


        //                plC = new PdfPCell(new Phrase("#", fn));
        //                fn.SetColor(255, 255, 255);
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9,122,168);
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //                pbody.AddCell(plC);
        //                plC = new PdfPCell(new Phrase("Item Name", fn));
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168) ;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168) ;
        //                pbody.AddCell(plC);
        //                plC = new PdfPCell(new Phrase("Quantity", fn));
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168) ;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168) ;
        //                pbody.AddCell(plC);
        //                plC = new PdfPCell(new Phrase("Unit", fn));
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168) ;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //                pbody.AddCell(plC);
        //                plC = new PdfPCell(new Phrase("Amount", fn));
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //                plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //                pbody.AddCell(plC);


        //                int sno = 1;

        //                foreach (var line in lines)
        //                {
        //                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
        //                    plC = new PdfPCell(new Phrase(sno.ToString(), fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(line.Itemname, fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(line.Qty.ToString(), fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(line.Um, fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(line.TotalAmt.ToString(), fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);

        //                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.ITALIC));
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(line.ItemDescription, fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase("", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase("", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);

        //                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.ITALIC));
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);

        //                    sno++;
        //                }

        //                while (sno <= 25)
        //                {
        //                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.ITALIC));
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);
        //                    plC = new PdfPCell(new Phrase(" ", fn));
        //                    plC.BorderWidth = 0;
        //                    plC.BorderWidthRight = 1f;
        //                    plC.BorderWidthLeft = 1f;
        //                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                    pbody.AddCell(plC);

        //                    sno++;
        //                }

        //                plC = new PdfPCell(pbody);

        //                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                ptot.AddCell(plC);

        //                plC = new PdfPCell(makeFooter(lines.Sum(a=>a.TotalAmt)));

        //                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                ptot.AddCell(plC);

        //                document.Add(ptot);
        //                document.Close();
        //            }



        //            result.result = msg;
        //            result.filename = filename;
        //            return result;
        //        }
        //        private PdfPTable makeFooter(double? amt)
        //        {
        //            PdfPTable footer = new PdfPTable(1);
        //            float[] widths = new float[] { 550f };
        //            footer.SetWidths(widths);
        //            footer.TotalWidth = 550f;
        //            footer.LockedWidth = true;
        //            iTextSharp.text.Font fn;

        //            PdfPCell plC;
        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 13, Font.BOLD));
        //            plC = new PdfPCell(new Phrase("Amount in Words:", fn));
        //            plC.BorderWidth = 0f;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //            fn.SetColor(255, 255, 255);
        //            plC.FixedHeight = 20;
        //            footer.AddCell(plC);
        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase(NumberToWords(Convert.ToInt32(amt)).ToUpper(), fn));
        //            plC.BorderWidth = 0f;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            plC.BorderWidthBottom = 1f;
        //            footer.AddCell(plC);
        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.BOLDITALIC));
        //            for (int i = 1; i <= 4; i++)
        //            {
        //                plC = new PdfPCell(new Phrase(" ", fn));
        //                plC.BorderWidth = 0f;
        //                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //                footer.AddCell(plC);
        //            }
        //            plC = new PdfPCell(new Phrase("Sales Manager", fn));
        //            plC.BorderWidth = 0f;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            footer.AddCell(plC);

        //            return footer;
        //        }


        //        private PdfPTable makeHeader(UserAddress addr, CrmQuotationUni header, int pagesize, int pages)
        //        {
        //            General g = new General();
        //            PdfPTable pheader = new PdfPTable(1);
        //            float[] widths = new float[] { 550f };
        //            pheader.SetWidths(widths);
        //            pheader.TotalWidth = 550f;
        //            pheader.LockedWidth = true;
        //            iTextSharp.text.Font fn;


        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 13, Font.BOLD));
        //            fn.SetColor(255, 255, 255);
        //            PdfPCell plC = new PdfPCell(new Phrase("SALE QUOTATION", fn));
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;           
        //            plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168) ;
        //            plC.FixedHeight = 20;
        //            pheader.AddCell(plC);

        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 14, Font.BOLD));
        //            plC = new PdfPCell(new Phrase(addr.branchName, fn));
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            plC.Border = 0;
        //            plC.FixedHeight = 50;
        //            pheader.AddCell(plC);

        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 9, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase("Modern Profound Tech, Suite: 303 & 304, Third Floor, Kondapur, Hyderabad, Telangana\r\n500032\r\nPhone no.: 8297658297 Email: accounting@loggindigital.com\r\nGSTIN: 36AAECL8196P1ZK, State: 36-Telangana", fn));           
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            plC.Border = 0;
        //            pheader.AddCell(plC);



        //            //PdfPTable psub = new PdfPTable(2);
        //            //psub.SetWidths(new float[] { 275f, 275f });
        //            //psub.TotalWidth = 550f;
        //            //psub.LockedWidth = true;

        //            PdfPTable pright = new PdfPTable(2);
        //            pright.SetWidths(new float[] { 100f, 175f });
        //            pright.TotalWidth = 550f;
        //            pright.LockedWidth = true;



        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 13, Font.BOLD));
        //            fn.SetColor(255, 255, 255);
        //            plC = new PdfPCell(new Phrase("Estimate Details", fn));
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_TOP;
        //            plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //            plC.FixedHeight = 20;
        //            plC.BorderWidth = 0;
        //            pheader.AddCell(plC);



        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));

        //            plC = new PdfPCell(new Phrase("SQ #:"+" "+ header.Seq, fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);



        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase("Date:" + " " + g.strDate(header.Dat.Value), fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);




        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase("GST:"+" "+ addr.fax, fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);



        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase("Validity:"+" "+ g.strDate(header.Validity.Value), fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);




        //            //fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
        //            //fn.SetColor(255, 255, 255);
        //            //plC = new PdfPCell(new Phrase("Estimate For", fn));
        //            //plC.BorderWidth = 0;
        //            //plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            //plC.VerticalAlignment = PdfPCell.ALIGN_TOP;
        //            //plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //            //plC.FixedHeight = 20;
        //            //psub.AddCell(plC);

        //            //fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));
        //            //plC = new PdfPCell(new Phrase(addr.address + "\n" + addr.city + "\n" + addr.mobile, fn));
        //            //plC.BorderWidth = 0;
        //            //plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            //plC.VerticalAlignment = PdfPCell.ALIGN_TOP;
        //            //psub.AddCell(plC);

        //            //plC = new PdfPCell(pright);


        //            //psub.AddCell(plC);

        //            //plC = new PdfPCell(psub);
        //            //pheader.AddCell(plC);

        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 13, Font.BOLD));
        //            plC = new PdfPCell(new Phrase("Estimate For", fn));
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
        //            fn.SetColor(255, 255, 255);
        //            plC.FixedHeight = 20;
        //            pheader.AddCell(plC);


        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 12, Font.BOLD));
        //            plC = new PdfPCell(new Phrase(header.PartyName, fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);

        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase(header.Addr, fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);
        //            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 11, Font.NORMAL));
        //            plC = new PdfPCell(new Phrase(header.City + ", " + header.Stat, fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);

        //            plC = new PdfPCell(new Phrase(" ", fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);
        //            plC = new PdfPCell(new Phrase(" ", fn));
        //            plC.BorderWidth = 0;
        //            plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            pheader.AddCell(plC);
        //            return pheader;
        //        }
        //        public static string NumberToWords(int number)
        //        {
        //            if (number == 0)
        //                return "zero";

        //            if (number < 0)
        //                return "minus " + NumberToWords(Math.Abs(number));

        //            string words = "";

        //            if ((number / 1000000) > 0)
        //            {
        //                words += NumberToWords(number / 1000000) + " million ";
        //                number %= 1000000;
        //            }

        //            if ((number / 1000) > 0)
        //            {
        //                words += NumberToWords(number / 1000) + " thousand ";
        //                number %= 1000;
        //            }

        //            if ((number / 100) > 0)
        //            {
        //                words += NumberToWords(number / 100) + " hundred ";
        //                number %= 100;
        //            }

        //            if (number > 0)
        //            {
        //                if (words != "")
        //                    words += "and ";

        //                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        //                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        //                if (number < 20)
        //                    words += unitsMap[number];
        //                else
        //                {
        //                    words += tensMap[number / 10];
        //                    if ((number % 10) > 0)
        //                        words += "-" + unitsMap[number % 10];
        //                }
        //            }

        //            return words;
        //        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMQuotations/PrintSaleQuotation")]
        public VoucherResult PrintPurchaseEnquiry([FromBody] GeneralInformation inf)
        {
            VoucherResult result = new VoucherResult();
            string msg = "";
            string filename = "";
            String str = ho.WebRootPath + "     " + ho.ContentRootPath;
            DateTime dat = DateTime.Now;
            filename = inf.usr.uCode + "CRMQUOTATIOM" + dat.Second.ToString() + dat.Minute.ToString() + dat.Hour.ToString() + inf.usr.cCode + inf.usr.bCode + ".pdf";
            LoginControlController ll = new LoginControlController();
            UserAddress addr = ll.makeBranchAddress(inf.usr.bCode, inf.usr.cCode);
            int pagesize = 15;

            CrmQuotationUni header = db.CrmQuotationUni.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).FirstOrDefault();
            var lines = (from p in (from a in db.CrmQuotationDet.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                                    join b in db.InvMaterialCompleteDetailsView.Where(a => a.Customercode == inf.usr.cCode) on a.ItemId equals b.RecordId
                                    select new
                                    {
                                        a.Sno,
                                        a.ItemId,
                                        b.Itemname,
                                        a.ItemDescription,
                                        a.Qty,
                                        a.Um,
                                        a.Rat,
                                        a.DiscPer,
                                        header.TotalAmt


                                    })
                         join
q in db.InvUm.Where(a => a.CustomerCode == inf.usr.cCode) on p.Um equals q.RecordId
                         select new
                         {
                             p.Sno,
                             p.ItemId,
                             p.Itemname,
                             p.ItemDescription,
                             p.Qty,
                             q.Um,
                             p.Rat,
                             p.DiscPer,
                             // Total = p.Qty * p.Rat
                             Total = (p.Qty * p.Rat) * (1 - (p.DiscPer / 100))

                         }).OrderBy(b => b.Sno).ToList();


            using (FileStream ms = new FileStream(ho.WebRootPath + "\\Reps\\" + filename, FileMode.Append, FileAccess.Write))
            {
                Document document = new Document(PageSize.A4, 75f, 40f, 20f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();
                PdfPTable ptot = new PdfPTable(1);
                float[] widths = new float[] { 550f };
                ptot.SetWidths(widths);
                ptot.TotalWidth = 550f;
                ptot.LockedWidth = true;
                iTextSharp.text.Font fn;
                fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 14, Font.BOLD));


                PdfPCell plC = new PdfPCell(makeHeader(addr, header, 1, 1));
                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                ptot.AddCell(plC);


                PdfPTable pbody = new PdfPTable(7);  // Updated to  columns
                pbody.SetWidths(new float[] { 50f, 150f, 70f, 70f, 70f, 70f, 70f }); // Adjusted column widths
                pbody.TotalWidth = 550f;
                pbody.LockedWidth = true;

                fn = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.White); ;

                // Column headers
                plC = new PdfPCell(new Phrase("S.No", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);

                plC = new PdfPCell(new Phrase("Item Name", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);

                plC = new PdfPCell(new Phrase("Quantity", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);

                plC = new PdfPCell(new Phrase("Unit", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);

                plC = new PdfPCell(new Phrase("Price/Unit", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);

                plC = new PdfPCell(new Phrase("Discount", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);

                plC = new PdfPCell(new Phrase("Total", fn));
                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                plC.BackgroundColor = new BaseColor(9, 122, 168);
                pbody.AddCell(plC);


                int sno = 1;
                foreach (var line in lines)
                {
                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
                    AddBodyCell(pbody, sno.ToString(), fn, PdfPCell.ALIGN_CENTER);
                    AddBodyCell(pbody, line.Itemname, fn, PdfPCell.ALIGN_LEFT);
                    AddBodyCell(pbody, line.Qty.ToString(), fn, PdfPCell.ALIGN_RIGHT);
                    AddBodyCell(pbody, line.Um.ToString(), fn, PdfPCell.ALIGN_CENTER);
                    AddBodyCell(pbody, line.Rat.ToString(), fn, PdfPCell.ALIGN_RIGHT);
                    AddBodyCell(pbody, line.DiscPer.ToString(), fn, PdfPCell.ALIGN_RIGHT);
                    AddBodyCell(pbody, line.Total.ToString(), fn, PdfPCell.ALIGN_RIGHT);

                    sno++;
                }

                while (sno <= 25)
                {
                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.ITALIC));
                    for (int i = 0; i < 4; i++)
                    {
                        AddBodyCell(pbody, " ", fn, PdfPCell.ALIGN_CENTER);
                    }

                    sno++;
                }


                while (sno <= 25)
                {
                    fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.ITALIC));
                    plC = new PdfPCell(new Phrase(" ", fn));
                    plC.BorderWidth = 0;
                    plC.BorderWidthRight = 1f;
                    plC.BorderWidthLeft = 1f;
                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    pbody.AddCell(plC);
                    plC = new PdfPCell(new Phrase(" ", fn));
                    plC.BorderWidth = 0;
                    plC.BorderWidthRight = 1f;
                    plC.BorderWidthLeft = 1f;
                    plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    pbody.AddCell(plC);
                    plC = new PdfPCell(new Phrase(" ", fn));
                    plC.BorderWidth = 0;
                    plC.BorderWidthRight = 1f;
                    plC.BorderWidthLeft = 1f;
                    plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    pbody.AddCell(plC);
                    plC = new PdfPCell(new Phrase(" ", fn));
                    plC.BorderWidth = 0;
                    plC.BorderWidthRight = 1f;
                    plC.BorderWidthLeft = 1f;
                    plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    pbody.AddCell(plC);

                    sno++;
                }

                plC = new PdfPCell(pbody);

                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                ptot.AddCell(plC);

                plC = new PdfPCell(makeFooter(
                       lines.Sum(a => a.Total) ?? 0.0, // Default to 0.0 if null
                    lines.Sum(a => a.Qty) ?? 0.0  // Default to 0.0 if null
                    ));

                plC.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                ptot.AddCell(plC);

                document.Add(ptot);
                document.Close();
            }



            result.result = msg;
            result.filename = filename;
            return result;

            // Helper method to add cells to the body table
            void AddBodyCell(PdfPTable table, string text, iTextSharp.text.Font font, int alignment)
            {
                PdfPCell cell = new PdfPCell(new Phrase(text, font));
                cell.BorderWidth = 0;
                cell.BorderWidthRight = 1f;
                cell.BorderWidthLeft = 1f;
                cell.HorizontalAlignment = alignment;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                table.AddCell(cell);
            }
        }


        private PdfPTable makeFooter(double totalAmount, double totalQuantity)
        {

            PdfPTable footer = new PdfPTable(1);
            float[] widths = new float[] { 550f };
            footer.SetWidths(widths);
            footer.TotalWidth = 550f;
            footer.LockedWidth = true;
            iTextSharp.text.Font fn;

            PdfPCell plC;
            // Adding Total Quantity and Amount cells
            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 12, Font.BOLD));

            PdfPTable totalsTable = new PdfPTable(2);
            totalsTable.SetWidths(new float[] { 250f, 250f });
            totalsTable.TotalWidth = 550f;
            totalsTable.LockedWidth = true;

            PdfPCell qtyCell = new PdfPCell(new Phrase($"Total Quantity: {totalQuantity}", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            totalsTable.AddCell(qtyCell);

            PdfPCell amountCell = new PdfPCell(new Phrase($"Total Amount: {totalAmount}", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            totalsTable.AddCell(amountCell);

            // Add the totals table to the footer
            plC = new PdfPCell(totalsTable)
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            footer.AddCell(plC);
            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 13, Font.BOLD));
            plC = new PdfPCell(new Phrase("Amount in Words:", fn));
            plC.BorderWidth = 0f;
            plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            plC.BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168);
            fn.SetColor(255, 255, 255);
            plC.FixedHeight = 20;
            footer.AddCell(plC);
            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));
            plC = new PdfPCell(new Phrase(NumberToWords(Convert.ToInt32(totalAmount)).ToUpper(), fn));
            plC.BorderWidth = 0f;
            plC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            plC.BorderWidthBottom = 1f;
            footer.AddCell(plC);


            fn = new iTextSharp.text.Font(FontFactory.GetFont("Arial", 10, Font.BOLDITALIC));
            for (int i = 1; i <= 4; i++)
            {
                plC = new PdfPCell(new Phrase(" ", fn));
                plC.BorderWidth = 0f;
                plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                footer.AddCell(plC);
            }
            plC = new PdfPCell(new Phrase("Sales Manager", fn));
            plC.BorderWidth = 0f;
            plC.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            plC.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            footer.AddCell(plC);

            return footer;
        }


        private PdfPTable makeHeader(UserAddress addr, CrmQuotationUni header, int pagesize, int pages)
        {
            Font fn = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.White);
            PdfPCell plC = new PdfPCell(new Phrase("ESTIMATE", fn))
            {
                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE,
                BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168),
                Colspan = 2,
                PaddingBottom = 10f
            };

            // Create the image object
            string imagePath = ho.WebRootPath + "\\images\\" +"logo.png";
            iTextSharp.text.Image img;
            try
            {
                img = iTextSharp.text.Image.GetInstance(imagePath);
                img.ScaleToFit(100f, 100f);
                img.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            }
            catch (Exception ex)
            {
                throw new Exception("Image loading failed: " + ex.Message);
            }

            // Create a PdfPCell for the image
            PdfPCell imageCell = new PdfPCell(img)
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE,
                PaddingLeft = 10f,
                PaddingTop = 10f,
            };

            // Create the PdfPCell for the company information
            fn = FontFactory.GetFont("Arial", 14, Font.BOLD);
            PdfPCell companyNameCell = new PdfPCell(new Phrase("LOGGIN DIGITAL PRIVATE LIMITED", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };

            fn = FontFactory.GetFont("Arial", 11, Font.NORMAL);
            PdfPCell companyInfoCell = new PdfPCell(new Phrase(
                "Modern Profound Tech, Suite: 303 & 304, Third Floor,\nKondapur, Hyderabad, Telangana, 500032.\n" +
                "Phone no.: 8297658297\nEmail: accounting@loggindigital.com\n", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };

            fn = FontFactory.GetFont("Arial", 11, Font.BOLD);
            PdfPCell gstCell = new PdfPCell(new Phrase("GSTIN: 36AAECL8196P1ZK, State: 36 - Telangana", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };

            // Combine the company information cells into a single cell using a nested table
            PdfPTable nestedTable = new PdfPTable(1);
            nestedTable.AddCell(companyNameCell);
            nestedTable.AddCell(companyInfoCell);
            nestedTable.AddCell(gstCell);

            PdfPCell nestedCell = new PdfPCell(nestedTable)
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };

            // Create a table to hold the logo and company info in one row
            PdfPTable headerContentTable = new PdfPTable(2);
            float[] headerContentWidths = new float[] { 1f, 2f }; // Adjust width ratio if needed
            headerContentTable.SetWidths(headerContentWidths);

            headerContentTable.AddCell(imageCell);
            headerContentTable.AddCell(nestedCell);

            // Create a PdfPCell for the combined header content
            PdfPCell combinedHeaderCell = new PdfPCell(headerContentTable)
            {
                BorderWidth = 0,
                Colspan = 2,
                PaddingBottom = 1f
            };

            // Create the main header table with 2 columns
            PdfPTable pheader = new PdfPTable(2);
            float[] widths = new float[] { 250f, 300f };
            pheader.SetWidths(widths);
            pheader.TotalWidth = 550f;
            pheader.LockedWidth = true;

            // Add the "ESTIMATE" cell spanning both columns
            pheader.AddCell(plC);

            // Add the combined header content cell
            pheader.AddCell(combinedHeaderCell);

            // Create a new table for "Estimate For" and "Estimate Details"
            PdfPTable estimateHeaderTable = new PdfPTable(2);
            estimateHeaderTable.SetWidths(new float[] { 1f, 1f }); // Equal widths for both columns

            // Add "Estimate For" cell (left-aligned)
            fn = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.White);
            PdfPCell estimateForCell = new PdfPCell(new Phrase("Estimate For", fn))
            {
                BorderWidth = 1,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE,
                PaddingBottom = 2f,
                BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168),
            };
            estimateHeaderTable.AddCell(estimateForCell);

            // Add "Estimate Details" cell (right-aligned)
            PdfPCell estimateDetailsCell = new PdfPCell(new Phrase("Estimate Details", fn))
            {
                BorderWidth = 1,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE,
                PaddingBottom = 2f,
                BackgroundColor = new iTextSharp.text.BaseColor(9, 122, 168),

            };
            estimateHeaderTable.AddCell(estimateDetailsCell);

            // Add the new estimate header row table
            pheader.AddCell(new PdfPCell(estimateHeaderTable)
            {
                Colspan = 2,
                BorderWidth = 0,
                PaddingBottom = 1f
            });

            // Create the left and right tables for estimate details
            PdfPTable pleft = new PdfPTable(2);
            pleft.SetWidths(new float[] { 225f, 225f });
            pleft.TotalWidth = 550f;
            pleft.LockedWidth = false;

            PdfPTable pright = new PdfPTable(2);
            pright.SetWidths(new float[] { 186f, 89f });
            pright.TotalWidth = 275f;
            pright.LockedWidth = true;

            // Estimate details
            fn = FontFactory.GetFont("Arial", 11, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase("Estimate No.:", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            pright.AddCell(cell);

            fn = FontFactory.GetFont("Arial", 12, Font.BOLD);
            cell = new PdfPCell(new Phrase(header.Seq.ToString(), fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            pright.AddCell(cell);

            fn = FontFactory.GetFont("Arial", 11, Font.NORMAL);
            cell = new PdfPCell(new Phrase("Date:", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            pright.AddCell(cell);

            fn = FontFactory.GetFont("Arial", 12, Font.BOLD);
            string formattedDate = header.Dat.HasValue ? header.Dat.Value.ToString("dd/MM/yyyy") : "";
            cell = new PdfPCell(new Phrase(formattedDate, fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            pright.AddCell(cell);

            fn = FontFactory.GetFont("Arial", 11, Font.NORMAL);
            cell = new PdfPCell(new Phrase("Place of supply:", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            pright.AddCell(cell);

            fn = FontFactory.GetFont("Arial", 11, Font.BOLD);
            cell = new PdfPCell(new Phrase("36 - Telangana", fn))
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            pright.AddCell(cell);

            Font boldFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font normalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            // Create a Phrase to combine the Vendorname in bold and the rest in normal font
            Phrase vendorDetailsPhrase = new Phrase();
            vendorDetailsPhrase.Add(new Chunk(header.PartyName + "\n\n", boldFont));
            vendorDetailsPhrase.Add(new Chunk(header.Addr + ", " + header.City + ", " + header.Country + ", " + header.Zip + "\n\n", normalFont));

            // Add "Contact No" and "State" details
            vendorDetailsPhrase.Add(new Chunk("Contact No: " + header.Mobile + "\n", normalFont));
            vendorDetailsPhrase.Add(new Chunk("State: " + "36 - Telangana", normalFont));

            // Reuse the 'cell' variable for the vendor details
            cell = new PdfPCell(vendorDetailsPhrase)
            {
                BorderWidth = 0,
                HorizontalAlignment = PdfPCell.ALIGN_LEFT,
                VerticalAlignment = PdfPCell.ALIGN_TOP
            };
            pleft.AddCell(cell);

            // Reuse the 'cell' variable for the pright table
            cell = new PdfPCell(pright)
            {
                Colspan = 2,
                BorderWidth = 1,
                PaddingBottom = 0
            };
            pleft.AddCell(cell);

            // Reuse the 'cell' variable for the estimate details
            cell = new PdfPCell(pleft)
            {
                Colspan = 2,
                BorderWidth = 1,
                PaddingBottom = 0
            };
            pheader.AddCell(cell);



            return pheader;


        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }


    }



}
