using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.crm
{
   
    public class CRMProductMasterController : ControllerBase
    {

        UsineContext db = new UsineContext();
        [HttpPost]
        [Authorize]
        [Route("api/CRMProductMaster/Savecrmprogroup")]
        public IActionResult Savecrmprogroup([FromBody] dynamic model)
        {
            crmprogroupdto crmprogroupdto=JsonConvert.DeserializeObject<crmprogroupdto>(model.ToString());
            if(crmprogroupdto == null)
            {
                crmprogroup crmprogroup=new crmprogroup();
                crmprogroup.groupName=crmprogroupdto.groupName;
                crmprogroup.BranchId = crmprogroupdto.BranchId;
                crmprogroup.CustomerCode = crmprogroupdto.CustomerCode;
                db.crmprogroup.Add(crmprogroup);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("api/CRMProductMaster/Getcrmprogroup")]
        public IActionResult Getcrmprogroup()
        {
            return Ok(db.crmprogroup.ToList());
           
        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMProductMaster/Savecrmprocategory")]
        public IActionResult Savecrmprocategory([FromBody] dynamic model)
        {
            crmprocategorydto crmprocategorydto = JsonConvert.DeserializeObject<crmprocategorydto>(model.ToString());
            if (crmprocategorydto == null)
            {
                crmprocategory crmprocategory = new crmprocategory();
                crmprocategory.categoryName = crmprocategorydto.categoryName;
                crmprocategory.crmprogroupId = crmprocategorydto.crmprogroupId;
                crmprocategory.BranchId = crmprocategorydto.BranchId;
                crmprocategory.CustomerCode = crmprocategorydto.CustomerCode;
                db.crmprocategory.Add(crmprocategory);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("api/CRMProductMaster/Getcrmprocategory")]
        public IActionResult Getcrmprocategory()
        {
            return Ok(db.crmprocategory.ToList());

        }
        [HttpPost]
        [Authorize]
        [Route("api/CRMProductMaster/SavecrmproProductMaster")]
        public IActionResult SavecrmproProductMaster([FromBody] dynamic model)
        {
            crmproductmasterdto crmproductmasterdto = JsonConvert.DeserializeObject<crmproductmasterdto>(model.ToString());
            if (crmproductmasterdto == null)
            {
                crmproductmaster crmproductmaster = new crmproductmaster();
                crmproductmaster.crmprogroupId = crmproductmasterdto.crmprogroupId;
                crmproductmaster.crmprocategoryId = crmproductmasterdto.crmprocategoryId;
                crmproductmaster.serviceName = crmproductmasterdto.serviceName;
                crmproductmaster.productCode = crmproductmasterdto.productCode;
                crmproductmaster.productDesc = crmproductmasterdto.productDesc;
                crmproductmaster.uom = crmproductmasterdto.uom;
                crmproductmaster.unit = crmproductmasterdto.unit;
                crmproductmaster.unitPrice = crmproductmasterdto.unitPrice;
                crmproductmaster.dealerPrice = crmproductmasterdto.dealerPrice;
                crmproductmaster.wholesalePrice = crmproductmasterdto.wholesalePrice;
                crmproductmaster.BranchId = crmproductmasterdto.BranchId;
                crmproductmaster.CustomerCode = crmproductmasterdto.CustomerCode;
                db.crmproductmaster.Add(crmproductmaster);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("api/CRMProductMaster/GetcrmproProductMaster")]
        public IActionResult GetcrmproProductMaster()
        {
            return Ok(db.crmproductmaster.ToList());

        }
    }
}
