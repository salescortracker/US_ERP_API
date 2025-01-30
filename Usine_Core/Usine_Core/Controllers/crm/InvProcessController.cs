using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.crm
{
    public class InvProcessController : ControllerBase
    {
        UsineContext db = new UsineContext();
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/SaveProcess")]
        public IActionResult SaveProcess([FromBody] dynamic model)
        {

            InvProcessDto invProcessDto = JsonConvert.DeserializeObject<InvProcessDto>(model.ToString());
            if (invProcessDto != null)
            {
                InvProcess invProcess = new InvProcess();
                invProcess.recordId = invProcessDto.recordId;
                invProcess.processName = invProcessDto.processName;
                invProcess.branchId = invProcessDto.branchId;
                invProcess.customerCode = invProcessDto.customerCode;
                invProcess.created_by = invProcessDto.created_by;
                invProcess.created_date = invProcessDto.created_date;
                invProcess.modified_by = invProcessDto.modified_by;
                invProcess.modified_date = invProcessDto.modified_date;


                db.InvProcess.Add(invProcess);
                db.SaveChanges();
            }
            return Ok();

        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/UpdateProcess")]
        public IActionResult UpdateProcess([FromBody] dynamic model)
        {
            InvProcessDto invProcessDto = JsonConvert.DeserializeObject<InvProcessDto>(model.ToString());
            var result = db.InvProcess.Where(y => y.recordId == invProcessDto.recordId).FirstOrDefault();
            if (result != null)
            {
                result.recordId = invProcessDto.recordId;
                result.processName = invProcessDto.processName;
                result.branchId = invProcessDto.branchId;
                result.customerCode = invProcessDto.customerCode;
                result.created_by = invProcessDto.created_by;
                result.created_date = invProcessDto.created_date;
                result.modified_by = invProcessDto.modified_by;
                result.modified_date = invProcessDto.modified_date;


                db.InvProcess.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetProcess")]
        public IActionResult GetProcess()
        {
            var process = db.InvProcess.Select(p => new InvProcessDto
            {
                recordId = p.recordId,
                processName = p.processName,
                branchId = p.branchId,
                customerCode = p.customerCode,
                created_by = p.created_by,
                created_date = p.created_date,
                modified_by = p.modified_by,
                modified_date = p.modified_date,

                //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

            }).ToList();

            return Ok(process.ToList());

        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetProcessById")]
        public IActionResult GetProcessById([FromBody] dynamic model)
        {
            InvProcessDto invProcessDto = JsonConvert.DeserializeObject<InvProcessDto>(model.ToString());

            var processlist = db.InvProcess.Where(y => y.recordId == invProcessDto.recordId).Select(list => new InvProcessDto
            {
                recordId = list.recordId,
                processName = list.processName,
                branchId = list.branchId,
                customerCode = list.customerCode,
                created_by = list.created_by,
                created_date = list.created_date,
                modified_by = list.modified_by,
                modified_date = list.modified_date,

            }).FirstOrDefault();



            return Ok(processlist);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetProcessDeleteById")]
        public IActionResult GetProcessDelete([FromBody] int id)
        {

            var process = db.InvProcess.FirstOrDefault(y => y.recordId == id);

            if (process == null)
            {

                return NotFound(new { message = "processname not found." });
            }


            db.InvProcess.Remove(process);

            db.SaveChanges();

            return Ok(new { message = " process deleted successfully." });
        }
    }
}
