using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.Inventory
{

    public class InvLabController : ControllerBase
    {
        UsineContext db = new UsineContext();
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/SaveLabScreen")]
        public IActionResult SaveLabScreen([FromBody] dynamic model)
        {

            InvLabDto invLabDto = JsonConvert.DeserializeObject<InvLabDto>(model.ToString());
            if (invLabDto != null)
            {
                InvLab invLab = new InvLab();

                invLab.LabCode = invLabDto.LabCode;
                invLab.LabName = invLabDto.LabName;
                invLab.ChemicalName = invLabDto.ChemicalName;
                invLab.Description = invLabDto.Description;
                invLab.LabIncharge = invLabDto.LabIncharge;
                invLab.Customer = invLabDto.Customer;
                invLab.Status = invLabDto.Status;
                invLab.BranchId = invLabDto.BranchId;
                invLab.CustomerCode = invLabDto.CustomerCode;
                invLab.CreatedBy = invLabDto.CreatedBy;
                invLab.CreatedAt = invLabDto.CreatedAt;
                invLab.ModifiedAt = invLabDto.ModifiedAt;
                invLab.ModifiedBy = invLabDto.ModifiedBy;


                db.InvLab.Add(invLab);
                db.SaveChanges();
            }
            return Ok();

        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/UpdateLabScreen")]
        public IActionResult UpdateLabScreen([FromBody] dynamic model)
        {
            InvLabDto invLabDto = JsonConvert.DeserializeObject<InvLabDto>(model.ToString());
            var result = db.InvLab.Where(y => y.RecordID == invLabDto.RecordID).FirstOrDefault();
            if (result != null)
            {
                result.RecordID = invLabDto.RecordID;
                result.LabCode = invLabDto.LabCode;
                result.LabName = invLabDto.LabName;
                result.ChemicalName = invLabDto.ChemicalName;
                result.Description = invLabDto.Description;
                result.LabIncharge = invLabDto.LabIncharge;
                result.Customer = invLabDto.Customer;
                result.Status = invLabDto.Status;
                result.BranchId = invLabDto.BranchId;
                result.CustomerCode = invLabDto.CustomerCode;
                result.CreatedAt = invLabDto.CreatedAt;
                result.CreatedBy = invLabDto.CreatedBy;
                result.ModifiedBy = invLabDto.ModifiedBy;
                result.ModifiedAt = invLabDto.ModifiedAt;
                db.InvLab.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Getlabscreen")]
        public IActionResult Getlabscreen()
        {
            var labscreen = db.InvLab.Select(l => new InvLabDto
            {
                RecordID = l.RecordID,
                LabCode = l.LabCode,
                LabName = l.LabName,
                ChemicalName = l.ChemicalName,
                LabIncharge = l.LabIncharge,
                Customer = l.Customer,
                Description = l.Description,
                Status = l.Status,
                // StatusName = l.Status != null ? db.InvLab.Where(g => g.RecordID == l.Status).FirstOrDefault().Description : null,
                BranchId = l.BranchId,
                CustomerCode = l.CustomerCode,
              
                ModifiedBy = l.ModifiedBy,
                ModifiedAt = l.ModifiedAt,



                //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

            }).ToList();

            return Ok(labscreen);

        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetlabscreenById")]
        public IActionResult GetlabscreenById([FromBody] dynamic model)
        {
            InvLabDto invLabDto = JsonConvert.DeserializeObject<InvLabDto>(model.ToString());

            var labscreenlist = db.InvLab.Where(y => y.RecordID == invLabDto.RecordID).Select(labscreen => new InvLabDto
            {
                RecordID = labscreen.RecordID,
                LabName = labscreen.LabName,
                LabCode = labscreen.LabCode,
                LabIncharge = labscreen.LabIncharge,
                ChemicalName = labscreen.ChemicalName,
                Customer = labscreen.Customer,
                Description = labscreen.Description,
                Status = labscreen.Status,

                //StatusName = labscreenlist.Status != null ? db.InvLab.Where(g => g.RecordID == labscreenlist.Status).FirstOrDefault().Description : null,
                BranchId = labscreen.BranchId,
                CustomerCode = labscreen.CustomerCode,
                CreatedBy = labscreen.CreatedBy,
                CreatedAt = labscreen.CreatedAt,
                ModifiedAt = labscreen.ModifiedAt,
                ModifiedBy = labscreen.ModifiedBy,
            }).FirstOrDefault();



            return Ok(labscreenlist);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Inventory/GetlabscreenDeleteById")]
        public IActionResult GetlabscreenDelete([FromBody] int id)
        {

            var labscreen = db.InvLab.FirstOrDefault(y => y.RecordID == id);

            if (labscreen == null)
            {

                return NotFound(new { message = "chemic not found." });
            }


            db.InvLab.Remove(labscreen);

            db.SaveChanges();

            return Ok(new { message = " chemical deleted successfully." });
        }

    }
}
