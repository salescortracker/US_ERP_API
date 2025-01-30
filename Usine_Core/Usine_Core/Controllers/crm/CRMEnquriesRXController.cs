using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usine_Core.Controllers.Admin;
using Usine_Core.Controllers.HRD;
using Usine_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Usine_Core.Controllers.CRM
{
    public class CRMEnquiryRxTotal
    {
        public CrmEnquiriesRx call { get; set; }
        public List<CrmEnquiryLineItem> lines { get; set; }
        public List<CrmEnquiryTax> taxes { get; set; }
        public int traCheck { get; set; }
        public UserInfo usr { get; set; }
        public string result { get; set; }
    }
   
    public class CRMEnquiryRXRequirements
    {
        public String seq { get; set; }
        public List<Employees> employees { get; set; }
        public List<CRMPendingsList> pendings { get; set; }
        public List<MisCountryMaster> countries { get; set; }
        public List<MisStateMaster> states { get; set; }
    }
    public class CRMEnquriesRXController : Controller
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();


        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRxEnquiryRequirements")]
        public CRMEnquiryRXRequirements GetCRMRxEnquiryRequirements([FromBody] UserInfo usr)
        {
            hrdDepartmentsController hr = new hrdDepartmentsController();
            CRMEnquiryRXRequirements tot = new CRMEnquiryRXRequirements();
            tot.seq = findSeq(usr);
            GeneralInformation inf = new GeneralInformation();
            inf.detail = "Sales";
            inf.usr = usr;
            //  tot.employees = hr.getEmployeesByDepartment(inf);
            GeneralInformation inf1 = new GeneralInformation();
            inf.recordId = 2;
            inf.usr = usr;
            tot.pendings = getPendingCallsList(inf);
            tot.countries = db.MisCountryMaster.Where(a => a.CustomerCode == usr.cCode).ToList();
            tot.states = db.MisStateMaster.Select(x => new MisStateMaster
            {
                StateName = x.StateName,
                CustomerCode = x.CustomerCode,
                Cntname = x.Cntname,
                RecordId = x.RecordId

            }).Where(a => a.CustomerCode == usr.cCode).OrderBy(b => b.StateName).ToList();
            //  tot.states = db.MisStateMaster.Where(a => a.CustomerCode == usr.cCode).OrderBy(b => b.StateName).ToList();
            return tot;
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRxEnquiries")]
        public List<CrmEnquiriesRx> GetCRMRxEnquiries([FromBody] GeneralInformation inf)
        {
            //DateTime dat1 = DateTime.Parse(inf.frmDate);
            //DateTime dat2 = DateTime.Parse(inf.toDate).AddDays(1);
            DateTime dat1;
            DateTime dat2;
            if (inf.frmDate != null && inf.toDate != null)
            {
                dat1 = DateTime.Parse(inf.frmDate);
                dat2 = DateTime.Parse(inf.toDate);//.AddDays(1);
            }
            else
            {
                dat1 = DateTime.Now;
                dat2 = DateTime.Now;
            }

            var det = db.CrmEnquiriesRx.Where(a => a.Dat.Value.Date >= dat1.Date && a.Dat.Value.Date <= dat2.Date && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).OrderBy(b => b.RecordId).ToList();
            return det;
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRxOrders")]
        public List<CrmOrdersRxuni> GetCRMRxOrders([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = DateTime.Parse(inf.toDate).AddDays(1);


            return (from a in db.CrmOrdersRxuni.Where(a => a.Dat >= dat1 && a.Dat <= dat2)
                    join b in (from p in db.CrmOrdersRxdet.Where(a => a.Dat >= dat1 && a.Dat <= dat2).GroupBy(b => b.RecordId)
                               select new
                               {
                                   recordId = p.Key,
                                   qty = p.Sum(b => b.Qty)
                               }) on a.RecordId equals b.recordId
                    select new CrmOrdersRxuni
                    {
                        RecordId = a.RecordId,
                        Seq = a.Seq,
                        Dat = a.Dat,
                        Customer = a.Customer,
                        BaseAmt = b.qty
                    }).OrderBy(d => d.RecordId).ToList();




            // return db.CrmEnquiriesRx.Where(a => a.Dat >= inf.frmDate.Value && a.Dat <= inf.toDate.Value && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).OrderBy(b => b.RecordId).ToList();
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/SetCRMRXEnquiry")]
        public CRMEnquiryRxTotal SetCRMRXEnquiry([FromBody] CRMEnquiryRxTotal tot)
        {
            String msg = "";
            String res = ac.transactionCheck("CRM", tot.call.Dat, tot.usr);
            if (res == "OK")
            {
                try
                {
                    switch (tot.traCheck)
                    {
                        case 1:
                            if (ac.screenCheck(tot.usr, 7, 2, 2, 1))
                            {
                                CrmEnquiriesRx call = new CrmEnquiriesRx();
                                call.Seq = findSeq(tot.usr);
                                call.Dat = ac.getPresentDateTime();
                                call.Callerid = tot.call.Callerid;
                                call.Customer = tot.call.Customer;
                                call.Addr = tot.call.Addr;
                                call.Country = tot.call.Country;
                                call.Stat = tot.call.Stat;
                                call.District = tot.call.District;
                                call.City = tot.call.City;
                                call.Zip = tot.call.Zip;

                                call.Mobile = tot.call.Mobile;
                                call.Tel = tot.call.Tel;
                                call.Fax = tot.call.Fax;
                                call.Webid = tot.call.Webid;
                                call.Email = tot.call.Email;
                                call.PrevcallId = tot.call.PrevcallId;
                                call.PrevCallMode = tot.call.PrevCallMode;
                                call.CustomerComments = tot.call.CustomerComments;
                                call.CallerComments = tot.call.CallerComments;
                                call.CallPosition = tot.call.CallPosition;
                                call.NextCallDate = tot.call.NextCallDate;
                                call.NextCallMode = tot.call.NextCallMode;
                                call.BranchId = tot.usr.bCode;
                                call.CustomerCode = tot.usr.cCode;
                                call.username = tot.usr.uCode;
                                call.telecallingno = tot.call.telecallingno;
                                call.Base = tot.call.Base;
                                call.Discount = tot.call.Discount;
                                call.Others = tot.call.Others;
                                call.Total = tot.call.Total;
                                db.CrmEnquiriesRx.Add(call);
                                db.SaveChanges();
                                int sno = 1;
                                int? RecordId = call.RecordId;

                                foreach (var line in tot.lines)
                                {
                                    line.RecordId = RecordId;
                                    line.Sno = sno;
                                    line.BranchId = tot.usr.bCode;
                                    line.CustomerCode = tot.usr.cCode;
                                    sno++;
                                    db.CrmEnquiryLineItems.Add(line);
                                    db.SaveChanges();
                                }
                                //sno = 1;
                                //if (tot.taxes != null)
                                //{
                                //    if (tot.taxes.Count() > 0)
                                //    {
                                //        foreach (var tax in tot.taxes)
                                //        {
                                //            tax.RecordId = __RecordId;
                                //            tax.Sno = sno;
                                //            tax.BranchId = tot.usr.bCode;
                                //            tax.CustomerCode = tot.usr.cCode;
                                //            tax.LineId = 1;
                                //            sno++;
                                //            db.CrmEnquiryTaxes.Add(tax);
                                //            db.SaveChanges();
                                //        }
                                //    }
                                //}



                                msg = "OK";
                            }
                            else
                            {
                                msg = "You are not authorised for Enquiries";
                            }
                            break;
                        case 2:
                            if (ac.screenCheck(tot.usr, 7, 2, 2, 2))
                            {
                                var call = db.CrmEnquiriesRx.Where(a => a.RecordId == tot.call.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                if (call != null)
                                {
                                    call.Callerid = tot.call.Callerid;
                                    call.Customer = tot.call.Customer;
                                    call.Addr = tot.call.Addr;
                                    call.Country = tot.call.Country;
                                    call.Stat = tot.call.Stat;
                                    call.District = tot.call.District;
                                    call.City = tot.call.City;
                                    call.Zip = tot.call.Zip;

                                    call.Mobile = tot.call.Mobile;
                                    call.Tel = tot.call.Tel;
                                    call.Fax = tot.call.Fax;
                                    call.Webid = tot.call.Webid;
                                    call.Email = tot.call.Email;
                                    call.PrevcallId = tot.call.PrevcallId;
                                    call.PrevCallMode = tot.call.PrevCallMode;
                                    call.CustomerComments = tot.call.CustomerComments;
                                    call.CallerComments = tot.call.CallerComments;
                                    call.CallPosition = tot.call.CallPosition;
                                    call.NextCallDate = tot.call.NextCallDate;
                                    call.NextCallMode = tot.call.NextCallMode;
                                    call.telecallingno = tot.call.telecallingno;
                                    call.Base = tot.call.Base;
                                    call.Discount = tot.call.Discount;
                                    call.Others = tot.call.Others;
                                    call.Total = tot.call.Total;

                                    db.SaveChanges();

                                    foreach (var line in tot.lines)
                                    {
                                        var linesupd = db.CrmEnquiryLineItems.Where(a => a.RecordId == line.RecordId && a.Sno == line.Sno).FirstOrDefault();
                                        if (linesupd != null)
                                        {
                                            linesupd.ItemId = line.ItemId;
                                            linesupd.ItemName = line.ItemName;
                                            linesupd.ItemDescription = line.ItemDescription;
                                            linesupd.Qty = line.Qty;
                                            linesupd.Um = line.Um;
                                            linesupd.Value = line.Value;
                                            linesupd.Discount = line.Discount;
                                            linesupd.LeadDays = line.LeadDays;
                                            linesupd.BranchId = line.BranchId;
                                            linesupd.CustomerCode = line.CustomerCode;
                                            linesupd.Rate = line.Rate;
                                            db.SaveChanges();
                                        }
                                    }
                                    msg = "OK";
                                }
                                else
                                {
                                    msg = "No record found";
                                }
                            }
                            else
                            {
                                msg = "You are not authorised for tele calling";
                            }
                            break;
                        case 3:
                            if (ac.screenCheck(tot.usr, 7, 2, 1, 3))
                            {
                                var call = db.CrmEnquiriesRx.Where(a => a.RecordId == tot.call.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                if (call != null)
                                {

                                    db.CrmEnquiriesRx.Remove(call);
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
                                msg = "You are not authorised for enquiry";
                            }
                            break;
                    }

                }
                catch (Exception ee)
                {
                    msg = ee.Message;
                }
            }
            else
            {
                msg = res;
            }

            tot.result = msg;
            return tot;
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRXEnquiryById/{id}")]
        public CRMEnquiryRxTotal GetCRMRXEnquiryById(int id)
        {
            CRMEnquiryRxTotal enquiryDetails = new CRMEnquiryRxTotal();

            try
            {
                // Fetch main enquiry details
                var enquiry = db.CrmEnquiriesRx.FirstOrDefault(e => e.RecordId == id);
                if (enquiry == null)
                {
                    throw new Exception("Enquiry not found.");
                }

                enquiryDetails.call = new CrmEnquiriesRx
                {
                    RecordId = enquiry.RecordId,
                    Seq = enquiry.Seq,
                    Dat = enquiry.Dat,
                    Callerid = enquiry.Callerid,
                    Customer = enquiry.Customer,
                    Addr = enquiry.Addr,
                    Country = enquiry.Country,
                    Stat = enquiry.Stat,
                    District = enquiry.District,
                    City = enquiry.City,
                    Zip = enquiry.Zip,
                    Mobile = enquiry.Mobile,
                    Tel = enquiry.Tel,
                    Fax = enquiry.Fax,
                    Webid = enquiry.Webid,
                    Email = enquiry.Email,
                    PrevcallId = enquiry.PrevcallId,
                    PrevCallMode = enquiry.PrevCallMode,
                    CustomerComments = enquiry.CustomerComments,
                    CallerComments = enquiry.CallerComments,
                    CallPosition = enquiry.CallPosition,
                    NextCallDate = enquiry.NextCallDate,
                    NextCallMode = enquiry.NextCallMode,
                    BranchId = enquiry.BranchId,
                    CustomerCode = enquiry.CustomerCode,
                    username = enquiry.username,
                    telecallingno = enquiry.telecallingno,
                    Base = enquiry.Base,
                    Discount = enquiry.Discount,
                    Others = enquiry.Others,
                    Total = enquiry.Total
                };

                // Fetch line items
                enquiryDetails.lines = db.CrmEnquiryLineItems
                    .Where(line => line.RecordId == id)
                    .ToList();

                // Fetch taxes
                enquiryDetails.taxes = db.CrmEnquiryTaxes
                    .Where(tax => tax.RecordId == id)
                    .ToList();

                enquiryDetails.result = "OK";
            }
            catch (Exception ex)
            {
                enquiryDetails.result = ex.Message;
            }

            return enquiryDetails;
        }



        private String findSeq(UserInfo usr)
        {
            DateTime dat = ac.getPresentDateTime();
            General g = new General();
            String seq = db.CrmEnquiriesRx.Where(a => a.Dat.Value.Year == dat.Year && a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).Max(b => b.Seq);
            int x = 0;
            if (seq != null)
            {
                x = int.Parse(seq.Substring(5, 7));
            }
            x++;
            return "SE" + dat.Year.ToString().Substring(2, 2) + "-" + g.zeroMake(x, 7);
        }

        public List<CRMPendingsList> getPendingCallsList(GeneralInformation inf)
        {
            List<CRMPendingsList> lst = new List<CRMPendingsList>();
            DateTime dat = ac.getPresentDateTime();
            var det1 = db.CrmTeleCallingRx.Where(a => a.NextCallDate <= dat && a.NextCallMode == "2" && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).ToList();
            var det2 = db.CrmEnquiriesRx.Where(a => a.NextCallDate <= dat && a.NextCallMode == 2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).ToList();
            foreach (CrmTeleCallingRx det in det1)
            {
                lst.Add(new CRMPendingsList
                {
                    id = det.RecordId,
                    customer = det.Customer,
                    mobile = det.Mobile,
                    mode = "TeleCall",
                    dat = det.Dat.Value,
                    customercomments = det.CustomerComments
                });
            }
            foreach (CrmEnquiriesRx det in det2)
            {
                lst.Add(new CRMPendingsList
                {
                    id = det.RecordId,
                    customer = det.Customer,
                    mobile = det.Mobile,
                    mode = "Enquiry",
                    dat = det.Dat.Value,
                    customercomments = det.CustomerComments
                });
            }
            return lst;
        }

    }
}
