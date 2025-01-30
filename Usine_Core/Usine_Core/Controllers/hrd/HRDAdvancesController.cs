using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usine_Core.Models;
using Usine_Core.Controllers.Admin;

namespace Usine_Core.Controllers.hrd
{
     public class HRDAdvanceTotal
    {
        public HrdAdvances advance { get; set; }
        public UserInfo usr { get; set; }
    }
    public class HRDAdvancesController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();

        [HttpPost]
        [Route("api/HRDAdvances/GetHRDAdvanceRequirements")]
        [Authorize]
        public dynamic GetHRDAdvanceRequirements([FromBody] UserInfo usr)
        {
            var list1 = (from a in db.HrdEmployees.Where(a => a.Branchid == usr.bCode && a.CustomerCode == usr.cCode)
                         join b in db.HrdDepartments.Where(a => a.Branchid == usr.bCode && a.CustomerCode == usr.cCode) on a.Department equals b.RecordId
                         join c in db.HrdDesignations.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode) on a.Designation equals c.RecordId
                         select new
                         {
                             recordid = a.RecordId,
                             empno = a.Empno,
                             empname = a.Empname,
                             mobile = a.Mobile,
                             department = b.SGrp,
                             designation = c.Designation
                         }).ToList();
            var list2 =  db.HrdAdvances.Where(a => a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();
            if (list2.Count() > 0)
            {
                return (from a in list1
                        join b in (from a in list2.GroupBy(b => b.Empno)
                         select new
                         {
                             recordid = a.Key,
                             prevadvance = a.Sum(b => (b.AdvanceDebit - b.AdvanceCredit)),
                             monthlydeduction = a.Max(b => b.AdvanceCutOff)
                         }).ToList() on a.recordid equals b.recordid
        into gj
                        from subdet in gj.DefaultIfEmpty()
                        select new
                        {
                            a.recordid,
                            a.empno,
                            a.empname,
                            a.mobile,
                            a.department,
                            a.designation,
                            prevadvance = subdet == null ? 0 : subdet.prevadvance,
                            monthlydeduction = subdet == null ? 0 : subdet.monthlydeduction
                        }).OrderBy(b => b.empname).ToList();
            }
            else
            {
                return (from a in list1
                        select new
                        {
                            a.recordid,
                            a.empno,
                            a.empname,
                            a.mobile,
                            a.department,
                            a.designation,
                            prevadvance =0,
                            monthlydeduction = 0
                        }).OrderBy(b => b.empname).ToList();
            }
        }


        [HttpPost]
        [Route("api/HRDAdvances/GetHRDAdvances")]
        [Authorize]
        public dynamic GetHRDAdvances([FromBody] GeneralInformation inf)
        {
            var dat1 = DateTime.Parse(inf.frmDate);
            var dat2 = dat1.AddDays(1);
            return (from a in db.HrdAdvances.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                       join b in db.HrdEmployees.Where(a => a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.Empno equals b.RecordId
                       select new
                       {
                           recordid = a.RecordId,
                           empno = b.Empno,
                           empname = b.Empname,
                           mobile = b.Mobile,
                           advance = a.AdvanceDebit,
                           monthlycutoff = a.AdvanceCutOff
                       }).OrderBy(b => b.recordid).ToList();
        }
        [HttpPost]
        [Route("api/HRDAdvances/setAdvance")]
        [Authorize]
        public TransactionResult setAdvance([FromBody] HRDAdvanceTotal tot)
        {
            string msg = "";
            try
            {
                tot.advance.Dat = ac.getPresentDateTime();
                tot.advance.AdvanceCredit = 0;
                tot.advance.BranchId = tot.usr.bCode;
                tot.advance.CustomerCode = tot.usr.cCode;
                db.HrdAdvances.Add(tot.advance);
                db.SaveChanges();
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
