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
    public class MaiInsuranceTotal
    {
        public MaiInsurancesUni header { get; set; }
        public dynamic details { get; set; }
        public List<int?> equips { get; set; }
        public int? traCheck { get; set; }
        public UserInfo usr { get; set; }
    }
    public class maiInsurancesController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();

        [HttpPost]
        [Authorize]
        [Route("api/maiInsurances/GetInsurances")]
        public dynamic GetInsurances([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = dat1.AddDays(1);
            return (from a in db.MaiInsurancesUni.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                       join b in db.PartyDetails.Where(a => a.CustomerCode == inf.usr.cCode) on a.InsuranceSupplier equals b.RecordId
                       select new
                       {
                           a.RecordId,
                           b.PartyName,
                           a.InsuredAmount,
                           a.InsuredFrom,
                           a.InsuredTo
                       }).OrderBy(b => b.RecordId).ToList();
        }


        [HttpPost]
        [Authorize]
        [Route("api/maiInsurances/GentInsurance")]
        public MaiInsuranceTotal GentInsurance([FromBody] GeneralInformation inf)
        {
            MaiInsuranceTotal tot = new MaiInsuranceTotal();
            tot.header = db.MaiInsurancesUni.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).FirstOrDefault();
            tot.details = (from a in db.MaiInsurancesDet.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                           join b in db.MaiEquipment.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.EquipId equals b.RecordId
                           join c in db.MaiEquipGroups.Where(a => a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on b.EquipmentGroup equals c.RecordId
                           select new
                           {
                               recordid = a.RecordId,
                               sno = a.Sno,
                               equipid = a.EquipId,
                               equipname = b.EquipmentName,
                               equipgroup = c.SGrp
                           }).OrderBy(b => b.recordid).ThenBy(c => c.sno).ToList();

            return tot;
        }

        [HttpPost]
        [Authorize]
        [Route("api/maiInsurances/setInsurance")]
        public TransactionResult setInsurance([FromBody] MaiInsuranceTotal tot)
        {
            string msg = "";
            try
            {
                tot.header.BranchId = tot.usr.bCode;
                tot.header.CustomerCode = tot.usr.cCode;
                db.MaiInsurancesUni.Add(tot.header);
                db.SaveChanges();
                List<MaiInsurancesDet> lines = new List<MaiInsurancesDet>();
                int? sno = 1;
                foreach (var det in tot.equips)
                {
                    lines.Add(new MaiInsurancesDet
                    {
                        RecordId = tot.header.RecordId,
                        Sno = sno,
                        EquipId = det,
                        BranchId = tot.usr.bCode,
                        CustomerCode = tot.usr.cCode
                    });

                    sno++;
                }
                if (lines.Count() > 0)
                {
                    db.MaiInsurancesDet.AddRange(lines);
                    db.SaveChanges();
                }

                msg = "OK";
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
