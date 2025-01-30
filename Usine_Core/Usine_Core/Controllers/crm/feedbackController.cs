using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Usine_Core.Models;
using Usine_Core.others.dtos;
namespace Usine_Core.Controllers.crm
{
  
    public class feedbackController : ControllerBase
    {
        UsineContext db = new UsineContext();



        [HttpPost]
        [Authorize]
        [Route("api/feedback/SaveFeedbackForm")]
        public IActionResult SaveFeedbackForm([FromBody] dynamic model)
        {

            Feedbackformdto feedbackformdto = JsonConvert.DeserializeObject<Feedbackformdto>(model.ToString());

            if (feedbackformdto != null)
            {
                Feedbackform feedbackForm = new Feedbackform
                {
                    RecordId = feedbackformdto.RecordId,
                    From = feedbackformdto.From,
                    Details = feedbackformdto.Details,
                    Date = feedbackformdto.Date,
                    QualityOfProduct = feedbackformdto.QualityOfProduct,
                    DeliverySchedule = feedbackformdto.DeliverySchedule,
                    Communication = feedbackformdto.Communication,
                    RequestElaborateAreas = feedbackformdto.RequestElaborateAreas,
                    Signature = feedbackformdto.Signature,
                    Designation = feedbackformdto.Designation,
                    AverageScore = feedbackformdto.AverageScore,
                    SignatureOfMarketingManager = feedbackformdto.SignatureOfMarketingManager,
                    BranchId = feedbackformdto.BranchId,
                    CustomerCode = feedbackformdto.CustomerCode
                };

                db.Feedbackform.Add(feedbackForm);
                db.SaveChanges();

            };




            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("api/feedback/GetAllFeedbackForms")]
        public IActionResult GetAllFeedbackForms()
        {

            try
            {
                var result = db.Feedbackform.Select(x => new Feedbackformdto
                {
                    From = x.From,
                    Details = x.Details,
                    Date = x.Date,
                    QualityOfProduct = x.QualityOfProduct,
                    DeliverySchedule = x.DeliverySchedule,
                    Communication = x.Communication,
                    //RequestElaborateAreas = x.RequestElaborateAreas,
                    //Signature = x.Signature,
                    //Designation = x.Designation,
                    AverageScore = x.AverageScore,
                    //SignatureOfMarketingManager = x.SignatureOfMarketingManager,
                    RecordId = x.RecordId,
                    //CustomerCode = x.CustomerCode,

                }).ToList();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/feedbackform/Updatefeedbackform")]
        public IActionResult Updatefeedbackform([FromBody] dynamic model)
        {
            Feedbackformdto feedbackformdto = JsonConvert.DeserializeObject<Feedbackformdto>(model.ToString());
            var result = db.Feedbackform.Where(y => y.RecordId == feedbackformdto.RecordId).FirstOrDefault();
            if (result != null)
            {
                result.RecordId = feedbackformdto.RecordId;
                result.From = feedbackformdto.From;
                result.Details = feedbackformdto.Details;
                result.QualityOfProduct = feedbackformdto.QualityOfProduct;
                result.DeliverySchedule = feedbackformdto.DeliverySchedule;
                result.Communication = feedbackformdto.Communication;
                result.RequestElaborateAreas = feedbackformdto.RequestElaborateAreas;
                result.Signature = feedbackformdto.Signature;
                result.Designation = feedbackformdto.Designation;
                result.AverageScore = feedbackformdto.AverageScore;
                result.SignatureOfMarketingManager = feedbackformdto.SignatureOfMarketingManager;
                result.BranchId = feedbackformdto.BranchId;
                result.CustomerCode = feedbackformdto.CustomerCode;

                db.Feedbackform.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("api/feedbackform/GetfeedbackformById")]
        public IActionResult GetfeedbackformById([FromBody] dynamic model)
        {
            Feedbackformdto feedbackformdto = JsonConvert.DeserializeObject<Feedbackformdto>(model.ToString());

            var statuslist = db.Feedbackform.Where(y => y.RecordId == feedbackformdto.RecordId).Select(status => new Feedbackformdto
            {
                RecordId = status.RecordId,
                From = status.From,
                Details = status.Details,
                QualityOfProduct = status.QualityOfProduct,
                DeliverySchedule = status.DeliverySchedule,
                Communication = status.Communication,
                RequestElaborateAreas = status.RequestElaborateAreas,
                Signature = status.Signature,
                Designation = status.Designation,
                AverageScore = status.AverageScore,
                SignatureOfMarketingManager = status.SignatureOfMarketingManager,
                BranchId = status.BranchId,
                CustomerCode = status.CustomerCode,
            }).FirstOrDefault();
            return Ok(statuslist);
        }


        [HttpPost]
        [Authorize]
        [Route("api/feedbackform/GetfeedbackformDeleteById")]
        public IActionResult GetfeedbackformDeleteById([FromBody] int id)
        {

            var feedbackform = db.Feedbackform.FirstOrDefault(y => y.RecordId == id);

            if (feedbackform == null)
            {

                return NotFound(new { message = "status not found." });
            }


            db.Feedbackform.Remove(feedbackform);

            db.SaveChanges();

            return Ok(new { message = "status deleted successfully." });
        }
    }
}
