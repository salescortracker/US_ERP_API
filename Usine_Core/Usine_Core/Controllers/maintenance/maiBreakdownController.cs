using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Usine_Core.Controllers.Admin;
using Usine_Core.Controllers.CRM;
using Usine_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Usine_Core.Controllers.maintenance
{
   public class MaiBreakdownTotal
    {
        public MaiBreakdownDetails detail { get; set; }
        public UserInfo usr { get; set; }
        public int? traCheck { get; set; }
        public List<MaiBreakdownServicesTaxes> taxes { get; set; }
    }
    public class maiBreakdownController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();

        [HttpPost]
        [Authorize]
        [Route("api/maiBreakdown/GetBreakdownList")]
        public dynamic GetBreakdownList([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = dat1.AddDays(10);
            return (from a in db.MaiBreakdownDetails.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                       join b in db.MaiEquipment.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.EquipId equals b.RecordId
                       select new
                       {
                           recordid = a.RecordId,
                           equipid = a.EquipId,
                           equipment = b.EquipmentName,
                           breakdowndate = a.BreakDownDate,
                           reporteduser = a.ReportedUser,
                           description = a.BreakDownDescription,
                           serviceassign = a.ServiceAssignCheck == 1 ? "Cleared" : "Pending",
                           serviceclear = a.ServiceClearanceCheck == 1 ? "Cleared" : "Pending",
                           pos = a.ServiceAssignCheck == 1 ? 0 : 1
                       }).OrderBy(b => b.breakdowndate).ToList();
        }


        [HttpPost]
        [Authorize]
        [Route("api/maiBreakdown/SetMaiBreakdown")]
        public TransactionResult SetMaiBreakdown([FromBody] MaiBreakdownTotal tot)
        {
            string msg = "";
            if(ac.screenCheck(tot.usr,9,2,2,(int)tot.traCheck))
            {
                try
                {
                    switch(tot.traCheck)
                    {
                        case 1:
                            tot.detail.Dat = ac.getPresentDateTime();
                            tot.detail.ServiceAssignCheck = 0;
                            tot.detail.BranchId = tot.usr.bCode;
                            tot.detail.CustomerCode = tot.usr.cCode;
                            tot.detail.ReportedUser = tot.usr.uCode;
                            db.MaiBreakdownDetails.Add(tot.detail);
                            db.SaveChanges();
                            break;
                        case 2:
                            var upd = db.MaiBreakdownDetails.Where(a => a.RecordId == tot.detail.RecordId && a.ServiceAssignCheck==0 && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                            if(upd != null)
                            {
                                upd.EquipId = tot.detail.EquipId;
                                upd.BreakDownDate = tot.detail.BreakDownDate;
                                upd.ReportedUser = tot.usr.bCode;
                                upd.BreakDownDescription = tot.detail.BreakDownDescription;
                                db.SaveChanges();
                            }

                            break;
                        case 3:
                            var del = db.MaiBreakdownDetails.Where(a => a.RecordId == tot.detail.RecordId && a.ServiceAssignCheck == 0 && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                            if(del != null)
                            {
                                db.MaiBreakdownDetails.Remove(del);
                                db.SaveChanges();
                            }
                            break;
                    }
                    msg = "OK";
                }
                catch(Exception ee)
                {
                    msg = ee.Message;
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
        [Route("api/maiBreakdown/GetServiceAssignsList")]
        public dynamic GetServiceAssignsList([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = dat1.AddDays(1);
            return (from a in db.MaiBreakdownDetails.Where(a => a.ServiceAssignDate >= dat1 && a.ServiceAssignDate < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                    join b in db.MaiEquipment.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.EquipId equals b.RecordId
                    join c in db.PartyDetails.Where(a => a.CustomerCode == inf.usr.cCode) on a.ServiceProvider equals c.RecordId
                    select new
                    {
                        recordid = a.RecordId,
                        equipid = a.EquipId,
                        equipment = b.EquipmentName,
                        assigndate = a.ServiceAssignDate,
                        reporteduser = a.ServiceAssignUser,
                        serviceprovider=c.PartyName,
                          serviceclear = a.ServiceClearanceCheck == 1 ? "Cleared" : "Pending",
                        pos = a.ServiceAssignCheck == 1 ? 0 : 1
                    }).OrderBy(b => b.assigndate).ToList();
        }

        [HttpPost]
        [Authorize]
        [Route("api/maiBreakdown/SetServiceAssign")]
        public TransactionResult SetServiceAssign([FromBody] MaiBreakdownTotal tot)
        {
            string msg="";
             if(ac.screenCheck(tot.usr,9,2,3,(int)tot.traCheck))
            {
                switch(tot.traCheck)
                {
                    case 1:
                        var cre = db.MaiBreakdownDetails.Where(a => a.RecordId == tot.detail.RecordId &&  a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                        cre.ServiceAssignCheck = 1;
                        cre.ServiceAssignDate = ac.getPresentDateTime();
                        cre.ServiceAssignUser = tot.usr.uCode;
                        cre.ServiceProvider = tot.detail.ServiceProvider;
                        cre.ServiceBaseAmount = tot.detail.ServiceBaseAmount;
                        cre.ServiceTaxes = tot.detail.ServiceTaxes;
                        cre.ServiceOtherAmt = tot.detail.ServiceOtherAmt;
                        cre.ServiceTotalAmount = tot.detail.ServiceTotalAmount;

                        int? sno1 = 0;
                        
                        sno1++;
                        var details1 = db.MaiBreakdownServicesTaxes.Where(a => a.RecordId == tot.detail.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                        if (details1.Count() > 0)
                        {
                            db.MaiBreakdownServicesTaxes.RemoveRange(details1);
                        }
                        foreach (var tax in tot.taxes)
                        {
                            tax.RecordId = tot.detail.RecordId;
                            tax.Sno = sno1;
                            tax.BranchId = tot.usr.bCode;
                            tax.CustomerCode = tot.usr.cCode;
                            sno1++;
                        }
                        if(tot.taxes.Count() > 0)
                        db.MaiBreakdownServicesTaxes.AddRange(tot.taxes);

                        db.SaveChanges();
                        msg = "OK";
                        break;
                    case 2:
                        var upd = db.MaiBreakdownDetails.Where(a => a.RecordId == tot.detail.RecordId && a.ServiceClearanceCheck == 0 && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                        upd.ServiceAssignCheck = 1;
                        upd.ServiceAssignDate = ac.getPresentDateTime();
                        upd.ServiceAssignUser = tot.usr.uCode;
                        upd.ServiceProvider = tot.detail.ServiceProvider;
                        upd.ServiceBaseAmount = tot.detail.ServiceBaseAmount;
                        upd.ServiceTaxes = tot.detail.ServiceTaxes;
                        upd.ServiceOtherAmt = tot.detail.ServiceOtherAmt;
                        upd.ServiceTotalAmount = tot.detail.ServiceTotalAmount;

                        int? sno = 0;
                        var det = db.MaiBreakdownServicesTaxes.Where(a => a.RecordId == tot.detail.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).Max(b => b.Sno);
                         if(det != null)
                        {
                            sno = det;
                        }
                        sno++;
                        var details = db.MaiBreakdownServicesTaxes.Where(a => a.RecordId == tot.detail.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                        if (details.Count() > 0)
                        {
                            db.MaiBreakdownServicesTaxes.RemoveRange(details);
                        }
                        foreach(var tax in tot.taxes)
                        {
                            tax.RecordId = tot.detail.RecordId;
                            tax.Sno = sno;
                            tax.BranchId = tot.usr.bCode;
                            tax.CustomerCode = tot.usr.cCode;
                            sno++;
                        }
                        db.MaiBreakdownServicesTaxes.AddRange(tot.taxes);

                        db.SaveChanges();
                         
                        break;
                    case 3:
                        var del = db.MaiBreakdownDetails.Where(a => a.RecordId == tot.detail.RecordId && a.ServiceClearanceCheck == 0 && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                        del.ServiceAssignCheck = 0;
                          
                        var detailsdel = db.MaiBreakdownServicesTaxes.Where(a => a.RecordId == tot.detail.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                        if (detailsdel.Count() > 0)
                        {
                            db.MaiBreakdownServicesTaxes.RemoveRange(detailsdel);
                        }
                       

                        db.SaveChanges();
                        break;
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


    }
}
