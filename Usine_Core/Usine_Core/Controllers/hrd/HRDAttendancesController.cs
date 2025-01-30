using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usine_Core.Models;
using Usine_Core.Controllers.Admin;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Data.SqlClient;

namespace Usine_Core.Controllers.hrd
{
    public class AttendancesTotal
    {
        public string dat { get; set; }
        public List<HrdAttendances> attendances { get; set; }
        public UserInfo usr { get; set; }
    }
    public class HRDAttendancesController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();


        [HttpPost]
        [Route("api/HRDAttendances/GetHRDAttendanceRequirements")]
        [Authorize]
        public dynamic GetHRDAttendanceRequirements([FromBody] GeneralInformation inf)
        {
            DateTime dat1 = DateTime.Parse(inf.frmDate);
            DateTime dat2 = dat1.AddDays(1);

            var lst1 = (from a in db.HrdEmployees.Where(a =>   a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                        join b in db.HrdDepartments.Where(a => a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.Department equals b.RecordId
                        join c in db.HrdDesignations.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.Designation equals c.RecordId
                        select new
                        {
                            a.RecordId,
                            a.Empno,
                            a.Empname,
                            c.Designation,
                            department = b.SGrp
                        }).ToList();
            var lst2 = db.HrdEmpInOutDetails.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).ToList();
            
            return (from a in lst1 join b in lst2 on a.RecordId equals b.EmpId into gj
                       from subdet in gj.DefaultIfEmpty()
                       select new
                       {
                           a.RecordId,
                           a.Empno,
                           a.Empname,
                           a.Designation,
                           a.department,
                           fromtime=subdet==null?"":subdet.FromTime,
                           totime=subdet==null?"":subdet.ToTime
                           
                       }).ToList();


        }
        [HttpPost]
        [Route("api/HRDAppointments/SetHRDAppointment")]
        [Authorize]
        public TransactionResult SetHRDAppointment([FromBody] AttendancesTotal tot)
        {
            string msg = "";
            DateTime dat1 = DateTime.Parse(tot.dat);
            DateTime dat2 = dat1.AddDays(1);
            try
            {
                var lst = db.HrdAttendances.Where(a => a.Dat >= dat1 && a.Dat < dat2 && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).ToList();
                if(lst.Count() > 0)
                {
                    db.HrdAttendances.RemoveRange(lst);
                }
                foreach(var ln in tot.attendances)
                {
                    ln.BranchId = tot.usr.bCode;
                    ln.CustomerCode = tot.usr.cCode;
                }
                if(tot.attendances.Count() > 0)
                {
                    db.HrdAttendances.AddRange(tot.attendances);
                }
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
