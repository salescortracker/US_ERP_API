using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;


namespace Usine_Core.Controllers.Inventory
{
   
    public class InvScopeofSupplyController : ControllerBase
    {
        UsineContext db = new UsineContext();
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/SaveScopeofSupply")]
        public IActionResult SaveScopeofSupply([FromBody] dynamic model)
        {

            InvScopeofSupplyDto invScopeofSupplyDto = JsonConvert.DeserializeObject<InvScopeofSupplyDto>(model.ToString());
            if (invScopeofSupplyDto != null)
            {
                InvScopeofSupply invScopeofSupply = new InvScopeofSupply();
                invScopeofSupply.recordId = invScopeofSupplyDto.recordId;
                invScopeofSupply.description = invScopeofSupplyDto.description;
                invScopeofSupply.branchId = invScopeofSupplyDto.branchId;
                invScopeofSupply.customerCode = invScopeofSupplyDto.customerCode;
                invScopeofSupply.created_by = invScopeofSupplyDto.created_by;
                invScopeofSupply.created_date = invScopeofSupplyDto.created_date;
                invScopeofSupply.modified_by = invScopeofSupplyDto.modified_by;
                invScopeofSupply.modified_date = invScopeofSupplyDto.modified_date;


                db.InvScopeofSupply.Add(invScopeofSupply);
                db.SaveChanges();
            }
            return Ok();

        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/UpdateScopeofSupply")]
        public IActionResult UpdateScopeofSupply([FromBody] dynamic model)
        {
            InvScopeofSupplyDto invScopeofSupplyDto = JsonConvert.DeserializeObject<InvScopeofSupplyDto>(model.ToString());
            var result = db.InvScopeofSupply.Where(y => y.recordId == invScopeofSupplyDto.recordId).FirstOrDefault();
            if (result != null)
            {
                result.recordId = invScopeofSupplyDto.recordId;
                result.description = invScopeofSupplyDto.description;
                result.branchId = invScopeofSupplyDto.branchId;
                result.customerCode = invScopeofSupplyDto.customerCode;
                result.created_by = invScopeofSupplyDto.created_by;
                result.created_date = invScopeofSupplyDto.created_date;
                result.modified_by = invScopeofSupplyDto.modified_by;
                result.modified_date = invScopeofSupplyDto.modified_date;


                db.InvScopeofSupply.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetScopeOfSupply")]
        public IActionResult GetScopeOfSupply()
        {
            var scopeofSupply = db.InvScopeofSupply.Select(s => new InvScopeofSupply
            {
                recordId = s.recordId,
                description = s.description,
                branchId = s.branchId,
                customerCode = s.customerCode,
                created_by = s.created_by,
                created_date = s.created_date,
                modified_by = s.modified_by,
                modified_date = s.modified_date,

                //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

            }).ToList();

            return Ok(scopeofSupply.ToList());

        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetScopeofSupplyById")]
        public IActionResult GetScopeofSupplyById([FromBody] dynamic model)
        {
            InvScopeofSupplyDto invScopeofSupplyDto = JsonConvert.DeserializeObject<InvScopeofSupplyDto>(model.ToString());

            var scopelist = db.InvScopeofSupply.Where(y => y.recordId == invScopeofSupplyDto.recordId).Select(list => new InvScopeofSupplyDto
            {
                recordId = list.recordId,
                description = list.description,
                branchId = list.branchId,
                customerCode = list.customerCode,
                created_by = list.created_by,
                created_date = list.created_date,
                modified_by = list.modified_by,
                modified_date = list.modified_date,

            }).FirstOrDefault();



            return Ok(scopelist);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetScopeofSupplyDeleteById")]
        public IActionResult GetScopeofSupplyDelete([FromBody] int id)
        {

            var scopeofSupply = db.InvScopeofSupply.FirstOrDefault(y => y.recordId == id);

            if (scopeofSupply == null)
            {

                return NotFound(new { message = "scope of supply not found." });
            }


            db.InvScopeofSupply.Remove(scopeofSupply);

            db.SaveChanges();

            return Ok(new { message = " scope of supply deleted successfully." });
        }
    }
}
