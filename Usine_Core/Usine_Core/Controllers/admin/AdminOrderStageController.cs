using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.admin
{
    public class AdminOrderStageController : ControllerBase
    {
        UsineContext db = new UsineContext();
        //order stage
        [HttpPost]
        [Route("api/CRMRx/getadmorderstage")]
        public IActionResult getadmorderstage([FromBody] dynamic model)
        {
            orderstagedto orderstagedto = JsonConvert.DeserializeObject<orderstagedto>(model.ToString());

            var result = db.Crmorderstages.Where(x => x.branch_id == orderstagedto.branch_id && x.customer_code == orderstagedto.customer_code).Select(x => new orderstagedto
            {
                id = x.id,
                description = x.description,
                branch_id = x.branch_id,
                customer_code = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/saveorderstage")]
        public IActionResult saveorderstage([FromBody] dynamic model)
        {
            orderstagedto orderstagedto = JsonConvert.DeserializeObject<orderstagedto>(model.ToString());

            if (orderstagedto != null)
            {
                crmorderstage crmorderstage = new crmorderstage();
                crmorderstage.description = orderstagedto.description;
                crmorderstage.branch_id = orderstagedto.branch_id;
                crmorderstage.customer_code = orderstagedto.customer_code;
                crmorderstage.created_at = DateTime.Now;
                db.Crmorderstages.Add(crmorderstage);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updateorderstage")]
        public IActionResult updateorderstage([FromBody] dynamic model)
        {
            orderstagedto orderstagedto = JsonConvert.DeserializeObject<orderstagedto>(model.ToString());

            var result = db.Crmorderstages.Where(x => x.id == orderstagedto.id && x.branch_id == orderstagedto.branch_id && x.customer_code == orderstagedto.customer_code).FirstOrDefault();
            if (result != null)
            {
                result.description = orderstagedto.description;
                result.branch_id = orderstagedto.branch_id;
                result.customer_code = orderstagedto.customer_code;
                result.modified_at = DateTime.Now;
                db.Crmorderstages.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deleteorderstage")]
        public IActionResult deleteorderstage([FromBody] dynamic model)
        {
            orderstagedto orderstagedto = JsonConvert.DeserializeObject<orderstagedto>(model.ToString());

            var result = db.Crmorderstages.Where(x => x.id == orderstagedto.id && x.branch_id == orderstagedto.branch_id && x.customer_code == orderstagedto.customer_code).FirstOrDefault();
            if (result != null)
            {
                db.Crmorderstages.Remove(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/getorderstageById")]
        public IActionResult getorderstageById([FromBody] dynamic model)
        {
            orderstagedto orderstagedto = JsonConvert.DeserializeObject<orderstagedto>(model.ToString());

            var result = db.Crmorderstages.Where(x => x.id == orderstagedto.id && x.branch_id == orderstagedto.branch_id && x.customer_code == orderstagedto.customer_code)
                .Select(x => new orderstagedto
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
