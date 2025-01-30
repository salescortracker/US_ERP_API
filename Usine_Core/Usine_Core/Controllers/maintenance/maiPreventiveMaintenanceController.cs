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
    public class MaiPMRequirementsInfo
    {
        public List<MaiEquipment> equipment { get; set; }
        public dynamic details { get; set; }
        public int? traCheck { get; set; }
    }
    public class MaiPMTotal
    {
        public MaiEquipmentPmdetails pm { get; set; }
        public UserInfo usr { get; set; }
    }
   
    public class maiPreventiveMaintenanceController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();

        [HttpPost]
        [Authorize]
        [Route("api/maiPreventiveMaintenance/GetMaiEquipmentPMRequirements")]
        public MaiPMRequirementsInfo GetMaiEquipmentPMRequirements([FromBody] UserInfo usr)
        {
            MaiPMRequirementsInfo tot = new MaiPMRequirementsInfo();
            tot.equipment = db.MaiEquipment.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();
            DateTime dat = ac.getPresentDateTime();

            tot.details = (from a in db.MaiEquipmentPreventiveMaintenances.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode)
                           join b in (from p in db.MaiEquipmentPmdetails.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).GroupBy(x => new { x.Equipid, x.Sno })
                                      select new
                                      {
                                          p.Key.Equipid,
                                          p.Key.Sno,
                                          prevdate = p.Max(b => b.Dat)
                                      }) on new { eid = a.RecordId, sno = a.Sno } equals new { eid = b.Equipid, sno = b.Sno }
                                      into gj
                           from subdet in gj.DefaultIfEmpty()
                           select new
                           {
                               a.RecordId,
                               a.Sno,
                               a.Prevmaintenance,
                               prevdate = subdet == null ? null : subdet.prevdate,
                               nextdate = subdet == null ? dat : (((DateTime)subdet.prevdate).AddDays((int)a.FrequencyInDays))
                           }).OrderBy(b => b.RecordId).ThenBy(c => c.Sno).ToList();

            return tot;
        }
        [HttpPost]
        [Authorize]
        [Route("api/maiPreventiveMaintenance/GetMaiEquipmentPMList")]
        public dynamic GetMaiEquipmentPMList([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = dat1.AddDays(1);
            return (from a in db.MaiEquipmentPmdetails.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                    join b in db.MaiEquipmentPreventiveMaintenances.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on new { equipid = a.Equipid, sno = a.Sno } equals new { equipid = b.RecordId, sno = b.Sno }
                    join c in db.MaiEquipment.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.Equipid equals c.RecordId
                    select new
                    {
                        recordid = a.RecordId,
                        dat = a.Dat,
                        equipid = a.Equipid,
                        sno = a.Sno,
                        equipmentname = c.EquipmentName,
                        pm = b.Prevmaintenance,
                        descr = a.Descriptio
                    }).OrderBy(b => b.dat).ToList();
        }
        [HttpPost]
        [Authorize]
        [Route("api/maiPreventiveMaintenance/SetMaiEquipmentPM")]
        public TransactionResult SetMaiEquipmentPM([FromBody] MaiPMTotal tot)
        {
            string msg = "";
            try
            {
                if(ac.screenCheck(tot.usr,8,2,1,1))
                {
                    tot.pm.Dat = ac.getPresentDateTime();
                    tot.pm.BranchId = tot.usr.bCode;
                    tot.pm.CustomerCode = tot.usr.cCode;
                    db.MaiEquipmentPmdetails.Add(tot.pm);
                    db.SaveChanges();
                    msg = "OK";
                }
                else
                {
                    msg = "You are not authorised for this transaction";
                }
               
            }
            catch(Exception ee)
            {
                msg = ee.Message;
            }
            TransactionResult result = new TransactionResult();
            result.result = msg;
            return result;
        }
    }
}
