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

    public class HRDLeaveRequestTotal
    {
        public HrdLeaveApplications leave { get; set; }
        public UserInfo usr { get; set; }
    }

    public class EmployeeLeaveInfo
    {
        public long? recordid { get; set; }
        public string empno { get; set; }
        public string empname { get; set; }
        public string mobile { get; set; }
        public string department { get; set; }
        public string designation { get; set; }
        public double? prevleaves { get; set; }
           
     
    }
    public class HRDLeaveRequestRequirements
    {
        public List<EmployeeLeaveInfo> employees { get; set; }
        public dynamic pendingrequests { get; set; }
    }
    
    public class HRDLeavesTraController : ControllerBase
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();
        [HttpPost]
        [Route("api/HRDLeavesTra/GetHRDLeaveRequestRequirements")]
        [Authorize]
        public HRDLeaveRequestRequirements  GetHRDLeaveRequestRequirements([FromBody] UserInfo usr)
        {
            DateTime dat = ac.getPresentDateTime();
        
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
            var detss = db.HrdLeaveApplications.Where(a => a.ApprovalStatus == 1 && a.Dat.Value.Year == dat.Year && a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();

            if (detss.Count() > 0)
            {
                var list2 = (from a in detss.GroupBy(b => b.Empno)
                             select new
                             {
                                 recordid = a.Key,
                                 prevleaves = a.Sum(b => (b.LeaveTo.Value - b.LeaveFrom.Value).TotalDays)
                             }).ToList();
            }
            HRDLeaveRequestRequirements tot = new HRDLeaveRequestRequirements();
            if (detss.Count() > 0)
            {
                tot.employees = (from a in list1
                           join b in (from a in detss.GroupBy(b => b.Empno)
                                      select new
                                      {
                                          recordid = a.Key,
                                          prevleaves = a.Sum(b => (b.LeaveTo.Value - b.LeaveFrom.Value).TotalDays)
                                      }).ToList() on a.recordid equals b.recordid
           into gj
                           from subdet in gj.DefaultIfEmpty()
                           select new EmployeeLeaveInfo
                           {
                              recordid= a.recordid,
                              empno= a.empno,
                              empname =a.empname,
                               mobile = a.mobile,
                               department = a.department,
                                designation = a.designation,
                               prevleaves = (subdet == null ? 0 : subdet.prevleaves)+1
                           }).OrderBy(b => b.empname).ToList();
            }
            else
            {
                tot.employees = (from a in list1
                                 select new EmployeeLeaveInfo
                                 {
                                     recordid = a.recordid,
                                     empno = a.empno,
                                     empname = a.empname,
                                     mobile = a.mobile,
                                     department = a.department,
                                     designation = a.designation,
                                     prevleaves =0
                                 }).OrderBy(b => b.empname).ToList();
            }
             
            
             
            var lst1 = db.HrdLeaveApplications.Where(a => a.ApprovalStatus == 0 && a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).ToList();
            tot.pendingrequests = (from a in lst1
                                   join b in tot.employees on a.Empno equals b.recordid
                                   select new
                                   {
                                       recordid=a.RecordId,
                                       empno=a.Empno,
                                       empname=b.empname,
                                       mobile=b.mobile,
                                       department=b.designation,
                                       designation=b.designation,
                                       prevleaves=b.prevleaves,
                                       leavefrom=a.LeaveFrom,
                                       leaveto=a.LeaveTo,

                                   }).ToList();
                

            return tot;
                       
        }


        [HttpPost]
        [Route("api/HRDLeavesTra/setLeaveRequest")]
        [Authorize]
        public TransactionResult setLeaveRequest([FromBody] HRDLeaveRequestTotal tot)
        {
            string msg = "";
            try
            {
                tot.leave.Dat = ac.getPresentDateTime();
                tot.leave.ApprovalStatus = 0;
                tot.leave.BranchId = tot.usr.bCode;
                tot.leave.CustomerCode = tot.usr.cCode;
                db.HrdLeaveApplications.Add(tot.leave);
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

       

        [HttpPost]
        [Route("api/HRDLeavesTra/setLeaveRequestApproval")]
        [Authorize]
        public TransactionResult setLeaveRequestApproval([FromBody] GeneralInformation inf)
        {
            
            string msg = "";
            try
            {
                var det = db.HrdLeaveApplications.Where(a => a.RecordId == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).FirstOrDefault();
                if(det != null)
                {
                    det.ApprovalStatus = inf.traCheck;
                    det.ApprovedBy = inf.usr.bCode;
                }
                db.SaveChanges();
                msg = "OK";
            }
            catch (Exception ee)
            {
                msg = ee.Message;
            }
            TransactionResult result = new TransactionResult();
            result.result = msg;
            return result;
        }

        }
}
