using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Usine_Core.Controllers.Admin;
using Usine_Core.Models;
using Usine_Core.others;
using Usine_Core.others.dtos;

namespace Usine_Core.Controllers.crm
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMLeadController : ControllerBase
    {
        UsineContext db = new UsineContext();
        public class AssignClass
        {
            public List<int> Leads { get; set; }
            public int CustomerCode { get; set; }
            public int AssignTo { get; set; }
        }
        //[HttpGet]
        //[Authorize]
        //[Route("GetAllLeads")]
        //public IActionResult GetAllLeads()
        //{
        //    var leads = db.LeadManagements.Select(l => new LeadManagementDto
        //    {
        //        Id = l.Id,
        //        Code = l.Code,
        //        Customer = l.Customer,
        //        BranchId = l.BranchId,
        //        CustomerCode = l.CustomerCode,
        //        LeadGroup = l.LeadGroup,
        //        Status = l.Status,
        //        LeadOwner = l.LeadOwner,
        //        Company = l.Company,
        //        FirstName = l.FirstName,
        //        LastName = l.LastName,
        //        Description = l.Description,
        //        BusinessEmail = l.BusinessEmail,
        //        SecondaryEmail = l.SecondaryEmail,
        //        PhoneNumber = l.PhoneNumber,
        //        AlternateNumber = l.AlternateNumber,
        //        LeadStatus = l.LeadStatus,
        //        LeadSource = l.LeadSource,
        //        LeadStage = l.LeadStage,
        //        Website = l.Website,
        //        Industry = l.Industry,
        //        NumberOfEmployees = l.NumberOfEmployees,
        //        AnnualRevenue = l.AnnualRevenue,
        //        Rating = l.Rating,
        //        EmailOutputFormat = l.EmailOutputFormat,
        //        SkypeId = l.SkypeId
        //    }).ToList();

        //    return Ok(leads);
        //}

        //[HttpPost]
        //[Authorize]
        //[Route("SaveLead")]
        //public IActionResult SaveLead([FromBody] LeadManagementDto leadDto)
        //{
        //    if (leadDto == null)
        //    {
        //        return BadRequest("Invalid lead data.");
        //    }

        //    var lead = new crmleads
        //    {
        //        Id = leadDto.Id,
        //        Code = leadDto.Code,
        //        Customer = leadDto.Customer,
        //        BranchId = leadDto.BranchId,
        //        CustomerCode = leadDto.CustomerCode,
        //        LeadGroup = leadDto.LeadGroup,
        //        Status = leadDto.Status,
        //        LeadOwner = leadDto.LeadOwner,
        //        Company = leadDto.Company,
        //        FirstName = leadDto.FirstName,
        //        LastName = leadDto.LastName,
        //        Description = leadDto.Description,
        //        BusinessEmail = leadDto.BusinessEmail,
        //        SecondaryEmail = leadDto.SecondaryEmail,
        //        PhoneNumber = leadDto.PhoneNumber,
        //        AlternateNumber = leadDto.AlternateNumber,
        //        LeadStatus = leadDto.LeadStatus,
        //        LeadSource = leadDto.LeadSource,
        //        LeadStage = leadDto.LeadStage,
        //        Website = leadDto.Website,
        //        Industry = leadDto.Industry,
        //        NumberOfEmployees = leadDto.NumberOfEmployees,
        //        AnnualRevenue = leadDto.AnnualRevenue,
        //        Rating = leadDto.Rating,
        //        EmailOutputFormat = leadDto.EmailOutputFormat,
        //        SkypeId = leadDto.SkypeId
        //    };

        //    db.LeadManagements.Add(lead);
        //    db.SaveChanges();

        //    return Ok(new { message = "Lead saved successfully", leadId = lead.Code });
        //}

        //[HttpPut]
        //[Authorize]
        //[Route("UpdateLead")]
        //public IActionResult UpdateLead([FromBody] LeadManagementDto leadDto)
        //{
        //    // Fetch the existing lead record using the Code ID
        //    var leadRecord = db.LeadManagements.Where(x => x.Code == leadDto.Code).FirstOrDefault();

        //    if (leadRecord != null)
        //    {
        //        // Update the existing lead record with new values from the DTO
        //        leadRecord.Customer = leadDto.Customer;
        //        leadRecord.BranchId = leadDto.BranchId;
        //        leadRecord.CustomerCode = leadDto.CustomerCode;
        //        leadRecord.LeadGroup = leadDto.LeadGroup;
        //        leadRecord.Status = leadDto.Status;
        //        leadRecord.LeadOwner = leadDto.LeadOwner;
        //        leadRecord.Company = leadDto.Company;
        //        leadRecord.FirstName = leadDto.FirstName;
        //        leadRecord.LastName = leadDto.LastName;
        //        leadRecord.Description = leadDto.Description;
        //        leadRecord.BusinessEmail = leadDto.BusinessEmail;
        //        leadRecord.SecondaryEmail = leadDto.SecondaryEmail;
        //        leadRecord.PhoneNumber = leadDto.PhoneNumber;
        //        leadRecord.AlternateNumber = leadDto.AlternateNumber;
        //        leadRecord.LeadStatus = leadDto.LeadStatus;
        //        leadRecord.LeadSource = leadDto.LeadSource;
        //        leadRecord.LeadStage = leadDto.LeadStage;
        //        leadRecord.Website = leadDto.Website;
        //        leadRecord.Industry = leadDto.Industry;
        //        leadRecord.NumberOfEmployees = leadDto.NumberOfEmployees;
        //        leadRecord.AnnualRevenue = leadDto.AnnualRevenue;
        //        leadRecord.Rating = leadDto.Rating;
        //        leadRecord.EmailOutputFormat = leadDto.EmailOutputFormat;
        //        leadRecord.SkypeId = leadDto.SkypeId;

        //        // Update the record in the database
        //        db.LeadManagements.Update(leadRecord);
        //        db.SaveChanges();

        //        return Ok("Lead updated successfully.");
        //    }

        //    // Return a not found response if no record was found
        //    return NotFound("Lead record not found.");
        //}

        //[HttpGet]
        //[Authorize]
        //[Route("GetLeadById/{id}")]
        //public IActionResult GetLeadById(int id)
        //{
        //    // Fetch the lead details by Code ID
        //    var leadDetails = db.LeadManagements
        //        .Where(l => l.Code == id)
        //        .Select(lead => new LeadManagementDto
        //        {
        //            Code = lead.Code,
        //            Customer = lead.Customer,
        //            BranchId = lead.BranchId,
        //            CustomerCode = lead.CustomerCode,
        //            LeadGroup = lead.LeadGroup,
        //            Status = lead.Status,
        //            LeadOwner = lead.LeadOwner,
        //            Company = lead.Company,
        //            FirstName = lead.FirstName,
        //            LastName = lead.LastName,
        //            Description = lead.Description,
        //            BusinessEmail = lead.BusinessEmail,
        //            SecondaryEmail = lead.SecondaryEmail,
        //            PhoneNumber = lead.PhoneNumber,
        //            AlternateNumber = lead.AlternateNumber,
        //            LeadStatus = lead.LeadStatus,
        //            LeadSource = lead.LeadSource,
        //            LeadStage = lead.LeadStage,
        //            Website = lead.Website,
        //            Industry = lead.Industry,
        //            NumberOfEmployees = lead.NumberOfEmployees,
        //            AnnualRevenue = lead.AnnualRevenue,
        //            Rating = lead.Rating,
        //            EmailOutputFormat = lead.EmailOutputFormat,
        //            SkypeId = lead.SkypeId
        //        })
        //        .FirstOrDefault();

        //    // Return the lead details if found, or a not found response if not
        //    if (leadDetails != null)
        //    {
        //        return Ok(leadDetails);
        //    }
        //    else
        //    {
        //        return NotFound("Lead details not found.");
        //    }
        //}

        //[HttpPost]
        //[Authorize]
        //[Route("DeleteLeadById")]
        //public IActionResult DeleteLeadById([FromBody] dynamic model)
        //{
        //    // Deserialize the incoming model to get the Lead ID
        //    LeadManagementDto leadDto = JsonConvert.DeserializeObject<LeadManagementDto>(model.ToString());

        //    // Find the lead record by Code ID
        //    var leadDetails = db.LeadManagements.Where(x => x.Code == leadDto.Code).FirstOrDefault();

        //    // Check if the lead record exists
        //    if (leadDetails == null)
        //    {
        //        return NotFound(new { message = "Lead details not found." });
        //    }

        //    // Remove the lead record from the database
        //    db.LeadManagements.Remove(leadDetails);
        //    db.SaveChanges();

        //    // Return a success message
        //    return Ok(new { message = "Lead details deleted successfully." });
        //}
        [HttpPost]
        [Authorize]
        [Route("GetCRMAllCustomerLeads")]
        public IActionResult GetCRMAllCustomerLeads([FromBody] dynamic model)
        {
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
            var leads = db.crmleads.Where(x => x.branch_id == leadDto.branch_id && x.customer_code == leadDto.customer_code && x.type_lead_cus == "CUS").Select(l => new crmleadsdto
            {
                id = l.id,
                //code = l.code,
                //customer = l.customer,
                branch_id = l.branch_id,
                customer_code = l.customer_code,
                // LeadGroups = l.LeadGroup != null ? db.SalcustomerGroups.Where(g => g.RecordId == l.LeadGroup).FirstOrDefault().MGrp : null,
                //Status = l.Status,
                LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                company = l.company,
                first_name = l.first_name,
                last_name = l.last_name,
                description = l.description,
                business_email = l.business_email,
                secondary_email = l.secondary_email,
                //phonenumber = l.phonenumber,
                //alternate_number = l.alternate_number,
                LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                // LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                website = l.website,
                //IndustryName = l.industry != null ? db.CrmIndustries.Where(i => i.Id == l.industry).FirstOrDefault().Description : null,
                //numberofemployees = l.numberofemployees,
                //annual_revenue = l.annual_revenue,
                rating = l.rating,
                // emailoutputformat = l.emailoutputformat,
                skypeid = l.skypeid,
                title = l.title,
                phone = l.phone,
                fax = l.fax,
                twitter = l.twitter,
                street = l.street,
                city = l.city,
                state = l.state,
                zipcode = l.zipcode,
                country = l.country,
            }).OrderByDescending(x => x.id).ToList();

            return Ok(leads);
        }
        [HttpGet]
        [Authorize]
        [Route("GetAllLeads")]
        public IActionResult GetAllLeads()
        {
            var leads = db.CrmLeadManagements.Select(l => new LeadManagementDto
            {
                Id = l.Id,
                Code = l.Code,
                Customer = l.Customer,
                BranchId = l.BranchId,
                CustomerCode = l.CustomerCode,
                LeadGroups = l.LeadGroup != null ? db.SalcustomerGroups.Where(g => g.RecordId == l.LeadGroup).FirstOrDefault().MGrp : null,
                Status = l.Status,
                LeadOwnerName = l.LeadOwner != null ? db.HrdEmployees.Where(e => e.RecordId == l.LeadOwner).FirstOrDefault().Empname : null,
                Company = l.Company,
                FirstName = l.FirstName,
                LastName = l.LastName,
                Description = l.Description,
                BusinessEmail = l.BusinessEmail,
                SecondaryEmail = l.SecondaryEmail,
                PhoneNumber = l.PhoneNumber,
                AlternateNumber = l.AlternateNumber,
                LeadStatusName = l.LeadStatus != null ? db.CrmLeadStatuses.Where(s => s.Id == l.LeadStatus).FirstOrDefault().Description : null,
                LeadSourceName = l.LeadSource != null ? db.CrmLeadSources.Where(s => s.id == l.LeadSource).FirstOrDefault().description : null,
                LeadStageName = l.LeadStage != null ? db.CrmLeadStages.Where(s => s.id == l.LeadStage).FirstOrDefault().description : null,
                Website = l.Website,
                IndustryName = l.Industry != null ? db.CrmIndustries.Where(i => i.Id == l.Industry).FirstOrDefault().Description : null,
                NumberOfEmployees = l.NumberOfEmployees,
                AnnualRevenue = l.AnnualRevenue,
                Rating = l.Rating,
                EmailOutputFormat = l.EmailOutputFormat,
                SkypeId = l.SkypeId,
                CreatedAt = l.CreatedAt,
                CreatedBy = l.CreatedBy,
                ModifiedBy = l.ModifiedBy,
                ModifiedAt = l.ModifiedAt
            }).ToList();

            return Ok(leads);
        }


        [HttpPost]
        [Authorize]
        [HttpPost("SaveLead")]
        public IActionResult SaveLead([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            var leadDto = JsonConvert.DeserializeObject<LeadManagementDto>(model.ToString());

            if (leadDto == null)
            {
                return BadRequest("Invalid lead data.");
            }

            // Create a new CrmLeadManagement entity and map the DTO fields
            var lead = new CrmLeadManagement
            {
                Code = leadDto.Code,
                Customer = leadDto.Customer,
                BranchId = leadDto.BranchId,
                CustomerCode = leadDto.CustomerCode,
                LeadGroup = leadDto.LeadGroup,
                Status = leadDto.Status,
                LeadOwner = leadDto.LeadOwner,
                Company = leadDto.Company,
                FirstName = leadDto.FirstName,
                LastName = leadDto.LastName,
                Description = leadDto.Description,
                BusinessEmail = leadDto.BusinessEmail,
                SecondaryEmail = leadDto.SecondaryEmail,
                PhoneNumber = leadDto.PhoneNumber,
                AlternateNumber = leadDto.AlternateNumber,
                LeadStatus = leadDto.LeadStatus,
                LeadSource = leadDto.LeadSource,
                LeadStage = leadDto.LeadStage,
                Website = leadDto.Website,
                Industry = leadDto.Industry,
                NumberOfEmployees = leadDto.NumberOfEmployees,
                AnnualRevenue = leadDto.AnnualRevenue,
                Rating = leadDto.Rating,
                EmailOutputFormat = leadDto.EmailOutputFormat,
                SkypeId = leadDto.SkypeId,
                CreatedAt = DateTime.UtcNow, // Set the created date to now
                CreatedBy = leadDto.CreatedBy, // Assuming you pass the CreatedBy ID in the DTO
                ModifiedBy = null, // Initially set to null, can be updated later
                ModifiedAt = null // Initially set to null, can be updated later
            };

            // Add the lead to the database context and save changes
            db.CrmLeadManagements.Add(lead);
            db.SaveChanges();
            int leadid = lead.Id;
            crmLeadContact crmLeadContact = new crmLeadContact();
            var contacts = new crmLeadContact
            {
                LeadId = leadid,
                FirstName = leadDto.FirstName,
                LastName = leadDto.LastName,
                Email = leadDto.Email,
                Mobile = leadDto.Mobile,
                Designation = leadDto.Designation,
                Location = leadDto.Location,
            };
            db.CrmLeadContact.Add(contacts);
            db.SaveChanges();

            // Return a success message with the ID of the created lead
            return Ok(new { message = "Lead saved successfully", leadId = lead.Id });
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMAllLeads")]
        public IActionResult GetCRMAllLeads([FromBody] dynamic model)
        {
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
            var leads = db.crmleads.Where(x => x.branch_id == leadDto.branch_id && x.customer_code == leadDto.customer_code && x.type_lead_cus != "CUS").Select(l => new crmleadsdto
            {
                id = l.id,
                //code = l.code,
                //customer = l.customer,
                branch_id = l.branch_id,
                customer_code = l.customer_code,
                // LeadGroups = l.LeadGroup != null ? db.SalcustomerGroups.Where(g => g.RecordId == l.LeadGroup).FirstOrDefault().MGrp : null,
                //Status = l.Status,
                LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                company = l.company,
                first_name = l.first_name,
                last_name = l.last_name,
                description = l.description,
                business_email = l.business_email,
                secondary_email = l.secondary_email,
                //phonenumber = l.phonenumber,
                //alternate_number = l.alternate_number,
                LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                // LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                website = l.website,
                //IndustryName = l.industry != null ? db.CrmIndustries.Where(i => i.Id == l.industry).FirstOrDefault().Description : null,
                //numberofemployees = l.numberofemployees,
                //annual_revenue = l.annual_revenue,
                rating = l.rating,
                // emailoutputformat = l.emailoutputformat,
                skypeid = l.skypeid,
                title = l.title,
                phone = l.phone,
                fax = l.fax,
                twitter = l.twitter,
                street = l.street,
                city = l.city,
                state = l.state,
                zipcode = l.zipcode,
                country = l.country,
                convert_customer = l.converted_customer == "Yes" ? true : false,
                customerconverted = l.converted_customer,
                created_at=l.created_at
            }).OrderByDescending(x=>x.id).ToList();

            return Ok(leads);
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMAllLeadsById")]
        public IActionResult GetCRMAllLeadsById([FromBody] dynamic model)
        {
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
            var leads = db.crmleads.Where(x => x.id == leadDto.id && x.branch_id == leadDto.branch_id && x.customer_code == leadDto.customer_code && x.type_lead_cus == null).Select(l => new crmleadsdto
            {
                id = l.id,
                //code = l.code,
                //customer = l.customer,
                branch_id = l.branch_id,
                customer_code = l.customer_code,
                lead_owner = l.lead_owner,
                // LeadGroups = l.LeadGroup != null ? db.SalcustomerGroups.Where(g => g.RecordId == l.LeadGroup).FirstOrDefault().MGrp : null,
                //Status = l.Status,
                LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                company = l.company,
                first_name = l.first_name,
                last_name = l.last_name,
                description = l.description,
                business_email = l.business_email,
                secondary_email = l.secondary_email,
                phonenumber = l.phonenumber,
                alternate_number = l.alternate_number,
                LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                website = l.website,
                IndustryName = l.industry,
                numberofemployees = l.numberofemployees,
                annual_revenue = l.annual_revenue,
                rating = l.rating,
                lead_source = l.lead_source,
                lead_stage = l.lead_stage,
                lead_status = l.lead_status,
                industry = l.industry,
                emailoutputformat = l.emailoutputformat,
                skypeid = l.skypeid,
                title = l.title,
                phone = l.phone,
                fax = l.fax,
                twitter = l.twitter,
                street = l.street,
                city = l.city,
                state = l.state,
                zipcode = l.zipcode,
                country = l.country,
                convert_customer = (l.converted_customer == "Yes") ? true ? l.converted_customer == null : false : false,
                customerconverted = l.converted_customer
            }).ToList();

            return Ok(leads);
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMAllcustomerById")]
        public IActionResult GetCRMAllcustomerById([FromBody] dynamic model)
        {
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());

            var leads = db.crmleads.Where(x => x.id == leadDto.id).Select(l => new crmleadsdto
            {
                id = l.id,
                //code = l.code,
                //customer = l.customer,
                branch_id = l.branch_id,
                customer_code = l.customer_code,
                lead_owner = l.lead_owner,
                // LeadGroups = l.LeadGroup != null ? db.SalcustomerGroups.Where(g => g.RecordId == l.LeadGroup).FirstOrDefault().MGrp : null,
                //Status = l.Status,
                LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                company = l.company,
                first_name = l.first_name,
                last_name = l.last_name,
                description = l.description,
                business_email = l.business_email,
                secondary_email = l.secondary_email,
                phonenumber = l.phonenumber,
                alternate_number = l.alternate_number,
                LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                // LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                website = l.website,
                //IndustryName = l.industry != null ? db.CrmIndustries.Where(i => i.Id == l.industry).FirstOrDefault().Description : null,
                //numberofemployees = l.numberofemployees,
                //annual_revenue = l.annual_revenue,
                rating = l.rating,
                // emailoutputformat = l.emailoutputformat,
                skypeid = l.skypeid,
                title = l.title,
                phone = l.phone,
                fax = l.fax,
                twitter = l.twitter,
                street = l.street,
                city = l.city,
                state = l.state,
                zipcode = l.zipcode,
                country = l.country,
            }).Where(x => x.branch_id == leadDto.branch_id && x.customer_code == leadDto.customer_code && x.type_lead_cus == "CUS").ToList();

            return Ok(leads);
        }
        [HttpPost]
        [Authorize]
        [HttpPost("SaveCRMLead")]
        public IActionResult SaveCRMLead([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());

            var check = db.crmleads.Where(a => a.company == leadDto.company).Count();
            var nameCheck = db.crmleads.Where(a => a.first_name == leadDto.first_name && a.last_name == leadDto.last_name).Count();

            var email = db.crmleads.Where(a => a.business_email == leadDto.business_email).Count();
            var phonenumber = db.crmleads.Where(a => a.phonenumber == leadDto.phonenumber).Count();
            if (check > 0)
            {
                return BadRequest("Company name already exists. Please enter a unique company name.");
            }
            // Check if the email address already exists
            else if (email > 0)
            {
                return BadRequest("Email address already exists. Please enter a unique email address.");
            }
            // Check if the phone number already exists
            else if (phonenumber > 0)
            {
                return BadRequest("Phone number already exists. Please enter a unique phone number.");
            }
            // Check if the first name and last name combination already exists
            else if (nameCheck > 0)
            {
                return BadRequest("A lead with the same first name and last name already exists.");
            }
            // Create a new CrmLeadManagement entity and map the DTO fields
            var lead = new crmleads
            {
                //code = leadDto.Code,
                // customer = leadDto.Customer,
                branch_id = leadDto.branch_id,
                customer_code = leadDto.customer_code,
                // lead_group = leadDto.LeadGroup,
                // status = leadDto.Status,
                lead_owner = leadDto.lead_owner,

                company = leadDto.company,
                first_name = leadDto.first_name,
                last_name = leadDto.last_name,
                description = leadDto.description,
                business_email = leadDto.business_email,
                secondary_email = leadDto.secondary_email,
                phonenumber = leadDto.phonenumber,
                alternate_number = leadDto.alternate_number,
                lead_status = leadDto.lead_status,
                lead_source = leadDto.lead_source,
                lead_stage = leadDto.lead_stage,
                website = leadDto.website,
                industry = leadDto.industry,
                numberofemployees = leadDto.numberofemployees,
                annual_revenue = leadDto.annual_revenue,
                rating = leadDto.rating,
                emailoutputformat = leadDto.emailoutputformat,
                skypeid = leadDto.skypeid,
                title = leadDto.title,
                phone = leadDto.phone,
                fax = leadDto.fax,
                twitter = leadDto.twitter,
                street = leadDto.street,
                city = leadDto.city,
                state = leadDto.state,
                zipcode = leadDto.zipcode,
                country = leadDto.country,
                created_at=DateTime.Now,
                

            };

            // Add the lead to the database context and save changes
            db.crmleads.Add(lead);
            db.SaveChanges();
            int leadid = lead.id;
           
            crmLeadContact crmLeadContact = new crmLeadContact();
            var contacts = new crmLeadContact
            {
                LeadId = leadid,
                FirstName = leadDto.FirstName,
                LastName = leadDto.LastName,
                Email = leadDto.Email,
                Mobile = leadDto.Mobile,
                Designation = leadDto.Designation,
                Location = leadDto.Location,
                BranchId=leadDto.branch_id,
                CustomerCode=leadDto.customer_code
                ,PrimaryContact=true
            };
            db.CrmLeadContact.Add(contacts);
            db.SaveChanges();
            if (lead.emailoutputformat != "true")            
            {
                sendEmail sendEmail = new sendEmail();
                sendEmail.EmailSend("Lead Details Created", lead.business_email, "Hi" + " " + lead.first_name + lead.last_name + "," + "\n" + " Below are the lead details:" + "\n" + "Company Name:" + lead.company + "\n", null);
            }
            //var result= GetCRMAllLeads(model);
            // Return a success message with the ID of the created lead
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("GetcrmleadcontactDeleteById")]
        public IActionResult GetcrmleadcontactDelete([FromBody] int id)
        {

            var contact = db.CrmLeadContact.FirstOrDefault(y => y.Id == id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found." });
            }

            var callLog = db.CrmCallLogs.Where(cl => cl.ContactId == contact.FirstName && cl.leadid == contact.LeadId).Count();

            if (callLog > 0)
            {

                return BadRequest(new { message = "This contact is associated with call logs and cannot be deleted." });
            }
            else
            {

                db.CrmLeadContact.Remove(contact);
                db.SaveChanges();
            }

            return Ok(new { message = "Contact deleted successfully." });
        }
        [HttpPost]
        [Authorize]
        [HttpPost("updateCRMCustomer")]
        public IActionResult updateCRMCustomer([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
            var res = db.crmleads.Where(x => x.id == leadDto.id).FirstOrDefault();
            if (res != null)
            {
                res.branch_id = leadDto.branch_id;
                res.customer_code = leadDto.customer_code;
                // lead_group = leadDto.LeadGroup,
                // status = leadDto.Status,
                res.lead_owner = leadDto.lead_owner;

                res.company = leadDto.company;
                res.first_name = leadDto.first_name;
                res.last_name = leadDto.last_name;
                res.description = leadDto.description;
                res.business_email = leadDto.business_email;
                res.secondary_email = leadDto.secondary_email;
                res.phonenumber = leadDto.phonenumber;
                res.alternate_number = leadDto.alternate_number;
                res.lead_status = leadDto.lead_status;
                res.lead_source = leadDto.lead_source;
                res.lead_stage = leadDto.lead_stage;
                res.website = leadDto.website;
                res.industry = leadDto.industry;
                res.numberofemployees = leadDto.numberofemployees;
                res.annual_revenue = leadDto.annual_revenue;
                res.rating = leadDto.rating;
                res.emailoutputformat = leadDto.emailoutputformat;
                res.skypeid = leadDto.skypeid;
                res.title = leadDto.title;
                res.phone = leadDto.phone;
                res.fax = leadDto.fax;
                res.twitter = leadDto.twitter;
                res.street = leadDto.street;
                res.city = leadDto.city;
                res.state = leadDto.state;
                res.zipcode = leadDto.zipcode;
                res.country = leadDto.country;
                res.type_lead_cus = "Cus";
                res.modified_at = DateTime.Now;
            }
            // Create a new CrmLeadManagement entity and map the DTO fields


            // Add the lead to the database context and save changes
            db.crmleads.Update(res);
            db.SaveChanges();

            //var result= GetCRMAllLeads(model);
            // Return a success message with the ID of the created lead
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [HttpPost("updateCRMLead")]
        public IActionResult updateCRMLead([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
            var res = db.crmleads.Where(x => x.id == leadDto.id).FirstOrDefault();
            if (res != null)
            {
                res.branch_id = leadDto.branch_id;
                res.customer_code = leadDto.customer_code;
                // lead_group = leadDto.LeadGroup,
                // status = leadDto.Status,
                res.lead_owner = leadDto.lead_owner;

                res.company = leadDto.company;
                res.first_name = leadDto.first_name;
                res.last_name = leadDto.last_name;
                res.description = leadDto.description;
                res.business_email = leadDto.business_email;
                res.secondary_email = leadDto.secondary_email;
                res.phonenumber = leadDto.phonenumber;
                res.alternate_number = leadDto.alternate_number;
                res.lead_status = leadDto.lead_status;
                res.lead_source = leadDto.lead_source;
                res.lead_stage = leadDto.lead_stage;
                res.website = leadDto.website;
                res.industry = leadDto.industry;
                res.numberofemployees = leadDto.numberofemployees;
                res.annual_revenue = leadDto.annual_revenue;
                res.rating = leadDto.rating;
                res.emailoutputformat = leadDto.emailoutputformat;
                res.skypeid = leadDto.skypeid;
                res.title = leadDto.title;
                res.phone = leadDto.phone;
                res.fax = leadDto.fax;
                res.twitter = leadDto.twitter;
                res.street = leadDto.street;
                res.city = leadDto.city;
                res.state = leadDto.state;
                res.zipcode = leadDto.zipcode;
                res.country = leadDto.country;
                res.modified_at = DateTime.Now;
            }
            // Create a new CrmLeadManagement entity and map the DTO fields
           

            // Add the lead to the database context and save changes
            db.crmleads.Update(res);
            db.SaveChanges();

            //var result= GetCRMAllLeads(model);
            // Return a success message with the ID of the created lead
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMAllCustomersById")]
        public IActionResult GetCRMAllCustomersById([FromBody] dynamic model)
        {
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
            var leads = db.crmleads.Where(x => x.id == leadDto.id && x.type_lead_cus == "Cus" && x.branch_id == leadDto.branch_id && x.customer_code == leadDto.customer_code).Select(l => new crmleadsdto
            {
                id = l.id,
                //code = l.code,
                //customer = l.customer,
                branch_id = l.branch_id,
                customer_code = l.customer_code,
                lead_owner = l.lead_owner,
                // LeadGroups = l.LeadGroup != null ? db.SalcustomerGroups.Where(g => g.RecordId == l.LeadGroup).FirstOrDefault().MGrp : null,
                //Status = l.Status,
                LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                company = l.company,
                first_name = l.first_name,
                last_name = l.last_name,
                description = l.description,
                business_email = l.business_email,
                secondary_email = l.secondary_email,
                phonenumber = l.phonenumber,
                alternate_number = l.alternate_number,
                LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                website = l.website,
                IndustryName = l.industry,
                numberofemployees = l.numberofemployees,
                annual_revenue = l.annual_revenue,
                rating = l.rating,
                lead_source = l.lead_source,
                lead_stage = l.lead_stage,
                lead_status = l.lead_status,
                industry = l.industry,
                emailoutputformat = l.emailoutputformat,
                skypeid = l.skypeid,
                title = l.title,
                phone = l.phone,
                fax = l.fax,
                twitter = l.twitter,
                street = l.street,
                city = l.city,
                state = l.state,
                zipcode = l.zipcode,
                country = l.country,
                created_at =l.created_at
            }).ToList();

            return Ok(leads);
        }
        [HttpPost]
        [Authorize]
        [HttpPost("SaveCRMCustomer")]
        public IActionResult SaveCRMCustomer([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            // Deserialize the incoming dynamic model to a specific DTO
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());

            var check = db.crmleads.Where(a => a.company == leadDto.company).Count();
            var nameCheck = db.crmleads.Where(a => a.first_name == leadDto.first_name && a.last_name == leadDto.last_name).Count();

            var email = db.crmleads.Where(a => a.business_email == leadDto.business_email).Count();
            var phonenumber = db.crmleads.Where(a => a.phonenumber == leadDto.phonenumber).Count();
            if (check > 0)
            {
                return BadRequest("Company name already exists. Please enter a unique company name.");
            }
            // Check if the email address already exists
            else if (email > 0)
            {
                return BadRequest("Email address already exists. Please enter a unique email address.");
            }
            // Check if the phone number already exists
            else if (phonenumber > 0)
            {
                return BadRequest("Phone number already exists. Please enter a unique phone number.");
            }
            // Check if the first name and last name combination already exists
            else if (nameCheck > 0)
            {
                return BadRequest("A lead with the same first name and last name already exists.");
            }

            // Create a new CrmLeadManagement entity and map the DTO fields
            var lead = new crmleads
            {
                //code = leadDto.Code,
                // customer = leadDto.Customer,
                branch_id = leadDto.branch_id,
                customer_code = leadDto.customer_code,
                // lead_group = leadDto.LeadGroup,
                // status = leadDto.Status,
                lead_owner = leadDto.lead_owner,
                company = leadDto.company,
                first_name = leadDto.first_name,
                last_name = leadDto.last_name,
                description = leadDto.description,
                business_email = leadDto.business_email,
                secondary_email = leadDto.secondary_email,
                phonenumber = leadDto.phonenumber,
                alternate_number = leadDto.alternate_number,
                lead_status = leadDto.lead_status,
                lead_source = leadDto.lead_source,
                lead_stage = leadDto.lead_stage,
                website = leadDto.website,
                industry = leadDto.industry,
                numberofemployees = leadDto.numberofemployees,
                annual_revenue = leadDto.annual_revenue,
                rating = leadDto.rating,
                emailoutputformat = leadDto.emailoutputformat,
                skypeid = leadDto.skypeid,
                title = leadDto.title,
                phone = leadDto.phone,
                fax = leadDto.fax,
                twitter = leadDto.twitter,
                street = leadDto.street,
                city = leadDto.city,
                state = leadDto.state,
                zipcode = leadDto.zipcode,
                country = leadDto.country,
                type_lead_cus = "Cus"
            };

            // Add the lead to the database context and save changes
            db.crmleads.Add(lead);
            db.SaveChanges();

            if (lead.emailoutputformat != "true")
            {
                sendEmail sendEmail = new sendEmail();
                sendEmail.EmailSend("Lead Details Created", lead.business_email, "Hi" + " " + lead.first_name + lead.last_name + "," + "\n" + " Below are the lead details:" + "\n" + "Company Name:" + lead.company + "\n", null);
            }
            //var result= GetCRMAllLeads(model);
            // Return a success message with the ID of the created lead
            return Ok();
        }


        [HttpGet]
        [Authorize]
        [HttpGet("GetLeadById/{id}")]
        public IActionResult GetLeadById(int id)
        {
            // Find the lead record by Id
            var lead = db.CrmLeadManagements
                         .Where(l => l.Id == id)
                         .Select(l => new LeadManagementDto
                         {
                             Id = l.Id,
                             Code = l.Code,
                             Customer = l.Customer,
                             BranchId = l.BranchId,
                             CustomerCode = l.CustomerCode,
                             LeadGroup = l.LeadGroup,
                             Status = l.Status,
                             LeadOwner = l.LeadOwner,
                             Company = l.Company,
                             FirstName = l.FirstName,
                             LastName = l.LastName,
                             Description = l.Description,
                             BusinessEmail = l.BusinessEmail,
                             SecondaryEmail = l.SecondaryEmail,
                             PhoneNumber = l.PhoneNumber,
                             AlternateNumber = l.AlternateNumber,
                             LeadStatus = l.LeadStatus,
                             LeadSource = l.LeadSource,
                             LeadStage = l.LeadStage,
                             Website = l.Website,
                             Industry = l.Industry,
                             NumberOfEmployees = l.NumberOfEmployees,
                             AnnualRevenue = l.AnnualRevenue,
                             Rating = l.Rating,
                             EmailOutputFormat = l.EmailOutputFormat,
                             SkypeId = l.SkypeId,
                             //CreatedAt = l.CreatedAt,
                             //CreatedBy = l.CreatedBy,
                             //ModifiedAt = l.ModifiedAt,
                             //ModifiedBy = l.ModifiedBy
                         })
                         .FirstOrDefault();

            // Check if the lead exists
            if (lead == null)
            {
                return NotFound(new { message = "Lead not found." });
            }


            // Return the lead details
            return Ok(lead);
        }

        [HttpPost]
        [Authorize]
        [Route("UpdateLead")]
        public IActionResult UpdateLead([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            LeadManagementDto leadDto = JsonConvert.DeserializeObject<LeadManagementDto>(model.ToString());

            if (leadDto == null || leadDto.Id <= 0)
            {
                return BadRequest(new { message = "Invalid lead data or missing lead ID." });
            }

            // Fetch the existing lead record using the Id
            var lead = db.CrmLeadManagements.FirstOrDefault(l => l.Id == leadDto.Id);

            if (lead == null)
            {
                return NotFound(new { message = "Lead not found." });
            }

            // Update the existing lead record with new values from the DTO
            lead.Code = leadDto.Code;
            lead.Customer = leadDto.Customer;
            lead.BranchId = leadDto.BranchId;
            lead.CustomerCode = leadDto.CustomerCode;
            lead.LeadGroup = leadDto.LeadGroup;
            lead.Status = leadDto.Status;
            lead.LeadOwner = leadDto.LeadOwner;
            lead.Company = leadDto.Company;
            lead.FirstName = leadDto.FirstName;
            lead.LastName = leadDto.LastName;
            lead.Description = leadDto.Description;
            lead.BusinessEmail = leadDto.BusinessEmail;
            lead.SecondaryEmail = leadDto.SecondaryEmail;
            lead.PhoneNumber = leadDto.PhoneNumber;
            lead.AlternateNumber = leadDto.AlternateNumber;
            lead.LeadStatus = leadDto.LeadStatus;
            lead.LeadSource = leadDto.LeadSource;
            lead.LeadStage = leadDto.LeadStage;
            lead.Website = leadDto.Website;
            lead.Industry = leadDto.Industry;
            lead.NumberOfEmployees = leadDto.NumberOfEmployees;
            lead.AnnualRevenue = leadDto.AnnualRevenue;
            lead.Rating = leadDto.Rating;
            lead.EmailOutputFormat = leadDto.EmailOutputFormat;
            lead.SkypeId = leadDto.SkypeId;

            // Uncomment these lines if tracking modification metadata
            // lead.ModifiedAt = DateTime.UtcNow; 
            // lead.ModifiedBy = leadDto.ModifiedBy; 

            // Update the record in the database
            db.CrmLeadManagements.Update(lead);
            db.SaveChanges();

            return Ok(new { message = "Lead updated successfully." });
        }




        // Delete a lead by ID
        [HttpPost]
        [Authorize]
        [Route("DeleteLeadById")]
        public IActionResult DeleteLeadById([FromBody] dynamic model)
        {
            // Deserialize the incoming model to the DTO
            LeadManagementDto leadDto = JsonConvert.DeserializeObject<LeadManagementDto>(model.ToString());

            // Find the lead record by Id
            var leadDetails = db.CrmLeadManagements.Where(x => x.Id == leadDto.Id).FirstOrDefault();

            // Check if the lead record exists
            if (leadDetails == null)
            {
                return NotFound(new { message = "Lead details not found." });
            }

            // Remove the lead record from the database
            db.CrmLeadManagements.Remove(leadDetails);
            db.SaveChanges();

            // Return a success message
            return Ok(new { message = "Lead details deleted successfully." });
        }



        //Dropdowns API'S ------------------------------------------------------------------------

        [HttpGet]
        [Authorize]
        [Route("GetAllCrmLeadSources")]
        public IActionResult GetAllCrmLeadSources()
        {
            // Retrieve all crmLeadSource records and map them to the DTO
            var leadSources = db.CrmLeadSources
                .Select(ls => new CrmLeadSourceDto
                {
                    Id = ls.id,
                    Description = ls.description,
                    BranchId = ls.branch_id,
                    CustomerCode = ls.customer_code,
                    CreatedAt = ls.created_at,
                    ModifiedAt = ls.modified_at
                }).ToList();

            // Return the list of lead sources
            return Ok(leadSources);
        }
        [HttpGet]
        [Authorize]
        [Route("GetAllcrmleadtitles")]
        public IActionResult GetAllcrmleadtitles()
        {
            // Retrieve all crmLeadSource records and map them to the DTO
            var leadtitles = db.crmleadtitles
                .Select(ls => new crmleadtitledto
                {
                    id = ls.id,
                    description = ls.description,
                    branch_id = ls.branch_id,
                    customer_code = ls.customer_code,
                    created_at = ls.created_at,
                    modified_at = ls.modified_at
                }).ToList();

            // Return the list of lead sources
            return Ok(leadtitles);
        }

        // GET api/CrmLeadStage/GetAllCrmLeadStages
        [HttpGet]
        [Authorize]  // Assuming authorization is required for this API
        [Route("GetAllCrmLeadStages")]
        public IActionResult GetAllCrmLeadStages()
        {
            // Retrieve all crmLeadStage records and map them to the DTO
            var leadStages = db.CrmLeadStages
                .Select(ls => new CrmLeadStageDto
                {
                    Id = ls.id,
                    Description = ls.description,

                    BranchId = ls.branch_id,
                    CustomerCode = ls.customer_code,

                    CreatedAt = ls.created_at,

                    ModifiedAt = ls.modified_at
                })
                .ToList();

            // Return the list of lead stages
            return Ok(leadStages);
        }


        // GET api/CrmIndustry/GetAllCrmIndustries
        [HttpGet]
        [Authorize]  // Assuming authorization is required for this API
        [Route("GetAllCrmIndustries")]
        public IActionResult GetAllCrmIndustries()
        {
            // Retrieve all crmIndustry records and map them to the DTO
            var industries = db.CrmIndustries
                .Select(ind => new CrmIndustryDto
                {
                    Id = ind.Id,
                    Description = ind.Description,
                    RecStatus = ind.RecStatus,
                    BranchId = ind.BranchId,
                    CustomerCode = ind.CustomerCode,
                    CreatedBy = ind.CreatedBy,
                    CreatedAt = ind.CreatedAt,
                    ModifiedBy = ind.ModifiedBy,
                    ModifiedAt = ind.ModifiedAt
                })
                .ToList();

            // Return the list of industries
            return Ok(industries);
        }

        // GET api/CrmLeadStatus/GetAllCrmLeadStatuses
        [HttpGet]
        [Authorize]  // Assuming authorization is required for this API
        [Route("GetAllCrmLeadStatuses")]
        public IActionResult GetAllCrmLeadStatuses()
        {
            // Retrieve all crmLeadStatus records and map them to the DTO
            var leadStatuses = db.CrmLeadStatuses
                .Select(ls => new CrmLeadStatusDto
                {
                    Id = ls.Id,
                    StageId = ls.StageId,
                    Description = ls.Description,
                    CreatedBy = ls.CreatedBy,
                    CreatedAt = ls.CreatedAt,
                    ModifiedBy = ls.ModifiedBy,
                    ModifiedAt = ls.ModifiedAt,
                    BranchId = ls.BranchId,
                    CustomerCode = ls.CustomerCode,
                    RecStatus = ls.RecStatus
                })
                .ToList();

            // Return the list of lead statuses
            return Ok(leadStatuses);
        }

        //lead status
        [HttpPost]
        [Route("api/CRMRx/getcrmleadstatus")]
        public IActionResult getcrmleadstatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto crmLeadStatus = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.Where(x => x.BranchId == crmLeadStatus.BranchId && x.CustomerCode == crmLeadStatus.CustomerCode).Select(x => new CrmLeadStatusDto
            {
                Id = x.Id,
                Description = x.Description,
                BranchId = x.BranchId,
                CustomerCode = x.CustomerCode

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/savecrmleadstatus")]
        public IActionResult savecrmleadstatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto crmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            if (crmLeadStatusDto != null)
            {
                crmleadstatus crmleadstatus = new crmleadstatus();
                crmleadstatus.Description = crmLeadStatusDto.Description;
                crmleadstatus.BranchId = crmLeadStatusDto.BranchId;
                crmleadstatus.CustomerCode = crmLeadStatusDto.CustomerCode;
                crmleadstatus.CreatedAt = DateTime.Now;
                db.crmleadstatuses.Add(crmleadstatus);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatecrmleadstatus")]
        public IActionResult updatecrmleadstatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto crmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.Where(x => x.Id == crmLeadStatusDto.Id && x.BranchId == crmLeadStatusDto.BranchId && x.CustomerCode == crmLeadStatusDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                result.Description = crmLeadStatusDto.Description;
                result.BranchId = crmLeadStatusDto.BranchId;
                result.CustomerCode = crmLeadStatusDto.CustomerCode;
                result.ModifiedAt = DateTime.Now;
                db.crmleadstatuses.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deletecrmleadstatus")]
        public IActionResult deletecrmleadstatus([FromBody] dynamic model)
        {
            CrmLeadStatusDto crmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.Where(x => x.Id == crmLeadStatusDto.Id && x.BranchId == crmLeadStatusDto.BranchId && x.CustomerCode == crmLeadStatusDto.CustomerCode).FirstOrDefault(); ;
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
            CrmLeadStatusDto crmLeadStatusDto = JsonConvert.DeserializeObject<CrmLeadStatusDto>(model.ToString());

            var result = db.crmleadstatuses.
                Where(x => x.Id == crmLeadStatusDto.Id && x.BranchId == crmLeadStatusDto.BranchId && x.CustomerCode == crmLeadStatusDto.CustomerCode)
                .Select(x => new CrmLeadStatusDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    BranchId = x.BranchId,
                    CustomerCode = x.CustomerCode
                }).FirstOrDefault();



            return Ok(result);
        }
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
        [Route("api/CRMRx/savecrmleadstage")]
        public IActionResult savecrmleadstage([FromBody] dynamic model)
        {
            CrmLeadStageDto crmLeadStageDto = JsonConvert.DeserializeObject<CrmLeadStageDto>(model.ToString());

            if (crmLeadStageDto != null)
            {
                crmleadstage crmleadstatus = new crmleadstage();
                crmleadstatus.description = crmLeadStageDto.Description;
                crmleadstatus.branch_id = crmLeadStageDto.BranchId;
                crmleadstatus.customer_code = crmLeadStageDto.CustomerCode;
                crmleadstatus.created_at = DateTime.Now;
                db.crmleadstages.Add(crmleadstatus);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatecrmleadstage")]
        public IActionResult updatecrmleadstage([FromBody] dynamic model)
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
        [Route("api/CRMRx/deletecrmleadstage")]
        public IActionResult deletecrmleadstage([FromBody] dynamic model)
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
            CrmLeadSourceDto crmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.Where(x => x.branch_id == crmLeadSourceDto.BranchId && x.customer_code == crmLeadSourceDto.CustomerCode).Select(x => new CrmLeadSourceDto
            {
                Id = x.id,
                Description = x.description,
                BranchId = x.branch_id,
                CustomerCode = x.customer_code

            }).ToList();
            return Ok(result);
        }
        [HttpPost]
        [Route("api/CRMRx/savecrmleadsource")]
        public IActionResult savecrmleadsource([FromBody] dynamic model)
        {
            CrmLeadSourceDto crmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            if (crmLeadSourceDto != null)
            {
                crmleadsource crmleadsource = new crmleadsource();
                crmleadsource.description = crmLeadSourceDto.Description;
                crmleadsource.branch_id = crmLeadSourceDto.BranchId;
                crmleadsource.customer_code = crmLeadSourceDto.CustomerCode;
                crmleadsource.created_at = DateTime.Now;
                db.crmleadsources.Add(crmleadsource);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/updatecrmleadsource")]
        public IActionResult updatecrmleadsource([FromBody] dynamic model)
        {

            CrmLeadSourceDto crmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.Where(x => x.id == crmLeadSourceDto.Id && x.branch_id == crmLeadSourceDto.BranchId && x.customer_code == crmLeadSourceDto.CustomerCode).FirstOrDefault();
            if (result != null)
            {
                result.description = crmLeadSourceDto.Description;
                result.branch_id = crmLeadSourceDto.BranchId;
                result.customer_code = crmLeadSourceDto.CustomerCode;
                result.modified_at = DateTime.Now;
                db.crmleadsources.Update(result);
                db.SaveChanges();
            }


            return Ok();
        }
        [HttpPost]
        [Route("api/CRMRx/deletecrmleadsource")]
        public IActionResult deletecrmleadsource([FromBody] dynamic model)
        {
            CrmLeadSourceDto crmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.Where(x => x.id == crmLeadSourceDto.Id && x.branch_id == crmLeadSourceDto.BranchId && x.customer_code == crmLeadSourceDto.CustomerCode).FirstOrDefault();
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
            CrmLeadSourceDto crmLeadSourceDto = JsonConvert.DeserializeObject<CrmLeadSourceDto>(model.ToString());

            var result = db.crmleadsources.
                Where(x => x.id == crmLeadSourceDto.Id && x.branch_id == crmLeadSourceDto.BranchId && x.customer_code == crmLeadSourceDto.CustomerCode)
                .Select(x => new CrmLeadSourceDto
                {
                    Description = x.description,
                    Id = x.id,
                    BranchId = x.branch_id,
                    CustomerCode = x.customer_code
                }).FirstOrDefault();



            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        [Route("SavecrmleadContact")]
        public IActionResult SavecrmleadContact([FromBody] dynamic model)
        {
            crmLeadContactDto crmLeadContactDto = JsonConvert.DeserializeObject<crmLeadContactDto>(model.ToString());
            var email = db.CrmLeadContact.Where(a => a.Email == crmLeadContactDto.Email).Count();
            var phonenumber = db.CrmLeadContact.Where(a => a.Mobile == crmLeadContactDto.Mobile).Count();
          
            // Check if the email address already exists
            if (email > 0)
            {
                return BadRequest("Email address already exists. Please enter a unique email address.");
            }
            // Check if the phone number already exists
            else if (phonenumber > 0)
            {
                return BadRequest("Phone number already exists. Please enter a unique phone number.");
            }
            if (crmLeadContactDto != null)
            {
                if (crmLeadContactDto.PrimaryContact == true)
                {
                    var result = db.CrmLeadContact.Where(y => y.LeadId == crmLeadContactDto.LeadId).ToList();
                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].PrimaryContact = false;
                    }
                    db.SaveChanges();
                }
                crmLeadContact crmLeadContact = new crmLeadContact();
                crmLeadContact.Id = crmLeadContactDto.Id;
                crmLeadContact.LeadId = crmLeadContactDto.LeadId;
                crmLeadContact.FirstName = crmLeadContactDto.FirstName;
                crmLeadContact.LastName = crmLeadContactDto.LastName;
                crmLeadContact.Mobile = crmLeadContactDto.Mobile;
                crmLeadContact.Email = crmLeadContactDto.Email;
                crmLeadContact.Designation = crmLeadContactDto.Designation;
                crmLeadContact.Location = crmLeadContactDto.Location;
                crmLeadContact.BranchId = crmLeadContactDto.BranchId;
                crmLeadContact.CustomerCode = crmLeadContactDto.CustomerCode;
                crmLeadContact.CreatedAt = crmLeadContactDto.CreatedAt;
                crmLeadContact.CreatedBy = crmLeadContactDto.CreatedBy;
                crmLeadContact.ModifiedBy = crmLeadContactDto.ModifiedBy;
                crmLeadContact.ModifiedAt = crmLeadContactDto.ModifiedAt;
                crmLeadContact.customer_id = crmLeadContactDto.customer_id;
                db.CrmLeadContact.Add(crmLeadContact);
                db.SaveChanges();


            }
            return Ok();

        }
        [HttpPost]
        [Authorize]
        [Route("Updatecrmleadcontact")]
        public IActionResult Updatecrmleadcontact([FromBody] dynamic model)
        {
            crmLeadContactDto crmLeadContactDto = JsonConvert.DeserializeObject<crmLeadContactDto>(model.ToString());
            var result = db.CrmLeadContact.Where(y => y.Id == crmLeadContactDto.Id).FirstOrDefault();
            if (result != null)
            {
                if (result.PrimaryContact == true)
                {
                    List<crmLeadContact> __results;
                    if (crmLeadContactDto.customer_id > 0)
                    {
                        __results = db.CrmLeadContact.Where(y => y.customer_id == crmLeadContactDto.customer_id).ToList();
                    }
                    else
                    {
                        __results = db.CrmLeadContact.Where(y => y.LeadId == crmLeadContactDto.LeadId).ToList();
                    }
                    if (__results != null)
                    {
                        for (int i = 0; i < __results.Count; i++)
                        {
                            __results[i].PrimaryContact = false;
                        }
                        db.SaveChanges();
                    }
                }
                result.Id = crmLeadContactDto.Id;
                result.LeadId = crmLeadContactDto.LeadId;
                result.customer_id = crmLeadContactDto.customer_id;
                result.FirstName = crmLeadContactDto.FirstName;
                result.LastName = crmLeadContactDto.LastName;
                result.Mobile = crmLeadContactDto.Mobile;
                result.Email = crmLeadContactDto.Email;
                result.Designation = crmLeadContactDto.Designation;
                result.Location = crmLeadContactDto.Location;
                result.PrimaryContact = crmLeadContactDto.PrimaryContact;



                db.CrmLeadContact.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("updateassignleadowners")]
        public IActionResult GetCRMAllLeads([FromBody] AssignClass obj)
        {
            if (obj == null || obj.Leads == null || obj.AssignTo <= 0)
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                var leadsToUpdate = db.crmleads.Select(x => new crmleads {
                
                AssignTo=x.AssignTo,
                id=x.id,
                lead_owner=x.lead_owner,
                business_email=x.business_email,
                first_name=x.first_name,
                company=x.company,
                branch_id=x.branch_id,
                customer_code=x.customer_code,
                last_name=x.last_name,
                lead_group=x.lead_group,
                lead_source=x.lead_source,
                lead_stage=x.lead_stage,
                lead_status=x.lead_status,
                description=x.description,
                alternate_number=x.alternate_number,
                website=x.website,
                phonenumber=x.phonenumber,
                industry=x.industry,
                city=x.city,
                state=x.state,
                street=x.street,
                country=x.country,
                emailoutputformat=x.emailoutputformat,
                annual_revenue=x.annual_revenue,
                numberofemployees=x.numberofemployees,


                }).Where(l => l.id == obj.Leads[0]).ToList();

                if (!leadsToUpdate.Any())
                {
                    return NotFound(new { message = "No matching leads found for the provided IDs." });
                }

                foreach (var lead in leadsToUpdate)
                {
                    lead.AssignTo = obj.AssignTo;
                    lead.lead_owner = obj.AssignTo;
                    lead.branch_id = lead.branch_id;
                    lead.customer_code = lead.customer_code;
                    lead.lead_group = lead.lead_group;
                    lead.lead_owner = lead.lead_owner;
                    lead.lead_source = lead.lead_source;
                    lead.lead_stage = lead.lead_stage;
                    lead.lead_status = lead.lead_status;
                    lead.first_name = lead.first_name;
                    lead.last_name = lead.last_name;
                    lead.description = lead.description;
                    lead.business_email = lead.business_email;
                    lead.secondary_email = lead.secondary_email;
                    lead.phonenumber = lead.phonenumber;
                    lead.alternate_number = lead.alternate_number;
                    lead.website = lead.website;
                    lead.industry = lead.industry;
                    lead.numberofemployees = lead.numberofemployees;
                    lead.annual_revenue = lead.annual_revenue;
                    lead.rating = lead.rating;
                    lead.emailoutputformat = lead.emailoutputformat;
                    lead.title = lead.title;
                    lead.city = lead.city;
                    lead.street = lead.street;
                    lead.state = lead.state;
                    lead.country = lead.country;
                    db.crmleads.Update(lead);
                    db.SaveChanges();
                    var result = db.HrdEmployees.Where(x => x.RecordId == lead.AssignTo).FirstOrDefault();
                    if (result != null)
                    {
                        sendEmail sendEmail = new sendEmail();
                        sendEmail.EmailSend("Assigned Leads", result.Email, "Hi " + lead.first_name + ",\n" + "You have assigned on the lead for the Company Name " + ":" + lead.company, null);

                    }
                }

               // db.SaveChanges();
               
                return Ok(new { status = true, message = "Updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("Getcrmleadcontact")]
        public IActionResult Getcrmleadcontact([FromBody] dynamic model)
        {
            crmLeadContactDto crmLeadContactDto = JsonConvert.DeserializeObject<crmLeadContactDto>(model.ToString());

            if (crmLeadContactDto.customer_id > 0)
            {
                var leadcontact = db.CrmLeadContact.Select(c => new crmLeadContactDto
                {
                    Id = c.Id,
                    LeadId = c.LeadId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Mobile = c.Mobile,
                    Email = c.Email,
                    Designation = c.Designation,
                    Location = c.Location,
                    BranchId = c.BranchId,
                    CustomerCode = c.CustomerCode,
                    customer_id = c.customer_id,
                    PrimaryContact = c.PrimaryContact
                    ,primaryconactName=c.PrimaryContact==true?"Yes":"No"
                    //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

                }).Where(x => x.BranchId == crmLeadContactDto.BranchId && x.CustomerCode == crmLeadContactDto.CustomerCode && x.customer_id == crmLeadContactDto.customer_id).ToList();
                return Ok(leadcontact);
            }
            else
            {
                var leadcontact = db.CrmLeadContact.Where(x => x.BranchId == crmLeadContactDto.BranchId && x.CustomerCode == crmLeadContactDto.CustomerCode && x.LeadId == crmLeadContactDto.LeadId).Select(c => new crmLeadContactDto
                {
                    Id = c.Id,
                   // LeadId = c.LeadId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Mobile = c.Mobile,
                    Email = c.Email,
                    Designation = c.Designation,
                    Location = c.Location,
                    PrimaryContact=c.PrimaryContact,
                    primaryconactName=c.PrimaryContact==true?"Yes":"No"
                    //BranchId = c.BranchId,
                    //CustomerCode = c.CustomerCode,
                    //customer_id = c.customer_id
                    //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

                }).ToList();
                return Ok(leadcontact);
            }


        }

        [HttpPost]
        [Authorize]
        [Route("GetcrmleadcontactById")]
        public IActionResult GetcrmleadcontactById([FromBody] dynamic model)
        {
            crmLeadContactDto crmLeadContactDto = JsonConvert.DeserializeObject<crmLeadContactDto>(model.ToString());

            var contactlist = db.CrmLeadContact.Where(y => y.Id == crmLeadContactDto.Id).Select(contactlist => new crmLeadContactDto
            {
                Id = contactlist.Id,
                //LeadId= crmLeadContactDto.LeadId,
                FirstName = contactlist.FirstName,
                LastName = contactlist.LastName,
                Mobile = contactlist.Mobile,
                Email = contactlist.Email,
                Designation = contactlist.Designation,
                Location = contactlist.Location,
                PrimaryContact = contactlist.PrimaryContact,

            }).FirstOrDefault();

            if (contactlist == null)
            {
                return NotFound("Record not found.");
            }

            return Ok(contactlist);
        }
        //[HttpPost]
        //[Authorize]
        //[Route("GetcrmleadcontactDeleteById")]
        //public IActionResult GetcrmleadcontactDelete([FromBody] int id)
        //{

        //    var contacts = db.CrmLeadContact.FirstOrDefault(y => y.Id == id);

        //    if (contacts == null)
        //    {

        //        return NotFound(new { message = "enquiry register not found." });
        //    }


        //    db.CrmLeadContact.Remove(contacts);

        //    db.SaveChanges();

        //    return Ok(new { message = "enquiry register deleted successfully." });
        //}
        [HttpPost]
        [Authorize]
        [Route("Savecrmleadorders")]
        public IActionResult Savecrmleadorders([FromBody] dynamic model)
        {
            crmleadcustSaleOrderDetdto crmleadcustSaleOrderDetdto = JsonConvert.DeserializeObject<crmleadcustSaleOrderDetdto>(model.ToString());
            if (crmleadcustSaleOrderDetdto != null)
            {
                crmleadcustSaleOrderDet crmleadcustSaleOrderDet = new crmleadcustSaleOrderDet();
                //crmleadcustSaleOrderDet.recordId = crmleadcustSaleOrderDetdto.recordId;
                crmleadcustSaleOrderDet.sno = crmleadcustSaleOrderDetdto.sno;
                crmleadcustSaleOrderDet.itemId = crmleadcustSaleOrderDetdto.itemId;
                crmleadcustSaleOrderDet.itemName = crmleadcustSaleOrderDetdto.itemName;
                crmleadcustSaleOrderDet.itemDescription = crmleadcustSaleOrderDetdto.itemDescription;
                crmleadcustSaleOrderDet.qty = crmleadcustSaleOrderDetdto.qty;
                crmleadcustSaleOrderDet.um = crmleadcustSaleOrderDetdto.um;
                crmleadcustSaleOrderDet.rat = crmleadcustSaleOrderDetdto.rat;
                crmleadcustSaleOrderDet.discPer = crmleadcustSaleOrderDetdto.discPer;
                crmleadcustSaleOrderDet.reqdBy = crmleadcustSaleOrderDetdto.reqdBy;
                crmleadcustSaleOrderDet.branchId = crmleadcustSaleOrderDetdto.branchId;
                crmleadcustSaleOrderDet.customerCode = crmleadcustSaleOrderDetdto.customerCode;
                crmleadcustSaleOrderDet.lead_id = crmleadcustSaleOrderDetdto.lead_id;
                crmleadcustSaleOrderDet.customer_id = crmleadcustSaleOrderDetdto.customer_id;
                crmleadcustSaleOrderDet.delivery_date = crmleadcustSaleOrderDetdto.delivery_date;
                crmleadcustSaleOrderDet.order_description = crmleadcustSaleOrderDetdto.order_description;
                crmleadcustSaleOrderDet.order_status = crmleadcustSaleOrderDetdto.order_status;
                crmleadcustSaleOrderDet.mode_of_payment = crmleadcustSaleOrderDetdto.mode_of_payment;

                db.crmleadcustSaleOrderDets.Add(crmleadcustSaleOrderDet);
                db.SaveChanges();


            }
            return Ok();

        }
        [HttpPost]
        [Authorize]
        [Route("Getcrmleadorder")]
        public IActionResult Getcrmleadorder([FromBody] dynamic model)
        {
            crmleadcustSaleOrderDetdto crmleadcustSaleOrderDetdto = JsonConvert.DeserializeObject<crmleadcustSaleOrderDetdto>(model.ToString());
            if (crmleadcustSaleOrderDetdto.customer_id > 0)
            {
                var leadcontact = db.crmleadcustSaleOrderDets.Where(x => x.branchId == crmleadcustSaleOrderDetdto.branchId && x.customerCode == crmleadcustSaleOrderDetdto.customerCode && x.customer_id == crmleadcustSaleOrderDetdto.customer_id).Select(c => new crmleadcustSaleOrderDetdto
                {
                    branchId = c.branchId,
                    customerCode = c.customerCode,
                    lead_id = c.lead_id,
                    recordId = c.recordId,
                    sno = c.sno,
                    itemId = c.itemId,

                    itemDescription = c.itemDescription,
                    qty = c.qty,
                    um = c.um,
                    rat = c.rat,
                    discPer = c.discPer,
                    reqdBy = c.reqdBy,
                    customer_id = c.customer_id,
                    mode_of_payment = c.mode_of_payment,
                    order_status = c.order_status,
                    order_description = c.order_description,
                    delivery_date = c.delivery_date,
                    itemName = c.itemId > 0 ? db.InvMaterials.Where(x => x.RecordId == c.itemId).FirstOrDefault().ItemName : null
                }).ToList();

                return Ok(leadcontact);
            }
            else
            {
                var leadcontact = db.crmleadcustSaleOrderDets.Where(x => x.branchId == crmleadcustSaleOrderDetdto.branchId && x.customerCode == crmleadcustSaleOrderDetdto.customerCode && x.lead_id == crmleadcustSaleOrderDetdto.lead_id).Select(c => new crmleadcustSaleOrderDetdto
                {
                    branchId = c.branchId,
                    customerCode = c.customerCode,
                    lead_id = c.lead_id,
                    recordId = c.recordId,
                    sno = c.sno,
                    itemId = c.itemId,

                    itemDescription = c.itemDescription,
                    qty = c.qty,
                    um = c.um,
                    rat = c.rat,
                    discPer = c.discPer,
                    reqdBy = c.reqdBy,
                    customer_id = c.customer_id,
                    mode_of_payment = c.mode_of_payment,
                    order_status = c.order_status,
                    order_description = c.order_description,
                    delivery_date = c.delivery_date,
                    itemName = c.itemId > 0 ? db.InvMaterials.Where(x => x.RecordId == c.itemId).FirstOrDefault().ItemName : null
                }).ToList();

                return Ok(leadcontact);
            }

        }
        [HttpPost]
        [Route("SaveCrmLeadCustEnquiryLineItem")]
        public IActionResult SaveCrmLeadCustEnquiryLineItem([FromBody] dynamic model)
        {

            CrmLeadCustEnquiryLineItemdto crmLeadCustEnquiryLineItemdto = JsonConvert.DeserializeObject<CrmLeadCustEnquiryLineItemdto>(model.ToString());

            CrmLeadCustEnquiryLineItems enquiryLineItem = new CrmLeadCustEnquiryLineItems
            {


                sno = crmLeadCustEnquiryLineItemdto.sno,
                ItemId = crmLeadCustEnquiryLineItemdto.ItemId,
                ItemName = crmLeadCustEnquiryLineItemdto.ItemName,
                ItemDescription = crmLeadCustEnquiryLineItemdto.ItemDescription,
                qty = crmLeadCustEnquiryLineItemdto.qty,
                um = crmLeadCustEnquiryLineItemdto.um,
                rate = crmLeadCustEnquiryLineItemdto.rate,
                discount = crmLeadCustEnquiryLineItemdto.discount,
                leaddays = crmLeadCustEnquiryLineItemdto.leaddays,
                branchid = crmLeadCustEnquiryLineItemdto.branchid,
                customercode = crmLeadCustEnquiryLineItemdto.customercode,
                lead_id = crmLeadCustEnquiryLineItemdto.lead_id,
                customer_id = crmLeadCustEnquiryLineItemdto.customer_id,
                value = crmLeadCustEnquiryLineItemdto.value,
            };
            db.CrmLeadCustEnquiryLineItem.Add(enquiryLineItem);
            db.SaveChanges();

            return Ok(new { message = "Enquiry Line Item saved successfully." });



        }

        [HttpPost]

        [Route("GetCrmLeadCustEnquiryLineItem")]
        public IActionResult GetCrmLeadCustEnquiryLineItem([FromBody] dynamic model)
        {
            CrmLeadCustEnquiryLineItemdto crmLeadCustEnquiryLineItemdto = JsonConvert.DeserializeObject<CrmLeadCustEnquiryLineItemdto>(model.ToString());


            var enquiryLineItems = db.CrmLeadCustEnquiryLineItem
                .Where(c => c.branchid == crmLeadCustEnquiryLineItemdto.branchid
                            && c.customercode == crmLeadCustEnquiryLineItemdto.customercode
                            && c.lead_id == crmLeadCustEnquiryLineItemdto.lead_id)
                .Select(c => new CrmLeadCustEnquiryLineItemdto
                {
                    RecordId = c.RecordId,
                    sno = c.sno,
                    ItemId = c.ItemId,

                    ItemDescription = c.ItemDescription,
                    qty = c.qty,
                    um = c.um,
                    rate = c.rate,
                    leaddays = c.leaddays,
                    branchid = c.branchid,
                    customercode = c.customercode,
                    lead_id = c.lead_id,
                    customer_id = c.customer_id,
                    value = c.value,
                    discount = c.discount,
                    leaditemname = c.ItemId > 0 ? db.InvMaterials.Where(x => x.RecordId == c.ItemId).FirstOrDefault().ItemName : null,


                })
                .ToList();

            return Ok(enquiryLineItems);
        }
        [HttpPost]
        [Authorize]
        [Route("Savecrmleadquotation")]
        public IActionResult Savecrmleadquotation([FromBody] dynamic model)
        {
            crmLeadQuotationsDto crmLeadQuotationsDto = JsonConvert.DeserializeObject<crmLeadQuotationsDto>(model.ToString());
            if (crmLeadQuotationsDto != null)
            {
                crmLeadQuotations crmLeadQuotations = new crmLeadQuotations();

                crmLeadQuotations.LeadId = crmLeadQuotationsDto.LeadId;

                crmLeadQuotations.ItemName = crmLeadQuotationsDto.ItemName;
                crmLeadQuotations.ItemDescription = crmLeadQuotationsDto.ItemDescription;
                crmLeadQuotations.Qty = crmLeadQuotationsDto.Qty;
                crmLeadQuotations.Um = crmLeadQuotationsDto.Um;
                crmLeadQuotations.LeadDays = crmLeadQuotationsDto.LeadDays;
                crmLeadQuotations.Rate = crmLeadQuotationsDto.Rate;
                crmLeadQuotations.Disper = crmLeadQuotationsDto.Disper;
                crmLeadQuotations.Tax = crmLeadQuotationsDto.Tax;
                crmLeadQuotations.BaseAmt = crmLeadQuotationsDto.BaseAmt;
                crmLeadQuotations.Discount = crmLeadQuotationsDto.Discount;
                crmLeadQuotations.Taxes = crmLeadQuotationsDto.Taxes;
                crmLeadQuotations.Others = crmLeadQuotationsDto.Others;
                crmLeadQuotations.TotalAmt = crmLeadQuotationsDto.TotalAmt;
                crmLeadQuotations.BranchId = crmLeadQuotationsDto.BranchId;
                crmLeadQuotations.CustomerCode = crmLeadQuotationsDto.CustomerCode;
                crmLeadQuotations.customer_id = crmLeadQuotationsDto.customer_id;
                db.CrmLeadQuotations.Add(crmLeadQuotations);
                db.SaveChanges();
            }
            return Ok();

        }

        [HttpPost]
        [Authorize]
        [Route("GetcrmleadQuotation")]
        public IActionResult GetcrmleadQuotation([FromBody] dynamic model)
        {
            crmLeadQuotationsDto crmLeadQuotationsDto = JsonConvert.DeserializeObject<crmLeadQuotationsDto>(model.ToString());
            if (crmLeadQuotationsDto.customer_id > 0)
            {
                var leadquotation = db.CrmLeadQuotations.Where(x => x.BranchId == crmLeadQuotationsDto.BranchId && x.CustomerCode == crmLeadQuotationsDto.CustomerCode && x.customer_id == crmLeadQuotationsDto.customer_id).Select(c => new crmLeadQuotationsDto
                {
                    Id = c.Id,
                    LeadId = c.LeadId,
                    ItemName = c.ItemName,
                    ItemDescription = c.ItemDescription,
                    Discount = c.Discount,
                    BaseAmt = c.BaseAmt,
                    TotalAmt = c.TotalAmt,
                    Tax = c.Tax,
                    Taxes = c.Taxes,
                    Others = c.Others,
                    Qty = c.Qty,
                    Um = c.Um,
                    leadquotename = c.ItemName > 0 ? db.InvMaterials.Where(x => x.RecordId == c.ItemName).FirstOrDefault().ItemName : null,
                    customer_id = c.customer_id,
                    //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

                }).ToList();

                return Ok(leadquotation);
            }
            else
            {
                var leadquotation = db.CrmLeadQuotations.Where(x => x.BranchId == crmLeadQuotationsDto.BranchId && x.CustomerCode == crmLeadQuotationsDto.CustomerCode && x.LeadId == crmLeadQuotationsDto.LeadId).Select(c => new crmLeadQuotationsDto
                {
                    Id = c.Id,
                    LeadId = c.LeadId,
                    ItemName = c.ItemName,
                    ItemDescription = c.ItemDescription,
                    Discount = c.Discount,
                    BaseAmt = c.BaseAmt,
                    TotalAmt = c.TotalAmt,
                    Tax = c.Tax,
                    Taxes = c.Taxes,
                    Others = c.Others,
                    Qty = c.Qty,
                    Um = c.Um,
                    leadquotename = c.ItemName > 0 ? db.InvMaterials.Where(x => x.RecordId == c.ItemName).FirstOrDefault().ItemName : null,
                    customer_id = c.customer_id,
                    //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

                }).ToList();

                return Ok(leadquotation);
            }

        }


        //[HttpPost]
        //[Authorize]
        //[Route("api/GetcrmleadQuotation")]
        //public IActionResult GetcrmleadQuotation([FromBody] dynamic model)
        //{
        //    crmLeadQuotationsDto crmLeadQuotationsDto = JsonConvert.DeserializeObject<crmLeadQuotationsDto>(model.ToString());

        //    var quotations = db.CrmLeadQuotations.Select(q => new crmLeadQuotationsDto
        //    {
        //        Id = q.Id,
        //        LeadId = q.LeadId,
        //        ItemName = q.ItemName,

        //        ItemDescription = q.ItemDescription,
        //        Qty = q.Qty,
        //        Um = q.Um,
        //        LeadDays = q.LeadDays,
        //        Rate = q.Rate,
        //        Disper = q.Disper,
        //        Tax = q.Tax,
        //        BaseAmt = q.BaseAmt,
        //        Discount = q.Discount,
        //        Taxes = q.Taxes,
        //        Others = q.Others,
        //        TotalAmt = q.TotalAmt,
        //        BranchId = q.BranchId,
        //        CustomerCode = q.CustomerCode
        //        //CrmprogroupName = c.crmprogroupId != null ? db.crmprogroup.Where(g => g.RecordId == c.crmprogroupId).Select(g => g.groupName).FirstOrDefault() : null,

        //    }).Where(x => x.BranchId == crmLeadQuotationsDto.BranchId && x.CustomerCode == crmLeadQuotationsDto.CustomerCode == crmLeadQuotationsDto).ToList();

        //    return Ok(quotations);

        //}

        [HttpPost]
        [Authorize]
        [Route("UpdatecrmleadQuotation")]
        public IActionResult UpdatecrmleadQuotation([FromBody] dynamic model)
        {
            crmLeadQuotationsDto crmLeadQuotationsDto = JsonConvert.DeserializeObject<crmLeadQuotationsDto>(model.ToString());
            var result = db.CrmLeadQuotations.Where(y => y.Id == crmLeadQuotationsDto.Id).FirstOrDefault();
            if (result != null)
            {

                result.LeadId = crmLeadQuotationsDto.LeadId;
                result.Id = crmLeadQuotationsDto.Id;
                result.ItemName = crmLeadQuotationsDto.ItemName;
                result.ItemDescription = crmLeadQuotationsDto.ItemDescription;
                result.Qty = crmLeadQuotationsDto.Qty;
                result.Um = crmLeadQuotationsDto.Um;
                result.LeadDays = crmLeadQuotationsDto.LeadDays;
                result.Rate = crmLeadQuotationsDto.Rate;
                result.Disper = crmLeadQuotationsDto.Disper;
                result.Tax = crmLeadQuotationsDto.Tax;
                result.BaseAmt = crmLeadQuotationsDto.BaseAmt;
                result.Discount = crmLeadQuotationsDto.Discount;
                result.Taxes = crmLeadQuotationsDto.Taxes;
                result.Others = crmLeadQuotationsDto.Others;
                result.TotalAmt = crmLeadQuotationsDto.TotalAmt;

                result.customer_id = crmLeadQuotationsDto.customer_id;


                db.CrmLeadQuotations.Update(result);
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("GetcrmleadquotationById")]
        public IActionResult GetcrmleadquotationById([FromBody] dynamic model)
        {
            crmLeadQuotationsDto crmLeadQuotationsDto = JsonConvert.DeserializeObject<crmLeadQuotationsDto>(model.ToString());

            var quotationlist = db.CrmLeadQuotations.Where(y => y.Id == crmLeadQuotationsDto.Id).Select(list => new
            {

                list.Id,
                list.LeadId,
                list.ItemName,
                list.ItemDescription,
                list.Qty,
                list.Um,
                list.Discount,
                list.Tax,
                list.BaseAmt,
                list.Taxes,
                list.Others,
                list.TotalAmt,



            }).FirstOrDefault();
            return Ok(quotationlist);
        }
        [HttpPost]
        [Authorize]
        [Route("GetcrmleadQuotationDeleteById")]
        public IActionResult GetcrmleadQuotationDeleteById([FromBody] dynamic model)
        {

            var input = JsonConvert.DeserializeObject<dynamic>(model.ToString());
            int id = input.Id;
            string branchId = input.BranchId;
            int customerCode = input.CustomerCode;


            var quotation = db.CrmLeadQuotations
                .Where(c => c.Id == id && c.BranchId == branchId && c.CustomerCode == customerCode)
                .FirstOrDefault();

            if (quotation == null)
            {
                return NotFound(new { message = "CRM lead contact not found." });
            }

            db.CrmLeadQuotations.Remove(quotation);
            db.SaveChanges();

            return Ok(new { message = "CRM lead contact deleted successfully." });
        }
        //telecalling details
        [HttpPost]
        [Authorize]
        [Route("Savecrmtelecalldetails")]
        public IActionResult Savecrmtelecalldetails([FromBody] dynamic model)
        {
            crmtelecallleadcustdto crmtelecallleadcustdto = JsonConvert.DeserializeObject<crmtelecallleadcustdto>(model.ToString());
            if (crmtelecallleadcustdto != null)
            {
                crmtelecallleadcust crmtelecallleadcust = new crmtelecallleadcust();
                crmtelecallleadcust.customer_id = crmtelecallleadcustdto.customer_id;
                crmtelecallleadcust.lead_Id = crmtelecallleadcustdto.lead_Id;
                crmtelecallleadcust.customercode = crmtelecallleadcustdto.customercode;
                crmtelecallleadcust.branchid = crmtelecallleadcustdto.branchid;
                crmtelecallleadcust.createdAt = DateTime.Now;
                db.crmtelecallleadcust.Add(crmtelecallleadcust);
                db.SaveChanges();

                var result = db.CrmLeadContact.Where(x => x.Id == crmtelecallleadcustdto.contactid).FirstOrDefault();
                CrmCallLogs crmCallLogs = new CrmCallLogs();
                crmCallLogs.branchid = crmtelecallleadcustdto.branchid;
                crmCallLogs.customer_id = crmtelecallleadcustdto.customer_id;
                crmCallLogs.customercode = crmtelecallleadcustdto.customercode;
                crmCallLogs.leadid = crmtelecallleadcustdto.lead_Id;
                crmCallLogs.callernotes = crmtelecallleadcustdto.callernotes;
                crmCallLogs.customernotes= crmtelecallleadcustdto.customernotes;
                crmCallLogs.reasonforcall = crmtelecallleadcustdto.reasonforcall;
                crmCallLogs.CallTypes = crmtelecallleadcustdto.calltypes;
                crmCallLogs.LeadOwnerId = crmtelecallleadcustdto.lead_Id;
                crmCallLogs.ContactId = result.FirstName + result.LastName;
                crmCallLogs.CreatedAt = DateTime.Now;
                db.CrmCallLogs.Add(crmCallLogs);
                db.SaveChanges();


            }
            return Ok();

        }
        [HttpPost]
        [Authorize]
        [Route("getcrmtelecalldetails")]
        public IActionResult getcrmtelecalldetails([FromBody] dynamic model)
        {
            crmtelecallleadcustdto crmtelecallleadcustdto = JsonConvert.DeserializeObject<crmtelecallleadcustdto>(model.ToString());
            if (crmtelecallleadcustdto != null)
            {
                var result = db.crmtelecallleadcust.Where(x => x.branchid == crmtelecallleadcustdto.branchid && x.customercode == crmtelecallleadcustdto.customercode).
                      Select(x => new crmtelecallleadcustdto
                      {
                          customer_id=x.customer_id,
                          customercode=x.customercode,
                          branchid=x.branchid,
                          lead_Id=x.lead_Id
                      }).ToList();
                return Ok(result);
            }
            return null;
            

        }
        [HttpPost]
        [Authorize]
        [Route("GetLeadsToCustomer")]
        public IActionResult GetLeadsToCustomer([FromBody] dynamic model)
        {
            try
            {
                // Deserialize the input model for lead
                crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());
                Console.WriteLine($"Searching for lead with ID: {leadDto.id}");

                if (leadDto == null)
                {
                    Console.WriteLine("Error: Deserialized leadDto is null.");
                    return BadRequest("Invalid input data.");
                }

                // Check for a matching lead record in the database
                var result = db.crmleads.Select(x => new {x.id,x.alternate_number,x.code,x.customer,x.branch_id,x.customer_code,
                    x.lead_group,x.lead_owner,x.lead_source,x.company,x.status,x.first_name,x.last_name,x.description,x.business_email
                    ,x.secondary_email,x.phone,x.phonenumber,x.lead_status,x.lead_stage,x.website,x.industry,x.numberofemployees,
                    x.annual_revenue,x.rating,x.emailoutputformat,x.skypeid,x.city,x.state,x.country,x.customer_id,x.zipcode,x.street
                    ,x.title
                
                }).Where(x => x.id == leadDto.id).FirstOrDefault();
                int newCustomerId = 0;

                if (result != null)
                {
                    // Create a new customer entry based on the lead
                    crmleads newCustomer = new crmleads
                    {
                        alternate_number = result.alternate_number,
                        type_lead_cus = "Cus",
                        code = result.code,
                        customer = result.customer,
                        branch_id = result.branch_id,
                        customer_code = result.customer_code,
                        lead_group = result.lead_group,
                        status = result.status,
                        lead_owner = result.lead_owner,
                        company = result.company,
                        first_name = result.first_name,
                        last_name = result.last_name,
                        description = result.description,
                        business_email = result.business_email,
                        secondary_email = result.secondary_email,
                        phonenumber = result.phonenumber,
                        lead_status = result.lead_status,
                        lead_source = result.lead_source,
                        lead_stage = result.lead_stage,
                        website = result.website,
                        industry = result.industry,
                        numberofemployees = result.numberofemployees,
                        annual_revenue = result.annual_revenue,
                        rating = result.rating,
                        emailoutputformat = result.emailoutputformat,
                        skypeid = result.skypeid,
                        city = result.city,
                        state = result.state,
                        country = result.country,
                        phone = result.phone,
                        customer_id = result.customer_id,
                        zipcode = result.zipcode,
                        street = result.street,
                        title = result.title,
                        
                    };
                    newCustomer.created_at = DateTime.Now;
                    newCustomer.converted_customer = "Yes";
                    // Save the new customer to the database
                    db.crmleads.Add(newCustomer);
                    db.SaveChanges();
                    newCustomerId = newCustomer.id;
                    //var resultupdate= db.crmleads.Select(x=>new crmleads
                    //{
                    //   referencecustomer_id= x.referencecustomer_id,
                    //    id=x.id
                    //}).Where(x => x.id == leadDto.id).FirstOrDefault();
                    
                    //resultupdate.referencecustomer_id= newCustomerId;
                    //db.crmleads.Update(resultupdate);
                    //db.SaveChanges();
                    Console.WriteLine($"New customer created with ID: {newCustomerId}");
                }
                else
                {
                    Console.WriteLine($"No lead found with ID: {leadDto.id}");
                    return NotFound("No leads found for this customer.");
                }
                var callcontactResults = db.CrmLeadContact.Where(x => x.LeadId == leadDto.id).ToList();
                foreach(var existingCallcontact in callcontactResults)
                {
                    crmLeadContact crmLeadContact = new crmLeadContact();
                    crmLeadContact.customer_id = newCustomerId;
                    crmLeadContact.FirstName = existingCallcontact.FirstName;
                    crmLeadContact.LastName = existingCallcontact.LastName;
                    crmLeadContact.BranchId = existingCallcontact.BranchId;
                    crmLeadContact.CustomerCode = existingCallcontact.CustomerCode;
                    crmLeadContact.CreatedAt = DateTime.Now;
                    crmLeadContact.Designation = existingCallcontact.Designation;
                    crmLeadContact.Email = existingCallcontact.Email;
                    crmLeadContact.Mobile = existingCallcontact.Mobile;
                    crmLeadContact.Location = existingCallcontact.Location;
                    db.CrmLeadContact.Add(crmLeadContact);
                    db.SaveChanges();
                }
                // Process Call Logs
                var callLogsResults = db.CrmCallLogs.Where(x => x.leadid == leadDto.id).ToList();
                foreach (var existingCallLog in callLogsResults)
                {
                    CrmCallLogs newCallLog = new CrmCallLogs
                    {
                        leadid = existingCallLog.leadid,
                        customer_id = newCustomerId, // Link to the new customer
                        ContactId = existingCallLog.ContactId,
                        LeadOwnerId = existingCallLog.LeadOwnerId,
                        CallTypes = existingCallLog.CallTypes,
                        CallDate = existingCallLog.CallDate,
                        Comments = existingCallLog.Comments,
                        branchid = existingCallLog.branchid,
                        customercode = existingCallLog.customercode,
                        callernotes = existingCallLog.callernotes,
                        customernotes = existingCallLog.customernotes,
                        reasonforcall = existingCallLog.reasonforcall,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = existingCallLog.CreatedBy,
                        ModifiedAt = existingCallLog.ModifiedAt,
                        ModifiedBy = existingCallLog.ModifiedBy
                    };

                    db.CrmCallLogs.Add(newCallLog);
                    db.SaveChanges();
                    Console.WriteLine($"Processed {callLogsResults.Count} call logs.");
                }
                var eventsResults = db.CrmLeadsEvents.Where(x => x.leadid == leadDto.id).ToList();
                foreach (var leadeventresult in eventsResults)
                {
                    CrmLeadsEvent crmLeadsEvent = new CrmLeadsEvent
                    {
                        customercode= leadeventresult.customercode,
                        branchid=leadeventresult.branchid,
                        customer_id=newCustomerId,
                        EventTitle=leadeventresult.EventTitle,
                        EventGuests = leadeventresult.EventGuests,
                        EventTime = leadeventresult.EventTime,
                        MeetingLink=leadeventresult.MeetingLink,
                        MeetingLocation=leadeventresult.MeetingLocation
                        ,CreatedAt=DateTime.Now,
                      

                    };

                    db.CrmLeadsEvents.Add(crmLeadsEvent);
                    db.SaveChanges();
                   // Console.WriteLine($"Processed {leadeventresult.Count} reminders.");
                }
                // Process Reminders
                var reminderResults = db.CrmRemainders.Where(x => x.leadid == leadDto.id).ToList();
                foreach (var existingReminder in reminderResults)
                {
                    CrmRemainder newReminder = new CrmRemainder
                    {
                        leadid = existingReminder.leadid,
                        customer_id = newCustomerId, // Link to the new customer
                        ReminderName = existingReminder.ReminderName,
                        ReminderDate = existingReminder.ReminderDate,
                        ReminderTime = existingReminder.ReminderTime,
                        reminder_type = existingReminder.reminder_type,
                        Notes = existingReminder.Notes,
                        BranchId = existingReminder.BranchId,
                        CustomerCode = existingReminder.CustomerCode,
                        CreatedBy = existingReminder.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedBy = existingReminder.ModifiedBy,
                        ModifiedDate = existingReminder.ModifiedDate
                    };

                    db.CrmRemainders.Add(newReminder);
                    db.SaveChanges();
                    Console.WriteLine($"Processed {reminderResults.Count} reminders.");
                }
                int enquiryid = 0;
                //enquiries
                var enquiryResults = db.CrmLeadEnquiryDetails.Where(x => x.LeadId == leadDto.id).ToList();
                foreach (var enquiry in enquiryResults)
                {
                    CrmLeadEnquiryDetail crmLeadEnquiryDetail = new CrmLeadEnquiryDetail
                    {
                       
                        customer_id=newCustomerId,
                        ContactName=enquiry.ContactName,
                        ContactId = enquiry.ContactId,
                        EnquiryDate = enquiry.EnquiryDate,
                        Comments = enquiry.Comments,
                        FollowupDate = enquiry.FollowupDate,
                        AdditionalCharges = enquiry.AdditionalCharges,
                        GrandTotal = enquiry.GrandTotal,
                        Subtotal = enquiry.Subtotal,
                        CustomerCode = enquiry.CustomerCode,
                        BranchCode=enquiry.BranchCode,

                    };
                    db.CrmLeadEnquiryDetails.Add(crmLeadEnquiryDetail);
                    db.SaveChanges();
                    enquiryid=crmLeadEnquiryDetail.EnquiryId;
                    var subenquiry=db.CrmLeadEnquiryLineDetails.Where(x=>x.EnquiryId==enquiryid).ToList();
                    foreach (var item in subenquiry)
                    {
                        CrmLeadEnquiryLineDetail crmLeadEnquiryLineDetail = new CrmLeadEnquiryLineDetail
                        {

                            ItemId = item.ItemId,
                            ItemCode = item.ItemCode,
                            ItemName=item.ItemName,
                            Qty = item.Qty,
                            UnitPrice=item.UnitPrice,
                            Discount=item.Discount,
                            DiscountAmount=item.DiscountAmount,
                            UomId=item.UomId,
                            UomName=item.UomName,
                            LeadDays=item.LeadDays,
                            TotalAmt=item.TotalAmt,
                            TotalPrice=item.TotalPrice,
                            CustomerCode=item.CustomerCode,
                            BranchCode=item.BranchCode,
                            


                        };
                        db.CrmLeadEnquiryLineDetails.Add(crmLeadEnquiryLineDetail);
                        db.SaveChanges();
                        
                    }
                }
                //quotation
                //enquiries
                int quotationid = 0;
                var quotationResults = db.CrmLeadQuotationDetails.Where(x => x.LeadId == leadDto.id).ToList();
                foreach (var quotationDetail in quotationResults)
                {
                    CrmLeadQuotationDetail crmLeadQuotationDetail = new CrmLeadQuotationDetail
                    {

                        customer_id = newCustomerId,
                       ContactId=quotationDetail.ContactId,
                        ContactName = quotationDetail.ContactName,
                        QuotationDate = quotationDetail.QuotationDate,
                        Comments = quotationDetail.Comments,
                        QuotationValidity = quotationDetail.QuotationValidity,
                        Terms = quotationDetail.Terms,
                        DeliveryTerms = quotationDetail.DeliveryTerms,
                        BillingAddress = quotationDetail.BillingAddress,
                        ShippingAddress = quotationDetail.ShippingAddress,
                        AdditionalCharges = quotationDetail.AdditionalCharges,
                        FreightCharge = quotationDetail.FreightCharge,
                        CustomerCode = quotationDetail.CustomerCode,
                        CustomsDuties = quotationDetail.CustomsDuties,
                        GrandTotal = quotationDetail.GrandTotal,
                        Subtotal = quotationDetail.Subtotal,
                        BranchCode = quotationDetail.BranchCode,

                    };
                    db.CrmLeadQuotationDetails.Add(crmLeadQuotationDetail);
                    db.SaveChanges();
                    quotationid = crmLeadQuotationDetail.QuotationId;
                    var subquotation = db.CrmLeadQuotationLinesDetails.Where(x => x.QuotationId == quotationid).ToList();
                    foreach (var item in subquotation)
                    {
                        CrmLeadQuotationLineDetail crmLeadQuotationLineDetail = new CrmLeadQuotationLineDetail
                        {

                           QuotationId=item.QuotationId,
                           ItemId=item.ItemId,
                           ItemCode=item.ItemCode,
                           ItemName=item.ItemName,
                           Qty=item.Qty,
                           UnitPrice=item.UnitPrice,
                           Discount=item.Discount,
                           DiscountAmount=item.DiscountAmount,
                           UomId=item.UomId,
                           UomName=item.UomName,
                           LeadDays=item.LeadDays,
                           TotalAmt=item.TotalAmt,
                           TotalPrice=item.TotalPrice,
                           TaxId=item.TaxId,
                           TaxAmount=item.TaxAmount,
                           TaxName=item.TaxName,
                           TaxPercentage=item.TaxPercentage,
                           CustomerCode=item.CustomerCode,
                           BranchCode=item.BranchCode

                        };
                        db.CrmLeadQuotationLinesDetails.Add(crmLeadQuotationLineDetail);
                        db.SaveChanges();

                    }
                }
                int orderid = 0;
                var orderResults = db.CrmLeadSoDetails.Where(x => x.LeadId == leadDto.id).ToList();
                foreach (var orderDet in orderResults)
                {
                    CrmLeadSoDetail crmLeadSoDetail = new CrmLeadSoDetail
                    {

                        customer_id = newCustomerId,
                        ContactId = orderDet.ContactId,
                        ContactName = orderDet.ContactName,
                        SoDate = orderDet.SoDate,
                        Comments = orderDet.Comments,
                        PaymentTerms = orderDet.PaymentTerms,
                        Terms = orderDet.Terms,
                        DeliveryTerms = orderDet.DeliveryTerms,
                        BillingAddress = orderDet.BillingAddress,
                        ShippingAddress = orderDet.ShippingAddress,
                        AdditionalCharges = orderDet.AdditionalCharges,
                        FreightCharge = orderDet.FreightCharge,
                        CustomerCode = orderDet.CustomerCode,
                        CustomsDuties = orderDet.CustomsDuties,
                        GrandTotal = orderDet.GrandTotal,
                        Subtotal = orderDet.Subtotal,
                        BranchCode = orderDet.BranchCode

                    };
                    db.CrmLeadSoDetails.Add(crmLeadSoDetail);
                    db.SaveChanges();
                    orderid = crmLeadSoDetail.SoId;
                    var subquotation = db.CrmLeadSoLinesDetails.Where(x => x.SoId == orderid).ToList();
                    foreach (var item in subquotation)
                    {
                        CrmLeadSoLinesDetail crmLeadSoLinesDetail = new CrmLeadSoLinesDetail
                        {

                            SoId = item.SoId,
                            ItemId = item.ItemId,
                            ItemCode = item.ItemCode,
                            ItemName = item.ItemName,
                            Qty = item.Qty,
                            UnitPrice = item.UnitPrice,
                            Discount = item.Discount,
                            DiscountAmount = item.DiscountAmount,
                            UomId = item.UomId,
                            UomName = item.UomName,
                            LeadDays = item.LeadDays,
                            TotalAmt = item.TotalAmt,
                            TotalPrice = item.TotalPrice,
                            TaxId = item.TaxId,
                            TaxAmount = item.TaxAmount,
                            TaxName = item.TaxName,
                            TaxPercentage = item.TaxPercentage,
                            CustomerCode = item.CustomerCode,
                            BranchCode = item.BranchCode

                        };
                        db.CrmLeadSoLinesDetails.Add(crmLeadSoLinesDetail);
                        db.SaveChanges();

                    }
                }

                //// Process Reminders
                //var contactResults = db.CrmLeadContact.Where(x => x.LeadId == leadDto.id).ToList();
                //foreach (var existingContact in contactResults)
                //{
                //    crmLeadContact leadContact = new crmLeadContact
                //    {
                //        LeadId = existingContact.LeadId,
                //        customer_id = newCustomerId, // Link to the new customer
                //        FirstName = existingContact.FirstName,
                //        LastName = existingContact.LastName,
                //        Email = existingContact.Email,
                //        Mobile = existingContact.Mobile,
                //        Designation = existingContact.Designation,
                //        Location = existingContact.Location,
                //        BranchId = existingContact.BranchId,
                //        CustomerCode = existingContact.CustomerCode,
                //        CreatedBy = existingContact.CreatedBy,
                //        CreatedAt = DateTime.UtcNow, // Setting current time as created date
                //        ModifiedBy = existingContact.ModifiedBy,
                //        ModifiedAt = existingContact.ModifiedAt
                //    };

                //    db.CrmLeadContact.Add(leadContact);  // Ensure you're adding to CrmLeadContact table (not CrmRemainders)
                //    db.SaveChanges();
                //    Console.WriteLine($"Processed contact for LeadId: {leadContact.LeadId}");
                //}




                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }




        [HttpPost]
        [Authorize]
        [Route("GetCRMAllCustomers")]
        public IActionResult GetCRMAllCustomers([FromBody] dynamic model)
        {
            crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());

            var leads = db.crmleads
                .Where(x => x.branch_id == leadDto.branch_id && x.customer_code == leadDto.customer_code && x.type_lead_cus == "Cus")
                .Select(l => new crmleadsdto
                {
                    id = l.id,
                    branch_id = l.branch_id,
                    customer_code = l.customer_code,
                    LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                    company = l.company,
                    first_name = l.first_name,
                    last_name = l.last_name,
                    description = l.description,
                    business_email = l.business_email,
                    secondary_email = l.secondary_email,
                    LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                    LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                    website = l.website,
                    rating = l.rating,
                    skypeid = l.skypeid,
                    title = l.title,
                    phone = l.phone,
                    fax = l.fax,
                    twitter = l.twitter,
                    street = l.street,
                    city = l.city,
                    state = l.state,
                    zipcode = l.zipcode,
                    country = l.country,
                    annual_revenue = l.annual_revenue,
                    phonenumber = l.phonenumber,
                    LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                    numberofemployees = l.numberofemployees,
                    industry = l.industry,
                    convert_customer=l.converted_customer=="Yes"?true:false,
                    customerconverted=l.converted_customer

                })
                .OrderByDescending(x => x.id)
                .ToList();

            return Ok(leads);
        }

        [HttpPost]
        [Authorize]
        [Route("GetCRMCustomerById")]
        public IActionResult GetCRMCustomerById([FromBody] dynamic model)
        {
            try
            {
                // Deserialize the input model to get the customer ID
                crmleadsdto leadDto = JsonConvert.DeserializeObject<crmleadsdto>(model.ToString());

                // Find the customer by ID
                var customer = db.crmleads
                    .Where(x => x.id == leadDto.id && x.type_lead_cus == "Cus")
                    .Select(l => new crmleadsdto
                    {
                        id = l.id,
                        branch_id = l.branch_id,
                        customer_code = l.customer_code,
                        LeadOwnerName = l.lead_owner != null ? db.HrdEmployees.Where(e => e.RecordId == l.lead_owner).FirstOrDefault().Empname : null,
                        company = l.company,
                        first_name = l.first_name,
                        last_name = l.last_name,
                        description = l.description,
                        business_email = l.business_email,
                        secondary_email = l.secondary_email,
                        LeadStatusName = l.lead_status != null ? db.CrmLeadStatuses.Where(s => s.Id == l.lead_status).FirstOrDefault().Description : null,
                        LeadSourceName = l.lead_source != null ? db.CrmLeadSources.Where(s => s.id == l.lead_source).FirstOrDefault().description : null,
                        website = l.website,
                        rating = l.rating,
                        skypeid = l.skypeid,
                        title = l.title,
                        phone = l.phone,
                        fax = l.fax,
                        twitter = l.twitter,
                        street = l.street,
                        city = l.city,
                        state = l.state,
                        zipcode = l.zipcode,
                        country = l.country,
                        annual_revenue = l.annual_revenue,
                        phonenumber = l.phonenumber,
                        LeadStageName = l.lead_stage != null ? db.CrmLeadStages.Where(s => s.id == l.lead_stage).FirstOrDefault().description : null,
                        numberofemployees = l.numberofemployees,
                        IndustryName = l.industry,
                        alternate_number = l.alternate_number,
                    })
                    .FirstOrDefault();

                // If customer not found, return NotFound response
                if (customer == null)
                {
                    return NotFound(new { message = "Customer not found" });
                }

                // Return the customer details
                return Ok(customer);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error: {ex.Message}");

                // Return an error response
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
