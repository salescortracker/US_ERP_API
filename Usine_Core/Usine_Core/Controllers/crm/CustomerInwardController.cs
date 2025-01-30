using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.crm
{
 
    public class CustomerInwardController : ControllerBase
    {
        UsineContext db = new UsineContext();
        [HttpPost]
        [Authorize]
        [Route("api/CustomerInward/SaveInward")]
        public IActionResult SaveInward([FromBody] dynamic model)
        {

            CustomerInwardDto customerInwardDto = JsonConvert.DeserializeObject<CustomerInwardDto>(model.ToString());
            if (customerInwardDto != null)
            {
                CustomerInward customerInward = new CustomerInward();
                customerInward.RecordId = customerInwardDto.RecordId;
                customerInward.InwardNo = customerInwardDto.InwardNo;
                customerInward.GpNo = customerInwardDto.GpNo;
                customerInward.NameOfCompany = customerInwardDto.NameOfCompany;
                customerInward.ItemDescription = customerInwardDto.ItemDescription;
                customerInward.Size = customerInwardDto.Size;
                customerInward.Process = customerInwardDto.Process;
                customerInward.RecordedQuantity = customerInwardDto.RecordedQuantity;
                customerInward.DispatchQuantity = customerInwardDto.DispatchQuantity;
                customerInward.DispatchRegisterNo = customerInwardDto.DispatchRegisterNo;
                customerInward.DateOfDelivery = customerInwardDto.DateOfDelivery;
                customerInward.BalanceQuantity = customerInwardDto.BalanceQuantity;
                customerInward.Signature = customerInwardDto.Signature;
                customerInward.BranchId = customerInwardDto.BranchId;
                customerInward.CustomerCode = customerInwardDto.CustomerCode;
                customerInward.CreatedBy = customerInwardDto.CreatedBy;
                customerInward.CreatedAt = customerInwardDto.CreatedAt;
                customerInward.ModifiedAt = customerInwardDto.ModifiedAt;
                customerInward.ModifiedBy = customerInwardDto.ModifiedBy;


                db.CustomerInward.Add(customerInward);
                db.SaveChanges();
            }
            return Ok();

        }

        [HttpPost]
        [Authorize]
        [Route("api/CustomerInward/UpdateInward")]
        public IActionResult UpdateInward([FromBody] dynamic model)
        {
            CustomerInwardDto customerInwardDto = JsonConvert.DeserializeObject<CustomerInwardDto>(model.ToString());
            var result = db.CustomerInward.Where(y => y.RecordId == customerInwardDto.RecordId).FirstOrDefault();
            if (result != null)
            {
                result.RecordId = customerInwardDto.RecordId;
                result.InwardNo = customerInwardDto.InwardNo;
                result.GpNo = customerInwardDto.GpNo;
                result.NameOfCompany = customerInwardDto.NameOfCompany;
                result.ItemDescription = customerInwardDto.ItemDescription;
                result.Size = customerInwardDto.Size;
                result.Process = customerInwardDto.Process;
                result.RecordedQuantity = customerInwardDto.RecordedQuantity;
                result.DispatchQuantity = customerInwardDto.DispatchQuantity;
                result.DispatchRegisterNo = customerInwardDto.DispatchRegisterNo;
                result.DateOfDelivery = customerInwardDto.DateOfDelivery;
                result.BalanceQuantity = customerInwardDto.BalanceQuantity;
                result.Signature = customerInwardDto.Signature;
                result.BranchId = customerInwardDto.BranchId;
                result.CustomerCode = customerInwardDto.CustomerCode;


                db.CustomerInward.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("api/CustomerInward/GetInward")]
        public IActionResult GetInward()
        {
            var Materials = db.CustomerInward.Select(m => new CustomerInwardDto
            {
                RecordId = m.RecordId,
                InwardNo = m.InwardNo,
                GpNo = m.GpNo,
                NameOfCompany = m.NameOfCompany,
                ItemDescription = m.ItemDescription,
                Size = m.Size,
                Process = m.Process,
                processName = m.Process != null ? db.InvProcess.Where(p => p.recordId == m.Process).FirstOrDefault().processName : null,
                RecordedQuantity = m.RecordedQuantity,
                DispatchQuantity = m.DispatchQuantity,
                DispatchRegisterNo = m.DispatchRegisterNo,
                DateOfDelivery = m.DateOfDelivery,
                BalanceQuantity = m.BalanceQuantity,
                BranchId = m.BranchId,
                CustomerCode = m.CustomerCode,


                //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

            }).ToList();

            return Ok(Materials.ToList());

        }

        [HttpPost]
        [Authorize]
        [Route("api/CustomerInward/GetInwardById")]
        public IActionResult GetInwardById([FromBody] dynamic model)
        {
            CustomerInwardDto customerInwardDto = JsonConvert.DeserializeObject<CustomerInwardDto>(model.ToString());

            var materiallist = db.CustomerInward.Where(y => y.RecordId == customerInwardDto.RecordId).Select(material => new CustomerInwardDto
            {
                RecordId = material.RecordId,
                InwardNo = material.InwardNo,
                GpNo = material.GpNo,
                NameOfCompany = material.NameOfCompany,
                ItemDescription = material.ItemDescription,
                Size = material.Size,
                Process = material.Process,
                RecordedQuantity = material.RecordedQuantity,
                DispatchQuantity = material.DispatchQuantity,
                DispatchRegisterNo = material.DispatchRegisterNo,
                DateOfDelivery = material.DateOfDelivery,
                BalanceQuantity = material.BalanceQuantity,
                BranchId = material.BranchId,
                CustomerCode = material.CustomerCode,
                Signature = material.Signature,



            }).FirstOrDefault();



            return Ok(materiallist);
        }

        [HttpPost]
        [Authorize]
        [Route("api/CustomerInward/GetInwardDeleteById")]
        public IActionResult GetInwardDelete([FromBody] int id)
        {

            var material = db.CustomerInward.FirstOrDefault(y => y.RecordId == id);

            if (material == null)
            {

                return NotFound(new { message = "chemic not found." });
            }


            db.CustomerInward.Remove(material);

            db.SaveChanges();

            return Ok(new { message = " chemical deleted successfully." });
        }

    }
}
