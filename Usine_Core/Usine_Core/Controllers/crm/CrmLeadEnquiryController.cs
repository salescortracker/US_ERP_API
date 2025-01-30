using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Usine_Core.Controllers.Admin;
using Usine_Core.Models;
using System;
using Org.BouncyCastle.Ocsp;
using DocumentFormat.OpenXml.Drawing;
namespace Usine_Core.Controllers.crm
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrmLeadEnquiryController : ControllerBase
    {
       
            public class CRMAddEnquiry
            {
                public CrmLeadEnquiryDetail obj { get; set; }
                public List<CrmLeadEnquiryLineDetail> lines { get; set; }
                public int traCheck { get; set; }
                public UserInfo usr { get; set; }
            }
        public class CRMAddQuotation
        {
            public CrmLeadQuotationDetail obj { get; set; }
            public List<CrmLeadQuotationLineDetail> lines { get; set; }
            public int traCheck { get; set; }
            public UserInfo usr { get; set; }
        }
        public class CRMAddSo
        {
            public CrmLeadSoDetail obj { get; set; }
            public List<CrmLeadSoLinesDetail> lines { get; set; }
            public int traCheck { get; set; }
            public UserInfo usr { get; set; }
        }
        UsineContext db = new UsineContext();
            AdminControl ac = new AdminControl();
        /// <summary>
        /// This was implement due to manideep for changes-26/12/
        /// added by durga
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
            [HttpPost]
            [Authorize]
            [Route("AddCRMRxEnquiry")]
            public IActionResult AddCRMRxEnquiry([FromBody] CRMAddEnquiry req)
            {
            CrmLeadEnquiryDetail crmLeadEnquiryDetail = new CrmLeadEnquiryDetail();

                if (req.obj == null)
                {
                    return BadRequest("Enquiry details cannot be null.");
                }

                try
                {
                    req.obj.EnquiryDate = req.obj.EnquiryDate ?? DateTime.Now;
                req.obj.customer_id = req.obj.customer_id;
                    if (req.traCheck == 1)
                    {
                        db.CrmLeadEnquiryDetails.Add(req.obj);
                        db.SaveChanges();

                        for (int i = 0; i < req.lines.Count; i++)
                        {
                            req.lines[i].LineId = 0;
                            req.lines[i].EnquiryId = req.obj.EnquiryId;
                        }

                        db.CrmLeadEnquiryLineDetails.AddRange(req.lines);
                        db.SaveChanges();

                        return Ok(new
                        {
                            Message = "Enquiry added successfully",
                            EnquiryId = req.obj.EnquiryId,
                            data = req
                        });
                    }
                    else if (req.traCheck == 2)
                    {
                        var existingEnquiry = db.CrmLeadEnquiryDetails
                                                  .FirstOrDefault(e => e.EnquiryId == req.obj.EnquiryId);
                        if (existingEnquiry == null)
                        {
                            return NotFound("Enquiry not found.");
                        }

                        existingEnquiry.ContactName = req.obj.ContactName;
                        existingEnquiry.ContactId = req.obj.ContactId;
                        existingEnquiry.EnquiryDate = req.obj.EnquiryDate;
                        existingEnquiry.Comments = req.obj.Comments;
                        existingEnquiry.FollowupDate = req.obj.FollowupDate;
                        existingEnquiry.AdditionalCharges = req.obj.AdditionalCharges;
                        existingEnquiry.GrandTotal = req.obj.GrandTotal;
                        existingEnquiry.Subtotal = req.obj.Subtotal;
                        existingEnquiry.LeadId = req.obj.LeadId;
                        existingEnquiry.CustomerCode = req.obj.CustomerCode;
                        existingEnquiry.BranchCode = req.obj.BranchCode;
                    existingEnquiry.customer_id = req.obj.customer_id;
                        db.CrmLeadEnquiryDetails.Update(existingEnquiry);
                        db.SaveChanges();

                        var existingLines = db.CrmLeadEnquiryLineDetails
                                               .Where(l => l.EnquiryId == req.obj.EnquiryId)
                                               .ToList();
                        db.CrmLeadEnquiryLineDetails.RemoveRange(existingLines);
                        db.SaveChanges();

                        for (int i = 0; i < req.lines.Count; i++)
                        {
                            req.lines[i].LineId = 0;
                            req.lines[i].EnquiryId = req.obj.EnquiryId;
                        }

                        db.CrmLeadEnquiryLineDetails.AddRange(req.lines);
                        db.SaveChanges();

                        return Ok(new
                        {
                            Message = "Enquiry updated successfully",
                            EnquiryId = req.obj.EnquiryId,
                            data = req
                        });
                    }
                    else
                    {
                        return BadRequest("Invalid traCheck value.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "An error occurred while adding or updating the enquiry.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
                }
            }

            [HttpPost]
            [Authorize]
            [Route("GetCRMRxEnquiries/{leadId}")]
            public IActionResult GetCRMRxEnquiries(UserInfo usr, int leadId)
            {
                try
                {
                    var enquiries = db.CrmLeadEnquiryDetails
                                      .Where(e => (e.LeadId == leadId || e.customer_id==leadId) && e.CustomerCode == usr.cCode)
                                      .OrderByDescending(e => e.EnquiryId)
                                      .ToList();

                    if (enquiries == null || enquiries.Count == 0)
                    {
                        return NotFound(new { Message = "No enquiries found for the given LeadId and CustomerId." });
                    }

                    return Ok(new
                    {
                        Message = "Enquiries retrieved successfully.",
                        Data = enquiries
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "An error occurred while retrieving the enquiries.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
                }
            }

            [HttpPost]
            [Authorize]
            [Route("GetCRMRxEnquiry/{enquiryId}")]
            public IActionResult GetCRMRxEnquiry(UserInfo usr, int enquiryId)
            {
            //try
            //{
            //    var enquiry = db.CrmLeadEnquiryDetails
            //                    .Where(e => e.EnquiryId == enquiryId)
            //                    .FirstOrDefault();

            //    if (enquiry == null)
            //    {
            //        return NotFound(new { Message = "Enquiry not found." });
            //    }

            //    var enquiryLines = db.CrmLeadEnquiryLineDetails
            //                         .Where(e => e.EnquiryId == enquiryId)
            //                         .ToList();

            //    var partyId = (enquiry.customer_id > 0) ? enquiry.customer_id : enquiry.LeadId;

            //    var leadDetails = db.crmleads
            //                      .Where(e => e.id == partyId)
            //                      .FirstOrDefault();

            //    var response = new
            //    {
            //        Enquiry = enquiry,
            //        EnquiryLines = enquiryLines,
            //        LeadDetails = leadDetails
            //    };

            //    return Ok(response);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { Message = "An error occurred while retrieving the enquiry data.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            //}
            try
            {
                var enquiry = db.CrmLeadEnquiryDetails
                                .Where(e => e.EnquiryId == enquiryId)
                                .FirstOrDefault();

                if (enquiry == null)
                {
                    return NotFound(new { Message = "Enquiry not found." });
                }

                var enquiryLines = db.CrmLeadEnquiryLineDetails
                                     .Where(e => e.EnquiryId == enquiryId)
                                     .ToList();
                crmleads leadDetails = new crmleads();
                if (enquiry.LeadId > 0)
                {
                     leadDetails = db.crmleads
                                      .Where(e => e.id == enquiry.LeadId)
                                      .FirstOrDefault();
                }
                else
                {
                     leadDetails = db.crmleads
                                      .Where(e => e.id == enquiry.customer_id)
                                      .FirstOrDefault();

                }

                var response = new
                {
                    Enquiry = enquiry,
                    EnquiryLines = enquiryLines,
                    LeadDetails = leadDetails
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the enquiry data.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }

        }
        [HttpPost]
        [Authorize]
        [Route("GetAllCRMRxSos")]
        public IActionResult GetAllCRMRxSos([FromBody] GeneralInformation inf)
        {
            try
            {
                // Check if frmDate and toDate are not null or empty
                if (string.IsNullOrEmpty(inf.frmDate) || string.IsNullOrEmpty(inf.toDate))
                {
                    return BadRequest(new { Message = "Date range (frmDate, toDate) is required." });
                }

                // Parse the date range from the input
                DateTime startDate = DateTime.Parse(inf.frmDate);
                DateTime endDate = DateTime.Parse(inf.toDate).AddDays(1);
                DateTime currentDate = ac.getPresentDateTime();

                // Query based on recordId condition
                if (inf.recordId == 1)
                {
                    var result = db.CrmLeadSoDetails
                                   .Where(e => e.SoDate >= startDate &&
                                               e.SoDate <= endDate &&
                                               e.CustomerCode == inf.usr.cCode)
                                   .OrderByDescending(e => e.so_seq_id)
                                   .ToList();

                    return Ok(result);
                }
                else
                {
                    var result = db.CrmLeadSoDetails
                                   .Where(e => e.SoDate >= startDate &&
                                               e.SoDate >= currentDate &&
                                               e.SoDate <= endDate &&
                                               e.CustomerCode == inf.usr.cCode)
                                   .OrderByDescending(e => e.so_seq_id)
                                   .ToList();

                    return Ok(result);
                }
            }
            catch (FormatException ex)
            {
                return BadRequest(new { Message = "Invalid date format.", Error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log and handle general errors
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("GetCrmLeadSoById")]
        public IActionResult GetCrmLeadSoById([FromBody] GeneralInformation inf)
        {
            try
            {
                // Validate SoId
                if (inf.recordId <= 0)
                {
                    return BadRequest(new { Message = "Invalid SoId. Please provide a valid ID." });
                }

                // Fetch the record from the database
                var record = db.CrmLeadSoDetails
                               .Where(e => e.SoId == inf.recordId
                                           && e.CustomerCode == inf.usr.cCode
                                           && e.BranchCode == inf.usr.bCode)
                               .FirstOrDefault();

                // Check if the record exists
                if (record == null)
                {
                    return NotFound(new { Message = $"No record found for SoId {inf.recordId}." });
                }

                // Return the record
                return Ok(record);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("AddCRMRxQuotation")]
        public IActionResult AddCRMRxQuotation([FromBody] CRMAddQuotation req)
        {
            if (req.obj == null)
            {
                return BadRequest("Quotation details cannot be null.");
            }
            try
            {
                req.obj.QuotationDate = req.obj.QuotationDate ?? DateTime.Now;
                req.obj.customer_id = req.obj.customer_id;
                if (req.traCheck == 1)
                {
                    db.CrmLeadQuotationDetails.Add(req.obj);
                    db.SaveChanges();
                    for (int i = 0; i < req.lines.Count; i++)
                    {
                        req.lines[i].LineId = 0;
                        req.lines[i].QuotationId = req.obj.QuotationId;
                    }
                    db.CrmLeadQuotationLinesDetails.AddRange(req.lines);
                    db.SaveChanges();
                    return Ok(new
                    {
                        Message = "Quotation added successfully",
                        QuotationId = req.obj.QuotationId,
                        data = req
                    });
                }
                else if (req.traCheck == 2)
                {
                    var existingQuotation = db.CrmLeadQuotationDetails
                                              .FirstOrDefault(e => e.QuotationId == req.obj.QuotationId);
                    if (existingQuotation == null)
                    {
                        return NotFound("Quotation not found.");
                    }
                    existingQuotation.QuotationId = req.obj.QuotationId;
                    existingQuotation.ContactName = req.obj.ContactName;
                    existingQuotation.ContactId = req.obj.ContactId;
                    existingQuotation.QuotationDate = req.obj.QuotationDate;
                    existingQuotation.Comments = req.obj.Comments;
                    existingQuotation.QuotationValidity = req.obj.QuotationValidity;
                    existingQuotation.Terms = req.obj.Terms;
                    existingQuotation.DeliveryTerms = req.obj.DeliveryTerms;
                    existingQuotation.BillingAddress = req.obj.BillingAddress;
                    existingQuotation.ShippingAddress = req.obj.ShippingAddress;
                    existingQuotation.AdditionalCharges = req.obj.AdditionalCharges;
                    existingQuotation.FreightCharge = req.obj.FreightCharge;
                    existingQuotation.CustomsDuties = req.obj.CustomsDuties;
                    existingQuotation.GrandTotal = req.obj.GrandTotal;
                    existingQuotation.Subtotal = req.obj.Subtotal;
                    existingQuotation.LeadId = req.obj.LeadId;
                    existingQuotation.customer_id = req.obj.customer_id;
                    db.CrmLeadQuotationDetails.Update(existingQuotation);
                    db.SaveChanges();
                    var existingLines = db.CrmLeadQuotationLinesDetails
                                           .Where(l => l.QuotationId == req.obj.QuotationId)
                                           .ToList();
                    db.CrmLeadQuotationLinesDetails.RemoveRange(existingLines);
                    db.SaveChanges();
                    for (int i = 0; i < req.lines.Count; i++)
                    {
                        req.lines[i].LineId = 0;
                        req.lines[i].QuotationId = req.obj.QuotationId;
                    }
                    db.CrmLeadQuotationLinesDetails.AddRange(req.lines);
                    db.SaveChanges();
                    return Ok(new
                    {
                        Message = "Quotation updated successfully",
                        //EnquiryId = req.obj.EnquiryId,
                        data = req
                    });
                }
                else
                {
                    return BadRequest("Invalid traCheck value.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding or updating the Quotation.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMRxQuotations/{leadId}")]
        public IActionResult GetCRMRxQuotations(UserInfo usr, int leadId)
        {
            try
            {
                var enquiries = db.CrmLeadQuotationDetails
                                  .Where(e => (e.LeadId == leadId || e.customer_id == leadId) && e.CustomerCode == usr.cCode)
                                  .OrderByDescending(e => e.QuotationId)
                                  .ToList();
                if (enquiries == null || enquiries.Count == 0)
                {
                    return NotFound(new { Message = "No Quotation found for the given LeadId and CustomerId." });
                }
                return Ok(new
                {
                    Message = "Quotation retrieved successfully.",
                    Data = enquiries
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the Quotation.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMRxQuotation/{quotationId}")]
        public IActionResult GetCRMRxQuotation(UserInfo usr, int quotationId)
        {
            try
            {
                var quotation = db.CrmLeadQuotationDetails
                                .Where(e => e.QuotationId == quotationId)
                                .FirstOrDefault();
                if (quotation == null)
                {
                    return NotFound(new { Message = "quotation not found." });
                }
                var quotationLines = db.CrmLeadQuotationLinesDetails
                                     .Where(e => e.QuotationId == quotationId)
                                     .ToList();
                crmleads leadDetails = new crmleads();
                if (quotation.LeadId > 0)
                {
                     leadDetails = db.crmleads
                                      .Where(e => e.id == quotation.LeadId)
                                      .FirstOrDefault();
                }
                else
                {
                     leadDetails = db.crmleads
                                      .Where(e => e.id == quotation.customer_id)
                                      .FirstOrDefault();

                }
                var response = new
                {
                    Quotation = quotation,
                    QuotationLines = quotationLines,
                    LeadDetails = leadDetails
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the enquiry data.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("CreateQuotationFromEnquiry/{enquiryId}")]
        public IActionResult CreateQuotationFromEnquiry(UserInfo usr, int enquiryId)
        {
            try
            {
                var enquiry = db.CrmLeadEnquiryDetails.FirstOrDefault(e => e.EnquiryId == enquiryId);
                if (enquiry == null)
                {
                    return NotFound(new { Message = "Enquiry not found." });
                }
                var quotation = new CrmLeadQuotationDetail
                {
                    ContactName = enquiry.ContactName,
                    ContactId = enquiry.ContactId,
                    QuotationDate = DateTime.Now,
                    Comments = enquiry.Comments,
                    AdditionalCharges = enquiry.AdditionalCharges,
                    GrandTotal = enquiry.GrandTotal,
                    Subtotal = enquiry.Subtotal,
                    LeadId = enquiry.LeadId,
                    CustomerCode = enquiry.CustomerCode,
                    BranchCode = enquiry.BranchCode,
                    QuotationValidity = DateTime.Now.AddDays(30),
                    Terms = "",
                    DeliveryTerms = "",
                    BillingAddress = "",
                    ShippingAddress = ""
                };
                db.CrmLeadQuotationDetails.Add(quotation);
                db.SaveChanges();
                var existingLines = db.CrmLeadEnquiryLineDetails
                       .Where(l => l.EnquiryId == enquiryId)
                       .Select(l => new CrmLeadQuotationLineDetail
                       {
                           QuotationId = quotation.QuotationId,
                           ItemId = l.ItemId,
                           ItemName = l.ItemName,
                           ItemCode = l.ItemCode,
                           Qty = l.Qty,
                           UnitPrice = l.UnitPrice,
                           Discount = l.Discount,
                           DiscountAmount = l.DiscountAmount,
                           UomId = l.UomId,
                           UomName = l.UomName,
                           LeadDays = l.LeadDays,
                           TotalPrice = l.TotalPrice,
                           TotalAmt = l.TotalAmt,
                           TaxId = null, // Default value
                           TaxName = null, // Default value
                           TaxPercentage = null, // Default value
                           TaxAmount = null, // Default value
                           CustomerCode = l.CustomerCode,
                           BranchCode = l.BranchCode
                       }).ToList();
                db.CrmLeadQuotationLinesDetails.AddRange(existingLines);
                db.SaveChanges();
                return Ok(new { QuotationId = quotation.QuotationId });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred.", Error = ex.Message });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("AddCRMRxSo")]
        public IActionResult AddCRMRxSo([FromBody] CRMAddSo req)
        {
            if (req.obj == null)
            {
                return BadRequest("Quotation details cannot be null.");
            }
            try
            {
                req.obj.SoDate = req.obj.SoDate ?? DateTime.Now;
                req.obj.customer_id = req.obj.customer_id;
                if (req.traCheck == 1)
                {
                    db.CrmLeadSoDetails.Add(req.obj);
                    db.SaveChanges();
                    for (int i = 0; i < req.lines.Count; i++)
                    {
                        req.lines[i].LineId = 0;
                        req.lines[i].SoId = req.obj.SoId;
                    }
                    db.CrmLeadSoLinesDetails.AddRange(req.lines);
                    db.SaveChanges();
                    return Ok(new
                    {
                        Message = "Quotation added successfully",
                        QuotationId = req.obj.SoId,
                        data = req
                    });
                }
                else if (req.traCheck == 2)
                {
                    var existingSo = db.CrmLeadSoDetails
                                              .FirstOrDefault(e => e.SoId == req.obj.SoId);
                    if (existingSo == null)
                    {
                        return NotFound("Quotation not found.");
                    }
                    existingSo.SoId = req.obj.SoId;
                    existingSo.ContactName = req.obj.ContactName;
                    existingSo.ContactId = req.obj.ContactId;
                    existingSo.SoDate = req.obj.SoDate;
                    existingSo.Comments = req.obj.Comments;
                    existingSo.PaymentTerms = req.obj.PaymentTerms;
                    existingSo.Terms = req.obj.Terms;
                    existingSo.DeliveryTerms = req.obj.DeliveryTerms;
                    existingSo.BillingAddress = req.obj.BillingAddress;
                    existingSo.ShippingAddress = req.obj.ShippingAddress;
                    existingSo.AdditionalCharges = req.obj.AdditionalCharges;
                    existingSo.FreightCharge = req.obj.FreightCharge;
                    existingSo.CustomsDuties = req.obj.CustomsDuties;
                    existingSo.GrandTotal = req.obj.GrandTotal;
                    existingSo.Subtotal = req.obj.Subtotal;
                    existingSo.LeadId = req.obj.LeadId;
                    existingSo.customer_id=req.obj.customer_id;
                    db.CrmLeadSoDetails.Update(existingSo);
                    db.SaveChanges();
                    var existingLines = db.CrmLeadSoLinesDetails
                                           .Where(l => l.SoId == req.obj.SoId)
                                           .ToList();
                    db.CrmLeadSoLinesDetails.RemoveRange(existingLines);
                    db.SaveChanges();
                    for (int i = 0; i < req.lines.Count; i++)
                    {
                        req.lines[i].LineId = 0;
                        req.lines[i].SoId = req.obj.SoId;
                    }
                    db.CrmLeadSoLinesDetails.AddRange(req.lines);
                    db.SaveChanges();
                    return Ok(new
                    {
                        Message = "Quotation updated successfully",
                        SoId = req.obj.SoId,
                        data = req
                    });
                }
                else
                {
                    return BadRequest("Invalid traCheck value.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding or updating the Quotation.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMRxSos/{leadId}")]
        public IActionResult GetCRMRxSos(UserInfo usr, int leadId)
        {
            try
            {
                var enquiries = db.CrmLeadSoDetails
                                  .Where(e => (e.LeadId == leadId || e.customer_id==leadId) && e.CustomerCode == usr.cCode)
                                  .OrderByDescending(e => e.SoId)
                                  .ToList();
                if (enquiries == null || enquiries.Count == 0)
                {
                    return NotFound(new { Message = "No Quotation found for the given LeadId and CustomerId." });
                }
                return Ok(new
                {
                    Message = "Quotation retrieved successfully.",
                    Data = enquiries
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the Quotation.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("GetCRMRxso/{soId}")]
        public IActionResult GetCRMRxso(UserInfo usr, int soId)
        {
            try
            {
                var so = db.CrmLeadSoDetails
                                .Where(e => e.SoId == soId)
                                .FirstOrDefault();
                if (so == null)
                {
                    return NotFound(new { Message = "So not found." });
                }
                var soLines = db.CrmLeadSoLinesDetails
                                     .Where(e => e.SoId == soId)
                                     .ToList();
                var partyId = (so.customer_id > 0) ? so.customer_id : so.LeadId;

                crmleads leadDetails = new crmleads();
                if (so.LeadId > 0)
                {
                     leadDetails = db.crmleads
                                      .Where(e => e.id == so.LeadId)
                                      .FirstOrDefault();
                }
                else
                {
                    leadDetails = db.crmleads
                                      .Where(e => e.id == so.customer_id)
                                      .FirstOrDefault();

                }
                var response = new
                {
                    So = so,
                    SoLines = soLines,
                    LeadDetails = leadDetails
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the enquiry data.", Error = $"Error: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("CreateSoFromQuotation/{quotationId}")]
        public IActionResult CreateSoFromQuotation(UserInfo usr, int quotationId)
        {
            try
            {
                var quotation = db.CrmLeadQuotationDetails.FirstOrDefault(e => e.QuotationId == quotationId);
                if (quotation == null)
                {
                    return NotFound(new { Message = "Enquiry not found." });
                }
                var so = new CrmLeadSoDetail
                {
                    SoId = 0,
                    ContactName = quotation.ContactName,
                    ContactId = quotation.ContactId,
                    SoDate = quotation.QuotationDate,
                    Comments = quotation.Comments,
                    PaymentTerms = "",
                    Terms = quotation.Terms,
                    DeliveryTerms = quotation.DeliveryTerms,
                    BillingAddress = quotation.BillingAddress,
                    ShippingAddress = quotation.ShippingAddress,
                    AdditionalCharges = quotation.AdditionalCharges,
                    FreightCharge = quotation.FreightCharge,
                    CustomsDuties = quotation.CustomsDuties,
                    GrandTotal = quotation.GrandTotal,
                    Subtotal = quotation.Subtotal,
                    LeadId = quotation.LeadId,
                    CustomerCode = quotation.CustomerCode,
                    BranchCode = quotation.BranchCode,
                };
                db.CrmLeadSoDetails.Add(so);
                db.SaveChanges();
                var existingLines = db.CrmLeadQuotationLinesDetails
                       .Where(l => l.QuotationId == quotationId)
                       .Select(l => new CrmLeadSoLinesDetail
                       {
                           SoId = so.SoId,
                           LineId = 0,
                           ItemId = l.ItemId,
                           ItemName = l.ItemName,
                           ItemCode = l.ItemCode,
                           Qty = l.Qty,
                           UnitPrice = l.UnitPrice,
                           Discount = l.Discount,
                           DiscountAmount = l.DiscountAmount,
                           UomId = l.UomId,
                           UomName = l.UomName,
                           LeadDays = l.LeadDays,
                           TotalPrice = l.TotalPrice,
                           TotalAmt = l.TotalAmt,
                           TaxId = l.TaxId,
                           TaxName = l.TaxName,
                           TaxPercentage = l.TaxPercentage,
                           TaxAmount = l.TaxAmount,
                           CustomerCode = l.CustomerCode,
                           BranchCode = l.BranchCode
                       }).ToList();
                db.CrmLeadSoLinesDetails.AddRange(existingLines);
                db.SaveChanges();
                return Ok(new { soId = so.SoId });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred.", Error = ex.Message });
            }
        }
    }
    
}
