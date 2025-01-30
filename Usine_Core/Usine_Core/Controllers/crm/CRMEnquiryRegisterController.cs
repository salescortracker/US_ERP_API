using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.crm
{
   
    public class CRMEnquiryRegisterController : ControllerBase
    {
        UsineContext db = new UsineContext();
        [HttpPost]
        [Authorize]
        [Route("api/CRMEnquiryRegister/Savecrmenquiryregister")]
        public IActionResult Savecrmenquiryregister([FromBody] dynamic model)
        {
            CrmEnquiryRegisterDto crmEnquiryRegisterDto = JsonConvert.DeserializeObject<CrmEnquiryRegisterDto>(model.ToString());
            if (crmEnquiryRegisterDto != null)
            {
                CrmEnquiryRegister crmEnquiryRegister = new CrmEnquiryRegister();
                crmEnquiryRegister.RecordId = crmEnquiryRegisterDto.RecordId;
                crmEnquiryRegister.Date = crmEnquiryRegisterDto.Date;
                crmEnquiryRegister.EnquiryFrom = crmEnquiryRegisterDto.EnquiryFrom;
                crmEnquiryRegister.EnquiryDetails = crmEnquiryRegisterDto.EnquiryDetails;
                crmEnquiryRegister.ProductRange = crmEnquiryRegisterDto.ProductRange;
                crmEnquiryRegister.Process = crmEnquiryRegisterDto.Process;
                crmEnquiryRegister.QuotationSubmissionDetails = crmEnquiryRegisterDto.QuotationSubmissionDetails;
                crmEnquiryRegister.NegotiationDetails = crmEnquiryRegisterDto.NegotiationDetails;
                crmEnquiryRegister.OrderAcceptanceDetails = crmEnquiryRegisterDto.OrderAcceptanceDetails;
                crmEnquiryRegister.ExpectedDateOfDelivery = crmEnquiryRegisterDto.ExpectedDateOfDelivery;
                crmEnquiryRegister.ActualDateOfDelivery = crmEnquiryRegisterDto.ActualDateOfDelivery;
                crmEnquiryRegister.Remarks = crmEnquiryRegisterDto.Remarks;
                crmEnquiryRegister.PreparedBy = crmEnquiryRegisterDto.PreparedBy;
                crmEnquiryRegister.ApprovedBy = crmEnquiryRegisterDto.ApprovedBy;
                crmEnquiryRegister.BranchId = crmEnquiryRegisterDto.BranchId;
                crmEnquiryRegister.CustomerCode = crmEnquiryRegisterDto.CustomerCode;

                db.CrmEnquiryRegister.Add(crmEnquiryRegister);
                db.SaveChanges();
            }
            return Ok();

        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMEnquiryRegister/Updatecrmenquiryregister")]
        public IActionResult Updatecrmenquiryregister([FromBody] dynamic model)
        {
            CrmEnquiryRegisterDto crmEnquiryRegisterDto = JsonConvert.DeserializeObject<CrmEnquiryRegisterDto>(model.ToString());
            var result = db.CrmEnquiryRegister.Where(y => y.RecordId == crmEnquiryRegisterDto.RecordId).FirstOrDefault();
            if (result != null)
            {
                result.RecordId = crmEnquiryRegisterDto.RecordId;
                result.Date = crmEnquiryRegisterDto.Date;
                result.EnquiryFrom = crmEnquiryRegisterDto.EnquiryFrom;
                result.EnquiryDetails = crmEnquiryRegisterDto.EnquiryDetails;
                result.ProductRange = crmEnquiryRegisterDto.ProductRange;
                result.Process = crmEnquiryRegisterDto.Process;
                result.QuotationSubmissionDetails = crmEnquiryRegisterDto.QuotationSubmissionDetails;
                result.NegotiationDetails = crmEnquiryRegisterDto.NegotiationDetails;
                result.OrderAcceptanceDetails = crmEnquiryRegisterDto.OrderAcceptanceDetails;
                result.ExpectedDateOfDelivery = crmEnquiryRegisterDto.ExpectedDateOfDelivery;
                result.ActualDateOfDelivery = crmEnquiryRegisterDto.ActualDateOfDelivery;
                result.Remarks = crmEnquiryRegisterDto.Remarks;
                result.PreparedBy = crmEnquiryRegisterDto.PreparedBy;
                result.ApprovedBy = crmEnquiryRegisterDto.ApprovedBy;



                db.CrmEnquiryRegister.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("api/CRMEnquiryRegister/Getcrmenquiryregister")]
        public IActionResult Getcrmenquiryregister()
        {
            var enquiryregister = db.CrmEnquiryRegister.Select(s => new CrmEnquiryRegisterDto
            {
                RecordId = s.RecordId,
                Date = s.Date,
                EnquiryFrom = s.EnquiryFrom,
                EnquiryDetails = s.EnquiryDetails,
                ProductRange = s.ProductRange,
                Process = s.Process,
                //LeadOwnerName = l.LeadOwner != null ? db.HrdEmployees.Where(e => e.RecordId == l.LeadOwner).FirstOrDefault().Empname : null,
               // processName = s.Process != null ? db.InvProcess.Where(p => p.recordId == s.Process).FirstOrDefault().processName : null,


                QuotationSubmissionDetails = s.QuotationSubmissionDetails,
                NegotiationDetails = s.NegotiationDetails,
                OrderAcceptanceDetails = s.OrderAcceptanceDetails,
                ExpectedDateOfDelivery = s.ExpectedDateOfDelivery,
                ActualDateOfDelivery = s.ActualDateOfDelivery,
                Remarks = s.Remarks,
                ApprovedBy = s.ApprovedBy,
                PreparedBy = s.PreparedBy,

                //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

            }).ToList();

            return Ok(enquiryregister);

        }

        [HttpPost]
        [Authorize]
        [Route("api/CRMEnquiryRegister/GetcrmenquiryregisterById")]
        public IActionResult GetcrmenquiryregisterById([FromBody] dynamic model)
        {
            CrmEnquiryRegisterDto crmEnquiryRegisterDto = JsonConvert.DeserializeObject<CrmEnquiryRegisterDto>(model.ToString());

            var registerlist = db.CrmEnquiryRegister.Where(y => y.RecordId == crmEnquiryRegisterDto.RecordId).Select(registerlist => new CrmEnquiryRegisterDto
            {
                RecordId = registerlist.RecordId,
                Date = registerlist.Date,
                EnquiryFrom = registerlist.EnquiryFrom,
                EnquiryDetails = registerlist.EnquiryDetails,
                ProductRange = registerlist.ProductRange,
                Process = registerlist.Process,
                QuotationSubmissionDetails = registerlist.QuotationSubmissionDetails,
                NegotiationDetails = registerlist.NegotiationDetails,
                OrderAcceptanceDetails = registerlist.OrderAcceptanceDetails,
                ExpectedDateOfDelivery = registerlist.ExpectedDateOfDelivery,
                ActualDateOfDelivery = registerlist.ActualDateOfDelivery,
                Remarks = registerlist.Remarks,
                PreparedBy = registerlist.PreparedBy,
                ApprovedBy = registerlist.ApprovedBy
            }).FirstOrDefault();

            if (registerlist == null)
            {
                return NotFound("Record not found.");
            }

            return Ok(registerlist);
        }


        //[HttpPost]
        //[Authorize]
        //[Route("api/CRMEnquiryRegister/GetcrmenquiryregisterById")]
        //public IActionResult GetcrmenquiryregisterById([FromBody] dynamic model)
        //{
        //    CrmEnquiryRegisterDto crmEnquiryRegisterDto = JsonConvert.DeserializeObject<CrmEnquiryRegisterDto>(model.ToString());

        //    var registerlist = db.CrmEnquiryRegister.Where(y => y.RecordId == crmEnquiryRegisterDto.RecordId).Select(registerlist => new CrmEnquiryRegisterDto
        //    {
        //      RecordId=registerlist.RecordId,
        //      Date=registerlist.Date,
        //      EnquiryFrom=registerlist.EnquiryFrom,
        //      EnquiryDetails=registerlist.EnquiryDetails,
        //      ProductRange=registerlist.ProductRange,
        //      Process=registerlist.Process,
        //      QuotationSubmissionDetails=registerlist.QuotationSubmissionDetails,
        //      NegotiationDetails=registerlist.NegotiationDetails,
        //      OrderAcceptanceDetails=registerlist.OrderAcceptanceDetails,
        //      ExpectedDateOfDelivery=registerlist.ExpectedDateOfDelivery,
        //      ActualDateOfDelivery=registerlist.ActualDateOfDelivery,
        //      Remarks=registerlist.Remarks,
        //      PreparedBy=registerlist.PreparedBy,
        //      ApprovedBy=registerlist.ApprovedBy,



        //    }).FirstOrDefault();
        //    return Ok(registerlist);
        //}

        [HttpPost]
        [Authorize]
        [Route("api/CRMEnquiryRegister/GetcrmenquiryregisterDeleteById")]
        public IActionResult GetcrmenquiryregisterDelete([FromBody] int id)
        {

            var enquiryregister = db.CrmEnquiryRegister.FirstOrDefault(y => y.RecordId == id);

            if (enquiryregister == null)
            {

                return NotFound(new { message = "enquiry register not found." });
            }


            db.CrmEnquiryRegister.Remove(enquiryregister);

            db.SaveChanges();

            return Ok(new { message = "enquiry register deleted successfully." });
        }
    }
}
