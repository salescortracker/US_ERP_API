using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usine_Core.Models;
using Usine_Core.Controllers.Admin;
using Microsoft.AspNetCore.Authorization;
using Usine_Core.Controllers.HRD;
using Usine_Core.others;
using Usine_Core.others.dtos;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.EMMA;

namespace Usine_Core.Controllers.CRM
{
    public class CRMTeleCallRxTotal
    {
        public CrmTeleCallingRx call { get; set; }
        public int traCheck { get; set; }
        public UserInfo usr { get; set; }
        public string result { get; set; }
    }
    public class CRMPendingsList
    {
        public int? id { get; set; }
        public string seq { get; set; }
        public int? callerId { get; set; }
        public String customer { get; set; }
        public String mobile { get; set; }
        public String mode { get; set; }
        public DateTime dat { get; set; }
        public string customercomments { get; set; }
        public string username { get; set; }
    }
    public class CRMTeleCallRequirements
    {
        public String seq { get; set; }
        public List<Employees> employees { get; set; }
        public List<CRMPendingsList> pendings { get; set; }
    }
    public class CRMTeleCallingRXController : Controller
    {
        UsineContext db = new UsineContext();
        AdminControl ac = new AdminControl();


        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRxTelecallRequirements")]
        public CRMTeleCallRequirements GetCRMRxTelecallRequirements([FromBody] UserInfo usr)
        {
            hrdDepartmentsController hr = new hrdDepartmentsController();
            CRMTeleCallRequirements tot = new CRMTeleCallRequirements();
            tot.seq = findSeq(usr);
            GeneralInformation inf = new GeneralInformation();
            inf.detail = "Sales";
            inf.usr = usr;
            tot.employees = hr.getEmployeesByDepartment(inf);
            GeneralInformation inf1 = new GeneralInformation();
            inf1.recordId = 1;
            inf1.detail = "TELECALL";
            inf1.usr = usr;
            tot.pendings = getPendingCallsList(inf1);
            return tot;
        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRxTelecalls")]
        public List<CrmTeleCallingRx> GetCRMRxTelecalls([FromBody]GeneralInformation inf)
        {
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
                 dat2= DateTime.Now;
            }
            UsineContext db1 = new UsineContext();
            var det= (from a in db.CrmTeleCallingRx.Where(a => a.Dat.Value.Date >= dat1.Date && a.Dat.Value.Date <= dat2.Date && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                      join b in db.HrdEmployees.Where(a => a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                      on a.Callerid equals b.RecordId into gj
                      from subdet in gj.DefaultIfEmpty()
                      
                      select new CrmTeleCallingRx
                       {
                           RecordId=a.RecordId,
                           Customer=a.Customer,
                           Seq=a.Seq,
                           Email=subdet==null? " ":subdet.Empname,
                           Mobile=a.Mobile,
                           CallPosition=a.CallPosition,
                          CustomerComments=a.callreason!=null? db.crmcallforreasons.Where(x=>x.branch_id==a.BranchId && x.customer_code==a.CustomerCode.ToString() && x.id==a.callreason).FirstOrDefault().description:""

                      }).OrderBy(e => e.Seq).ToList();

            return det;
          
            
        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCrmtelcalls")]
        public List<CrmTeleCallingRx> GetCrmtelcalls([FromBody] GeneralInformation inf)
        {
           
            UsineContext db1 = new UsineContext();
            var result = db.CrmTeleCallingRx.Where(x => x.Customer==inf.detail && x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCrmsaleorders")]
        public List<CrmSaleOrderUni> GetCrmsaleorders([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmSaleOrderUni.Where(x => x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCrmtelcallsbynumber")]
        public List<CrmTeleCallingRx> GetCrmtelcallsbynumber([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmTeleCallingRx.Where(x => x.Seq==inf.detail && x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/Getcrmenquiries")]
        public List<CrmEnquiriesRx> Getcrmenquiries([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmEnquiriesRx.Where(x => x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/Getcrmenquiriesbytelecalno")]
        public List<CrmEnquiriesRx> Getcrmenquiriesbytelecalno([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmEnquiriesRx.Where(x =>x.telecallingno==inf.detail && x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/Getcrmquotations")]
        public List<CrmQuotationUni> Getcrmquotations([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmQuotationUni.Where(x => x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/Getcrmorders")]
        public List<CrmSaleOrderUni> Getcrmorders([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmSaleOrderUni.Where(x => x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).OrderByDescending(x=>x.RecordId).ToList();

            return result;


        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/Getcrmquotationsbycustomers")]
        public List<CrmQuotationUni> Getcrmquotationsbycustomers([FromBody] GeneralInformation inf)
        {

            UsineContext db1 = new UsineContext();
            var result = db.CrmQuotationUni.Where(x =>x.PartyName==inf.detail && x.BranchId == inf.usr.bCode && x.CustomerCode == inf.usr.cCode).ToList();

            return result;


        }
       
        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/GetCRMRxTelecallById")]
        public dynamic GetCRMRxTelecallById([FromBody] GeneralInformation inf)
        {

            var recid = (int?)inf.recordId;

            UsineContext db1 = new UsineContext();
            var list1 = db.CrmTeleCallingRx.Where(a => a.RecordId == recid && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).ToList();
            var list2_01 = (from a in db.CrmTeleCallingRx.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                            join b in db.HrdEmployees.Where(a => a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.Callerid equals b.RecordId
                            select new CrmTeleCallingRx
                            {
                                RecordId = a.RecordId,
                                Seq = a.Seq,
                                Dat = a.Dat,
                                BranchId = "TeleCall",
                                PrevCallMode = "1",
                                Mobile = a.Mobile,
                                Email = b.Empname,
                                Username = a.Username,
                                CustomerComments = a.CustomerComments,
                                callreason=a.callreason
                            }
                          ).ToList();
            var list2_02 = (from a in db.CrmEnquiriesRx.Where(a => a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode)
                            join b in db.HrdEmployees.Where(a => a.Branchid == inf.usr.bCode && a.CustomerCode == inf.usr.cCode) on a.Callerid equals b.RecordId
                            select new CrmTeleCallingRx
                            {
                                RecordId = a.RecordId,
                                Seq = a.Seq,
                                Dat = a.Dat,
                                BranchId = "Enquiry",
                                PrevCallMode = "2",
                                Mobile = a.Mobile,
                                Email = b.Empname,
                                Username = a.username,
                                CustomerComments = a.CustomerComments,
                                
                            }
                       ).ToList();
             
            var list2 = new List<CrmTeleCallingRx>();
            if(list2_01.Count > 0 && list2_02.Count > 0)
            {
                var list = list2_01.Union(list2_02);
                foreach(var x in list)
                {
                    list2.Add(new CrmTeleCallingRx
                    {
                        RecordId = x.RecordId,
                        Seq = x.Seq,
                        Dat = x.Dat,
                        BranchId = x.BranchId,
                        PrevCallMode = x.PrevCallMode,
                        Mobile = x.Mobile,
                        Email = x.Email,
                        Username = x.Username,
                        CustomerComments = x.CustomerComments,
                        callreason=x.callreason
                    });
                }
            }
            else
            {
                if(list2_01.Count > 0)
                {
                    var list = list2_01;
                    foreach (var x in list)
                    {
                        list2.Add(new CrmTeleCallingRx
                        {
                            RecordId = x.RecordId,
                            Seq = x.Seq,
                            Dat = x.Dat,
                            BranchId = x.BranchId,
                            PrevCallMode = x.PrevCallMode,
                            Mobile = x.Mobile,
                            Email = x.Email,
                            Username = x.Username,
                            CustomerComments = x.CustomerComments,
                            callreason=x.callreason
                        });
                    }
                }
                if(list2_02.Count > 0)
                {
                    var list = list2_02;
                    foreach (var x in list)
                    {
                        list2.Add(new CrmTeleCallingRx
                        {
                            RecordId = x.RecordId,
                            Seq = x.Seq,
                            Dat = x.Dat,
                            BranchId = x.BranchId,
                            PrevCallMode = x.PrevCallMode,
                            Mobile = x.Mobile,
                            Email = x.Email,
                            Username = x.Username,
                            CustomerComments = x.CustomerComments,
                            callreason=x.callreason
                        });
                    }
                }
            }
            var det = (from a in list1
                       join b in list2
                       on new { id = a.PrevcallId, callmode = a.PrevCallMode } equals new { id = b.RecordId, callmode = b.PrevCallMode } into gj
                       from subdet in gj.DefaultIfEmpty()
                       select new
                       {
                           RecordId = a.RecordId,
                           Customer = a.Customer,
                           Seq = a.Seq,
                           Mobile = a.Mobile,
                           Email = a.Email,
                           PrevcallId = subdet == null ? null : subdet.RecordId,
                           prevseq = subdet == null ? null : subdet.Seq,
                           prevDat = subdet == null ? null : subdet.Dat,
                           prevCallMode = subdet == null ? null : subdet.BranchId,
                           prevMobile = subdet == null ? null : subdet.Mobile,
                           prevComments = subdet == null ? null : subdet.CustomerComments,
                           prevemployee=subdet== null ? null : subdet.Email,
                           prevuser=subdet == null ? null : subdet.Username,
                           customerComments = a.CustomerComments,
                           callerCommnets = a.CallerComments,
                           pos = a.CallPosition,
                           nextcalldate = a.NextCallDate,
                           nextcallmode = a.NextCallMode
                           ,callreason=a.callreason
                       }).FirstOrDefault();

            return det;


        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMRx/SetCRMRXTeleCall")]
        public CRMTeleCallRxTotal SetCRMRXTeleCall([FromBody]CRMTeleCallRxTotal tot)
        {
            String msg = "";
            String res = ac.transactionCheck("CRM", tot.call.Dat, tot.usr);
            StringConversions sc = new StringConversions();
            DateTime dat = ac.getPresentDateTime(); // DateTime.Parse(ac.strDate(tot.call.Dat.Value) + " " + ac.strTime(ac.getPresentDateTime()));
            if (res == "OK")
            {
                try
                {
                    switch (tot.traCheck)
                    {
                        case 1:
                            if (ac.screenCheck(tot.usr, 7, 2, 2, 1))
                            {
                                var usrname = sc.makeStringToAscii(tot.usr.uCode.ToLower());
                                var empp = db.UserCompleteProfile.Where(a => a.UsrName == usrname && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                
                                CrmTeleCallingRx call = new CrmTeleCallingRx();
                                call.Seq = findSeq(tot.usr);
                                call.Dat = dat;
                                 call.Customer = tot.call.Customer;
                                if (empp != null)
                                {
                                    var emp = db.HrdEmployees.Where(a => a.RecordId == empp.EmployeeNo && a.Branchid == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                   if(emp != null)
                                    {
                                        call.Callerid = (int?)emp.RecordId;
                                    }
                                }
                                call.Mobile = tot.call.Mobile;
                                call.Email = tot.call.Email;
                                 call.PrevcallId = tot.call.PrevcallId;
                                //call.PrevCallMode = tot.call.PrevCallMode;
                                call.CustomerComments = tot.call.CustomerComments;
                                call.CallerComments = tot.call.CallerComments;
                                call.CallPosition = tot.call.CallPosition;
                                call.NextCallDate = tot.call.NextCallDate;
                                call.NextCallMode = tot.call.NextCallMode;
                                call.ReminderCheck = tot.call.ReminderCheck;
                                call.Username = tot.usr.uCode;
                                call.BranchId = tot.usr.bCode;
                                call.CustomerCode = tot.usr.cCode;
                                call.callreason = tot.call.callreason;
                                db.CrmTeleCallingRx.Add(call);
                                db.SaveChanges();

                                if (tot.call.PrevcallId !=null)
                                {
                                    if(tot.call.PrevCallMode=="TELECALL")
                                    {
                                        var prevcall = db.CrmTeleCallingRx.Where(a => a.RecordId == tot.call.PrevcallId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                        prevcall.nextCallId = call.RecordId;
                                     //   prevcall.PrevCallMode = "TELECALL";
                                    }
                                    else
                                    {
                                        var prevcall1 = db.CrmEnquiriesRx.Where(a => a.RecordId == tot.call.PrevcallId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                        prevcall1.nextCallId = call.RecordId;
                                      //  prevcall1.PrevCallMode = "TELECALL";
                                    }
                                    db.SaveChanges();
                                }
                                msg = "OK";
                            }
                            else
                            {
                                msg = "You are not authorised for tele calling";
                            }
                            break;
                        case 2:
                            if (ac.screenCheck(tot.usr, 7, 2, 2, 2))
                            {
                                var call = db.CrmTeleCallingRx.Where(a => a.RecordId == tot.call.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                if (call != null)
                                {
                                    call.Dat = tot.call.Dat;
                                    call.Callerid = tot.call.Callerid;
                                    call.Customer = tot.call.Customer;
                                    call.Mobile = tot.call.Mobile;
                                    call.Email = tot.call.Email;
                                    call.PrevcallId = tot.call.PrevcallId;
                                    call.PrevCallMode = tot.call.PrevCallMode;
                                    call.CustomerComments = tot.call.CustomerComments;
                                    call.CallerComments = tot.call.CallerComments;
                                    call.CallPosition = tot.call.CallPosition;
                                    call.NextCallDate = tot.call.NextCallDate;
                                    call.NextCallMode = tot.call.NextCallMode;
                                    call.ReminderCheck = tot.call.ReminderCheck;
                                    call.Username = tot.usr.uCode;
                                    call.BranchId = tot.usr.bCode;
                                    call.CustomerCode = tot.usr.cCode;
                                    call.callreason = tot.call.callreason;
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
                                msg = "You are not authorised for tele calling";
                            }
                            break;
                        case 3:
                            if (ac.screenCheck(tot.usr, 7, 2, 2, 3))
                            {
                                var call = db.CrmTeleCallingRx.Where(a => a.RecordId == tot.call.RecordId && a.BranchId == tot.usr.bCode && a.CustomerCode == tot.usr.cCode).FirstOrDefault();
                                if (call != null)
                                {
                                   
                                    db.CrmTeleCallingRx.Remove(call);
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
                                msg = "You are not authorised for tele calling";
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
        
        
        private String findSeq(UserInfo usr)
        {
            DateTime dat = ac.getPresentDateTime();
            General g = new General();
            String seq = db.CrmTeleCallingRx.Where(a => a.Dat.Value.Year == dat.Year && a.BranchId == usr.bCode && a.CustomerCode == usr.cCode).Max(b => b.Seq);
            int x = 0;
            if (seq != null)
            {
                x = int.Parse(seq.Substring(5, 7));
            }
            x++;
            return "TC" + dat.Year.ToString().Substring(2, 2) + "-" + g.zeroMake(x, 7);
        }

        public List<CRMPendingsList> getPendingCallsList(GeneralInformation inf)
        {
            List<CRMPendingsList> lst = new List<CRMPendingsList>();
            DateTime dat = ac.getPresentDateTime();
            var det1 = db.CrmTeleCallingRx.Where(a => a.NextCallDate <= dat && a.nextCallId ==null && a.NextCallMode == inf.recordId.ToString() && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).ToList();
            var det2 = db.CrmEnquiriesRx.Where(a => a.NextCallDate <= dat && a.nextCallId==null && a.NextCallMode == inf.recordId && a.BranchId == inf.usr.bCode && a.CustomerCode == inf.usr.cCode).ToList();
            foreach(CrmTeleCallingRx det in det1)
            {
                lst.Add(new CRMPendingsList
                {
                    id = det.RecordId,
                    seq=det.Seq,
                    callerId=det.Callerid,
                    customer = det.Customer,
                    mobile = det.Mobile,
                    mode = "TeleCall",
                    dat = det.Dat.Value,
                    customercomments=det.CustomerComments,
                    username=det.Username
                });
            }
            foreach (CrmEnquiriesRx det in det2)
            {
                lst.Add(new CRMPendingsList
                {
                    id = det.RecordId,
                    seq=det.Seq,
                    callerId=det.Callerid,
                    customer = det.Customer,
                    mobile = det.Mobile,
                    mode = "Enquiry",
                    dat = det.Dat.Value,
                    customercomments=det.CustomerComments,
                    username=det.username
                    
                });
            }
            return lst;
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmcallforreason")]
        public IActionResult getcrmcallforreason([FromBody] dynamic model)
        {
            crmcallforreasondto crmcallforreasondto=JsonConvert.DeserializeObject<crmcallforreasondto>(model.ToString());

            var result = db.crmcallforreasons.Where(x =>x.branch_id==crmcallforreasondto.branch_id && x.customer_code == crmcallforreasondto.customer_code).Select(x => new crmcallforreasondto
            {
              id=x.id,
              description=x.description,
              branch_id=x.branch_id,
              customer_code=x.customer_code
                
            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/savecrmcallforreason")]
        public IActionResult savecrmcallforreason([FromBody] dynamic model)
        {
            crmcallforreasondto crmcallforreasondto = JsonConvert.DeserializeObject<crmcallforreasondto>(model.ToString());

            if (crmcallforreasondto != null)
            {
                crmcallforreason crmcallforreason = new crmcallforreason();
                crmcallforreason.description = crmcallforreasondto.description;
                crmcallforreason.branch_id = crmcallforreasondto.branch_id;
                crmcallforreason.customer_code = crmcallforreasondto.customer_code;
                crmcallforreason.created_at = DateTime.Now;
                db.crmcallforreasons.Add(crmcallforreason);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatecrmcallforreason")]
        public IActionResult updatecrmcallforreason([FromBody] dynamic model)
        {
            crmcallforreasondto crmcallforreasondto = JsonConvert.DeserializeObject<crmcallforreasondto>(model.ToString());

            var result = db.crmcallforreasons.Where(x => x.id == crmcallforreasondto.id && x.branch_id == crmcallforreasondto.branch_id && x.customer_code == crmcallforreasondto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.description = crmcallforreasondto.description;
                result.branch_id = crmcallforreasondto.branch_id;
                result.customer_code = crmcallforreasondto.customer_code;
                result.modified_at = DateTime.Now;
                db.crmcallforreasons.Update(result);
                db.SaveChanges();
            }

          
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deletecrmcallforreason")]
        public IActionResult deletecrmcallforreason([FromBody] dynamic model)
        {
            crmcallforreasondto crmcallforreasondto = JsonConvert.DeserializeObject<crmcallforreasondto>(model.ToString());

            var result = db.crmcallforreasons.Where(x => x.id == crmcallforreasondto.id && x.branch_id == crmcallforreasondto.branch_id && x.customer_code == crmcallforreasondto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.crmcallforreasons.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmcallforreasonById")]
        public IActionResult getcrmcallforreasonById([FromBody] dynamic model)
        {
            crmcallforreasondto crmcallforreasondto = JsonConvert.DeserializeObject<crmcallforreasondto>(model.ToString());

            var result = db.crmcallforreasons.
                Where(x => x.id == crmcallforreasondto.id && x.branch_id == crmcallforreasondto.branch_id && x.customer_code == crmcallforreasondto.customer_code)
                .Select(x=>new crmcallforreasondto
                {
                    description=x.description,
                    id=x.id,
                    branch_id=x.branch_id,
                    customer_code=x.customer_code
                }).FirstOrDefault();
           


            return Ok(result);
        }
        //order status
        [HttpPost]
        [Route("api/CRMRx/getcrmorderstatus")]
        public IActionResult getcrmorderstatus([FromBody] dynamic model)
        {
            crmorderstatusdto crmorderstatusdto = JsonConvert.DeserializeObject<crmorderstatusdto>(model.ToString());

            var result = db.Crmorderstatuses.Where(x => x.branch_id == crmorderstatusdto.branch_id && x.customer_code == crmorderstatusdto.customer_code).Select(x => new crmorderstatusdto
            {
                id = x.id,
                description = x.description,
                branch_id = x.branch_id,
                customer_code = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/savecrmorderstatus")]
        public IActionResult savecrmorderstatus([FromBody] dynamic model)
        {
            crmorderstatusdto crmorderstatusdto = JsonConvert.DeserializeObject<crmorderstatusdto>(model.ToString());

            if (crmorderstatusdto != null)
            {
                crmorderstatus crmorderstatus = new crmorderstatus();
                crmorderstatus.description = crmorderstatusdto.description;
                crmorderstatus.branch_id = crmorderstatusdto.branch_id;
                crmorderstatus.customer_code = crmorderstatusdto.customer_code;
                crmorderstatus.created_at = DateTime.Now;
                db.Crmorderstatuses.Add(crmorderstatus);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatecrmcorderstatus")]
        public IActionResult updatecrmcorderstatus([FromBody] dynamic model)
        {
            crmorderstatusdto crmorderstatusdto = JsonConvert.DeserializeObject<crmorderstatusdto>(model.ToString());

            var result = db.Crmorderstatuses.Where(x => x.id == crmorderstatusdto.id && x.branch_id == crmorderstatusdto.branch_id && x.customer_code == crmorderstatusdto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.description = crmorderstatusdto.description;
                result.branch_id = crmorderstatusdto.branch_id;
                result.customer_code = crmorderstatusdto.customer_code;
                result.modified_at = DateTime.Now;
                db.Crmorderstatuses.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deletecrmorderstatus")]
        public IActionResult deletecrmorderstatus([FromBody] dynamic model)
        {
            crmorderstatusdto crmorderstatusdto = JsonConvert.DeserializeObject<crmorderstatusdto>(model.ToString());

            var result = db.Crmorderstatuses.Where(x => x.id == crmorderstatusdto.id && x.branch_id == crmorderstatusdto.branch_id && x.customer_code == crmorderstatusdto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.Crmorderstatuses.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmorderstatusById")]
        public IActionResult getcrmorderstatusById([FromBody] dynamic model)
        {
            crmorderstatusdto crmorderstatusdto = JsonConvert.DeserializeObject<crmorderstatusdto>(model.ToString());

            var result = db.Crmorderstatuses.
                Where(x => x.id == crmorderstatusdto.id && x.branch_id == crmorderstatusdto.branch_id && x.customer_code == crmorderstatusdto.customer_code)
                .Select(x => new crmorderstatusdto
                {
                    description = x.description,
                    id = x.id,
                    branch_id = x.branch_id,
                    customer_code = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }
        //industry
        //order status
        [HttpPost]
        [Route("api/CRMRx/getcrmleadindustry")]
        public IActionResult getcrmleadindustry([FromBody] dynamic model)
        {
            CrmIndustryDto crmIndustryDto = JsonConvert.DeserializeObject<CrmIndustryDto>(model.ToString());

            var result = db.CrmIndustries.Where(x => x.BranchId == crmIndustryDto.BranchId && x.CustomerCode == crmIndustryDto.CustomerCode).Select(x => new CrmIndustryDto
            {
                Id = x.Id,
                Description = x.Description,
                BranchId = x.BranchId,
                CustomerCode = x.CustomerCode

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadindustry")]
        public IActionResult saveLeadindustry([FromBody] dynamic model)
        {
            CrmIndustryDto crmIndustryDto = JsonConvert.DeserializeObject<CrmIndustryDto>(model.ToString());

            if (crmIndustryDto != null)
            {
                CrmIndustry crmIndustry = new CrmIndustry();
                crmIndustry.Description = crmIndustryDto.Description;
                crmIndustry.BranchId = crmIndustryDto.BranchId;
                crmIndustry.CustomerCode = crmIndustryDto.CustomerCode;
                crmIndustry.CreatedAt = DateTime.Now;
                db.CrmIndustries.Add(crmIndustry);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadindustry")]
        public IActionResult updateLeadindustry([FromBody] dynamic model)
        {
            CrmIndustryDto crmIndustryDto = JsonConvert.DeserializeObject<CrmIndustryDto>(model.ToString());

            var result = db.CrmIndustries.Where(x => x.Id == crmIndustryDto.Id && x.BranchId == crmIndustryDto.BranchId && x.CustomerCode == crmIndustryDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                result.Description = crmIndustryDto.Description;
                result.BranchId = crmIndustryDto.BranchId;
                result.CustomerCode = crmIndustryDto.CustomerCode;
                result.ModifiedAt = DateTime.Now;
                db.CrmIndustries.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadindustry")]
        public IActionResult deleteLeadindustry([FromBody] dynamic model)
        {
            CrmIndustryDto crmIndustryDto = JsonConvert.DeserializeObject<CrmIndustryDto>(model.ToString());

            var result = db.CrmIndustries.Where(x => x.Id == crmIndustryDto.Id && x.BranchId == crmIndustryDto.BranchId && x.CustomerCode == crmIndustryDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                db.CrmIndustries.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadindustryById")]
        public IActionResult getcrmleadindustryById([FromBody] dynamic model)
        {
            CrmIndustryDto crmIndustryDto = JsonConvert.DeserializeObject<CrmIndustryDto>(model.ToString());

            var result = db.CrmIndustries.
                Where(x => x.Id == crmIndustryDto.Id && x.BranchId == crmIndustryDto.BranchId && x.CustomerCode == crmIndustryDto.CustomerCode)
                .Select(x => new CrmIndustryDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    BranchId = x.BranchId,
                    CustomerCode = x.CustomerCode
                }).FirstOrDefault();



            return Ok(result);
        }

        //call types
        [HttpPost]
        [Route("api/CRMRx/getcrmleadcalltypes")]
        public IActionResult getcrmleadcalltypes([FromBody] dynamic model)
        {
            CrmCallTypeDto crmCallTypeDto = JsonConvert.DeserializeObject<CrmCallTypeDto>(model.ToString());

            var result = db.CrmCallTypes.Where(x => x.BranchId == crmCallTypeDto.BranchId && x.customercode == crmCallTypeDto.customercode).Select(x => new CrmCallTypeDto
            {
                Id = x.Id,
                Description = x.Description,
                BranchId = x.BranchId,
                customercode = x.customercode

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadcalltypes")]
        public IActionResult saveLeadcalltypes([FromBody] dynamic model)
        {
            CrmCallTypeDto crmCallTypeDto = JsonConvert.DeserializeObject<CrmCallTypeDto>(model.ToString());

            if (crmCallTypeDto != null)
            {
                CrmCallTypes crmCallTypes = new CrmCallTypes();
                crmCallTypes.Description = crmCallTypeDto.Description;
                crmCallTypes.BranchId = crmCallTypeDto.BranchId;
                crmCallTypes.customercode = crmCallTypeDto.customercode;
                crmCallTypes.CreatedAt = DateTime.Now;
                db.CrmCallTypes.Add(crmCallTypes);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadcalltypes")]
        public IActionResult updateLeadcalltypes([FromBody] dynamic model)
        {
            CrmCallTypeDto crmCallTypeDto = JsonConvert.DeserializeObject<CrmCallTypeDto>(model.ToString());

            var result = db.CrmCallTypes.Where(x => x.Id == crmCallTypeDto.Id && x.BranchId == crmCallTypeDto.BranchId && x.customercode == crmCallTypeDto.customercode).FirstOrDefault();
            if (result != null)
            {
                result.Description = crmCallTypeDto.Description;
                result.BranchId = crmCallTypeDto.BranchId;
                result.customercode = crmCallTypeDto.customercode;
                result.ModifiedAt = DateTime.Now;
                db.CrmCallTypes.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadcalltypes")]
        public IActionResult deleteLeadcalltypes([FromBody] dynamic model)
        {
            CrmCallTypeDto crmCallTypeDto = JsonConvert.DeserializeObject<CrmCallTypeDto>(model.ToString());

            var result = db.CrmCallTypes.Where(x => x.Id == crmCallTypeDto.Id && x.BranchId == crmCallTypeDto.BranchId && x.customercode == crmCallTypeDto.customercode).FirstOrDefault();
            if (result != null)
            {
                db.CrmCallTypes.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadcalltypesById")]
        public IActionResult getcrmleadcalltypesById([FromBody] dynamic model)
        {
            CrmCallTypeDto crmCallTypeDto = JsonConvert.DeserializeObject<CrmCallTypeDto>(model.ToString());

            var result = db.CrmCallTypes.
                Where(x => x.Id == crmCallTypeDto.Id && x.BranchId == crmCallTypeDto.BranchId && x.customercode == crmCallTypeDto.customercode)
                .Select(x => new CrmCallTypeDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    BranchId = x.BranchId,
                    customercode = x.customercode
                }).FirstOrDefault();



            return Ok(result);
        }

        //reminder type

        [HttpPost]
        [Route("api/CRMRx/getcrmleadremindertype")]
        public IActionResult getcrmleadremindertype([FromBody] dynamic model)
        {
            crmleadreminderdto crmleadreminderdto = JsonConvert.DeserializeObject<crmleadreminderdto>(model.ToString());

            var result = db.Crmleadreminders.Where(x => x.branch_id == crmleadreminderdto.branch_id && x.customer_code == crmleadreminderdto.customer_code).Select(x => new crmleadreminderdto
            {
                id = x.id,
                description = x.description,
                branch_id = x.branch_id,
                customer_code = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadremindertype")]
        public IActionResult saveLeadremindertype([FromBody] dynamic model)
        {
            crmleadreminderdto crmleadreminderdto = JsonConvert.DeserializeObject<crmleadreminderdto>(model.ToString());

            if (crmleadreminderdto != null)
            {
                crmleadreminder crmRemainder = new crmleadreminder();
                crmRemainder.description = crmleadreminderdto.description;
                crmRemainder.branch_id = crmleadreminderdto.branch_id;
                crmRemainder.customer_code = crmleadreminderdto.customer_code;
                crmRemainder.created_at = DateTime.Now;
                db.Crmleadreminders.Add(crmRemainder);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadremindertypes")]
        public IActionResult updateLeadremindertypes([FromBody] dynamic model)
        {
            crmleadreminderdto crmleadreminderdto = JsonConvert.DeserializeObject<crmleadreminderdto>(model.ToString());

            var result = db.Crmleadreminders.Where(x => x.id == crmleadreminderdto.id && x.branch_id == crmleadreminderdto.branch_id && x.customer_code == crmleadreminderdto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.description = crmleadreminderdto.description;
                result.branch_id = crmleadreminderdto.branch_id;
                result.customer_code = crmleadreminderdto.customer_code;
                result.modified_at = DateTime.Now;
                db.Crmleadreminders.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadremindertypes")]
        public IActionResult deleteLeadremindertypes([FromBody] dynamic model)
        {
            crmleadreminderdto crmleadreminderdto = JsonConvert.DeserializeObject<crmleadreminderdto>(model.ToString());

            var result = db.Crmleadreminders.Where(x => x.id == crmleadreminderdto.id && x.branch_id == crmleadreminderdto.branch_id && x.customer_code == crmleadreminderdto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.Crmleadreminders.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadreminerTypesById")]
        public IActionResult getcrmleadreminerTypesById([FromBody] dynamic model)
        {
            crmleadreminderdto crmleadreminderdto = JsonConvert.DeserializeObject<crmleadreminderdto>(model.ToString());

            var result = db.Crmleadreminders.
                Where(x => x.id == crmleadreminderdto.id && x.branch_id == crmleadreminderdto.branch_id && x.customer_code == crmleadreminderdto.customer_code)
                .Select(x => new crmleadreminderdto
                {
                    description = x.description,
                    id = x.id,
                    branch_id = x.branch_id,
                    customer_code = x.customer_code
                   
                }).FirstOrDefault();



            return Ok(result);
        }
        //call types
        [HttpPost]
        [Route("api/CRMRx/getcrmleadcity")]
        public IActionResult getcrmleadcity([FromBody] dynamic model)
        {
            crmleadcitydto crmleadcitydto = JsonConvert.DeserializeObject<crmleadcitydto>(model.ToString());

            var result = db.crmleadcity.Where(x => x.Branch_Id == crmleadcitydto.Branch_Id && x.customer_code == crmleadcitydto.customer_code).Select(x => new crmleadcitydto
            {
                Id = x.Id,
                Description = x.Description,
                Branch_Id = x.Branch_Id,
                customer_code = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadcity")]
        public IActionResult saveLeadcity([FromBody] dynamic model)
        {
            crmleadcitydto crmleadcitydto = JsonConvert.DeserializeObject<crmleadcitydto>(model.ToString());

            if (crmleadcitydto != null)
            {
                crmleadcity crmleadcity = new crmleadcity();
                crmleadcity.Description = crmleadcitydto.Description;
                crmleadcity.Branch_Id = crmleadcitydto.Branch_Id;
                crmleadcity.customer_code = crmleadcitydto.customer_code;
                crmleadcity.created_at = DateTime.Now;
                db.crmleadcity.Add(crmleadcity);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadcity")]
        public IActionResult updateLeadcity([FromBody] dynamic model)
        {
            crmleadcitydto crmleadcitydto = JsonConvert.DeserializeObject<crmleadcitydto>(model.ToString());

            var result = db.crmleadcity.Where(x => x.Id == crmleadcitydto.Id && x.Branch_Id == crmleadcitydto.Branch_Id && x.customer_code == crmleadcitydto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.Description = crmleadcitydto.Description;
                result.Branch_Id = crmleadcitydto.Branch_Id;
                result.customer_code = crmleadcitydto.customer_code;
                result.modified_at = DateTime.Now;
                db.crmleadcity.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadcity")]
        public IActionResult deleteLeadcity([FromBody] dynamic model)
        {
            crmleadcitydto crmleadcitydto = JsonConvert.DeserializeObject<crmleadcitydto>(model.ToString());

            var result = db.crmleadcity.Where(x => x.Id == crmleadcitydto.Id && x.Branch_Id == crmleadcitydto.Branch_Id && x.customer_code == crmleadcitydto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.crmleadcity.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadcityById")]
        public IActionResult getcrmleadcityById([FromBody] dynamic model)
        {
            crmleadcitydto crmleadcitydto = JsonConvert.DeserializeObject<crmleadcitydto>(model.ToString());

            var result = db.crmleadcity.
                Where(x => x.Id == crmleadcitydto.Id && x.Branch_Id == crmleadcitydto.Branch_Id && x.customer_code == crmleadcitydto.customer_code)
                .Select(x => new crmleadcitydto
                {
                    Description = x.Description,
                    Id = x.Id,
                    Branch_Id = x.Branch_Id,
                    customer_code = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }
        //
        //lead stage
        [HttpPost]
        [Route("api/CRMRx/getcrmleadstage")]
        public IActionResult getcrmleadstage([FromBody] dynamic model)
        {
            CrmLeadStageDto crmLeadStageDto = JsonConvert.DeserializeObject<CrmLeadStageDto>(model.ToString());

            var result = db.crmleadstages.Where(x => x.branch_id == crmLeadStageDto.BranchId && x.customer_code == crmLeadStageDto.CustomerCode).Select(x => new CrmLeadStageDto
            {
                Id = x.id,
                Description = x.description,
                BranchId = x.branch_id,
                CustomerCode = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadStage")]
        public IActionResult saveLeadStage([FromBody] dynamic model)
        {
            CrmLeadStageDto crmLeadStageDto = JsonConvert.DeserializeObject<CrmLeadStageDto>(model.ToString());

            if (crmLeadStageDto != null)
            {
                crmleadstage crmleadstage = new crmleadstage();
                crmleadstage.description = crmLeadStageDto.Description;
                crmleadstage.branch_id = crmLeadStageDto.BranchId;
                crmleadstage.customer_code = crmLeadStageDto.CustomerCode;
                crmleadstage.created_at = DateTime.Now;
                db.crmleadstages.Add(crmleadstage);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadStage")]
        public IActionResult updateLeadStage([FromBody] dynamic model)
        {
            CrmLeadStageDto crmLeadStageDto = JsonConvert.DeserializeObject<CrmLeadStageDto>(model.ToString());

            var result = db.crmleadstages.Where(x => x.id == crmLeadStageDto.Id && x.branch_id == crmLeadStageDto.BranchId && x.customer_code == crmLeadStageDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                result.description = crmLeadStageDto.Description;
                result.branch_id = crmLeadStageDto.BranchId;
                result.customer_code = crmLeadStageDto.CustomerCode;
                result.modified_at = DateTime.Now;
                db.crmleadstages.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadStage")]
        public IActionResult deleteLeadStage([FromBody] dynamic model)
        {
            CrmLeadStageDto crmLeadStageDto = JsonConvert.DeserializeObject<CrmLeadStageDto>(model.ToString());

            var result = db.crmleadstages.Where(x => x.id == crmLeadStageDto.Id && x.branch_id == crmLeadStageDto.BranchId && x.customer_code == crmLeadStageDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                db.crmleadstages.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadstageById")]
        public IActionResult getcrmleadstageById([FromBody] dynamic model)
        {
            CrmLeadStageDto crmLeadStageDto = JsonConvert.DeserializeObject<CrmLeadStageDto>(model.ToString());

            var result = db.crmleadstages.
                Where(x => x.id == crmLeadStageDto.Id && x.branch_id == crmLeadStageDto.BranchId && x.customer_code == crmLeadStageDto.CustomerCode)
                .Select(x => new CrmLeadStageDto
                {
                    Description = x.description,
                    Id = x.id,
                    BranchId = x.branch_id,
                    CustomerCode = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }
        //lead source 
        [HttpPost]
        [Route("api/CRMRx/getcrmleadsource")]
        public IActionResult getcrmleadsource([FromBody] dynamic model)
        {
            CrmLeadSourceDto CrmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.Where(x => x.branch_id == CrmLeadSourceDto.BranchId && x.customer_code == CrmLeadSourceDto.CustomerCode).Select(x => new CrmLeadSourceDto
            {
                Id = x.id,
                Description = x.description,
                BranchId = x.branch_id,
                CustomerCode = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadSource")]
        public IActionResult saveLeadSource([FromBody] dynamic model)
        {
            CrmLeadSourceDto CrmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            if (CrmLeadSourceDto != null)
            {
                crmleadsource crmleadsource = new crmleadsource();
                crmleadsource.description = CrmLeadSourceDto.Description;
                crmleadsource.branch_id = CrmLeadSourceDto.BranchId;
                crmleadsource.customer_code = CrmLeadSourceDto.CustomerCode;
                crmleadsource.created_at = DateTime.Now;
                db.crmleadsources.Add(crmleadsource);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadSource")]
        public IActionResult updateLeadSource([FromBody] dynamic model)
        {
            CrmLeadSourceDto CrmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.Where(x => x.id == CrmLeadSourceDto.Id && x.branch_id == CrmLeadSourceDto.BranchId && x.customer_code == CrmLeadSourceDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                result.description = CrmLeadSourceDto.Description;
                result.branch_id = CrmLeadSourceDto.BranchId;
                result.customer_code = CrmLeadSourceDto.CustomerCode;
                result.modified_at = DateTime.Now;
                db.crmleadsources.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadSource")]
        public IActionResult deleteLeadSource([FromBody] dynamic model)
        {
            CrmLeadSourceDto CrmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.Where(x => x.id == CrmLeadSourceDto.Id && x.branch_id == CrmLeadSourceDto.BranchId && x.customer_code == CrmLeadSourceDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                db.crmleadsources.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadsourceById")]
        public IActionResult getcrmleadsourceById([FromBody] dynamic model)
        {
            CrmLeadSourceDto CrmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.
                Where(x => x.id == CrmLeadSourceDto.Id && x.branch_id == CrmLeadSourceDto.BranchId && x.customer_code == CrmLeadSourceDto.CustomerCode)
                .Select(x => new CrmLeadSourceDto
                {
                    Description = x.description,
                    Id = x.id,
                    BranchId = x.branch_id,
                    CustomerCode = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }
        //lead status 
        [HttpPost]
        [Route("api/CRMRx/getcrmleadstatus")]
        public IActionResult getcrmleadstatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto CrmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.Where(x => x.BranchId == CrmLeadStatusDto.BranchId && x.CustomerCode == CrmLeadStatusDto.CustomerCode).Select(x => new CrmLeadStatusDto
            {
                Id = x.Id,
                Description = x.Description,
                BranchId = x.BranchId,
                CustomerCode = x.CustomerCode

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveLeadStatus")]
        public IActionResult saveLeadStatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto CrmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            if (CrmLeadStatusDto != null)
            {
                crmleadstatus crmleadstatus = new crmleadstatus();
                crmleadstatus.Description = CrmLeadStatusDto.Description;
                crmleadstatus.BranchId = CrmLeadStatusDto.BranchId;
                crmleadstatus.CustomerCode = CrmLeadStatusDto.CustomerCode;
                crmleadstatus.CreatedAt = DateTime.Now;
                db.crmleadstatuses.Add(crmleadstatus);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateLeadStatus")]
        public IActionResult updateLeadStatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto CrmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.Where(x => x.Id == CrmLeadStatusDto.Id && x.BranchId == CrmLeadStatusDto.BranchId && x.CustomerCode == CrmLeadStatusDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                result.Description = CrmLeadStatusDto.Description;
                result.BranchId = CrmLeadStatusDto.BranchId;
                result.CustomerCode = CrmLeadStatusDto.CustomerCode;
                result.ModifiedAt = DateTime.Now;
                db.crmleadstatuses.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteLeadStatus")]
        public IActionResult deleteLeadStatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto CrmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.Where(x => x.Id == CrmLeadStatusDto.Id && x.BranchId == CrmLeadStatusDto.BranchId && x.CustomerCode == CrmLeadStatusDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                db.crmleadstatuses.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadstatusById")]
        public IActionResult getcrmleadstatusById([FromBody] dynamic model)
        {
            CrmLeadStatusDto CrmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.
                Where(x => x.Id == CrmLeadStatusDto.Id && x.BranchId == CrmLeadStatusDto.BranchId && x.CustomerCode == CrmLeadStatusDto.CustomerCode)
                .Select(x => new CrmLeadStatusDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    BranchId = x.BranchId,
                    CustomerCode = x.CustomerCode
                }).FirstOrDefault();



            return Ok(result);
        }
        //lead owner 
        [HttpPost]
        [Route("api/CRMRx/getcrmleadowner")]
        public IActionResult getcrmleadowner([FromBody] dynamic model)
        {
            crmleadownerdto crmleadownerdto = JsonConvert.DeserializeObject<crmleadownerdto>(model.ToString());

            var result = db.crmleadowner.Where(x => x.branch_id == crmleadownerdto.branch_id && x.customer_code == crmleadownerdto.customer_code).Select(x => new crmleadownerdto
            {
                id = x.id,
                description = x.description,
                branch_id = x.branch_id,
                customer_code = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/savecrmleadowner")]
        public IActionResult savecrmleadowner([FromBody] dynamic model)
        {
            crmleadownerdto crmleadownerdto = JsonConvert.DeserializeObject<crmleadownerdto>(model.ToString());

            if (crmleadownerdto != null)
            {
                crmleadowner crmleadowner = new crmleadowner();
                crmleadowner.description = crmleadownerdto.description;
                crmleadowner.branch_id = crmleadownerdto.branch_id;
                crmleadowner.customer_code = crmleadownerdto.customer_code;
                crmleadowner.created_at = DateTime.Now;
                db.crmleadowner.Add(crmleadowner);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatecrmleadowner")]
        public IActionResult updatecrmleadowner([FromBody] dynamic model)
        {
            crmleadownerdto crmleadownerdto = JsonConvert.DeserializeObject<crmleadownerdto>(model.ToString());

            var result = db.crmleadowner.Where(x => x.id == crmleadownerdto.id && x.branch_id == crmleadownerdto.branch_id && x.customer_code == crmleadownerdto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.description = crmleadownerdto.description;
                result.branch_id = crmleadownerdto.branch_id;
                result.customer_code = crmleadownerdto.customer_code;
                result.modified_at = DateTime.Now;
                db.crmleadowner.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deletecrmleadowner")]
        public IActionResult deletecrmleadowner([FromBody] dynamic model)
        {
            crmleadownerdto crmleadownerdto = JsonConvert.DeserializeObject<crmleadownerdto>(model.ToString());

            var result = db.crmleadowner.Where(x => x.id == crmleadownerdto.id && x.branch_id == crmleadownerdto.branch_id && x.customer_code == crmleadownerdto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.crmleadowner.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmleadownerById")]
        public IActionResult getcrmleadownerById([FromBody] dynamic model)
        {
            crmleadownerdto crmleadownerdto = JsonConvert.DeserializeObject<crmleadownerdto>(model.ToString());

            var result = db.crmleadowner.
                Where(x => x.id == crmleadownerdto.id && x.branch_id == crmleadownerdto.branch_id && x.customer_code == crmleadownerdto.customer_code)
                .Select(x => new crmleadownerdto
                {
                    description = x.description,
                    id = x.id,
                    branch_id = x.branch_id,
                    customer_code = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }
        //dispatch email status
        [HttpPost]
        [Route("api/CRMRx/getcrmdispatchemail")]
        public IActionResult getcrmdispatchemail([FromBody] dynamic model)
        {
            crmsaledispatchemaildto crmsaledispatchemaildto = JsonConvert.DeserializeObject<crmsaledispatchemaildto>(model.ToString());

            var result = db.crmsaledispatchemail.Where(x => x.branch_id == crmsaledispatchemaildto.branch_id && x.customer_code == crmsaledispatchemaildto.customer_code).Select(x => new crmsaledispatchemaildto
            {
                id = x.id,
                description = x.description,
                branch_id = x.branch_id,
                customer_code = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/savecrmdispatchemail")]
        public IActionResult savecrmdispatchemail([FromBody] dynamic model)
        {
            crmsaledispatchemaildto crmsaledispatchemaildto = JsonConvert.DeserializeObject<crmsaledispatchemaildto>(model.ToString());

            if (crmsaledispatchemaildto != null)
            {
                crmsaledispatchemail crmsaledispatchemail = new crmsaledispatchemail();
                crmsaledispatchemail.description = crmsaledispatchemaildto.description;
                crmsaledispatchemail.branch_id = crmsaledispatchemaildto.branch_id;
                crmsaledispatchemail.customer_code = crmsaledispatchemaildto.customer_code;
                crmsaledispatchemail.created_at = DateTime.Now;
                db.crmsaledispatchemail.Add(crmsaledispatchemail);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatcrmdispatchemail")]
        public IActionResult updatcrmdispatchemail([FromBody] dynamic model)
        {
            crmsaledispatchemaildto crmsaledispatchemaildto = JsonConvert.DeserializeObject<crmsaledispatchemaildto>(model.ToString());

            var result = db.crmsaledispatchemail.Where(x => x.id == crmsaledispatchemaildto.id && x.branch_id == crmsaledispatchemaildto.branch_id && x.customer_code == crmsaledispatchemaildto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.description = crmsaledispatchemaildto.description;
                result.branch_id = crmsaledispatchemaildto.branch_id;
                result.customer_code = crmsaledispatchemaildto.customer_code;
                result.modified_at = DateTime.Now;
                db.crmsaledispatchemail.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deletecrmdispatchemail")]
        public IActionResult deletecrmdispatchemail([FromBody] dynamic model)
        {
            crmsaledispatchemaildto crmsaledispatchemaildto = JsonConvert.DeserializeObject<crmsaledispatchemaildto>(model.ToString());

            var result = db.crmsaledispatchemail.Where(x => x.id == crmsaledispatchemaildto.id && x.branch_id == crmsaledispatchemaildto.branch_id && x.customer_code == crmsaledispatchemaildto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.crmsaledispatchemail.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getcrmdispatchemailById")]
        public IActionResult getcrmdispatchemailById([FromBody] dynamic model)
        {
            crmsaledispatchemaildto crmsaledispatchemaildto = JsonConvert.DeserializeObject<crmsaledispatchemaildto>(model.ToString());

            var result = db.crmsaledispatchemail.
                Where(x => x.id == crmsaledispatchemaildto.id && x.branch_id == crmsaledispatchemaildto.branch_id && x.customer_code == crmsaledispatchemaildto.customer_code)
                .Select(x => new crmsaledispatchemaildto
                {
                    description = x.description,
                    id = x.id,
                    branch_id = x.branch_id,
                    customer_code = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }


    }
}
