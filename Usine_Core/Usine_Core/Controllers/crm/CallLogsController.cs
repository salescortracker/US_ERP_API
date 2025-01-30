using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Usine_Core.Models;
using Usine_Core.others.dtos;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading.Tasks;
using Usine_Core.Controllers.Admin;
using Usine_Core.others;


namespace Usine_Core.Controllers.crm
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallLogsController : ControllerBase
    {
        UsineContext db = new UsineContext();
        public class CrmCalllisting
        {
            public string fromDate { get; set; }
            public string toDate { get; set; }
            public UserInfo usr { get; set; }
        }
        public class CrmCallLogWithReasonDto
        {
            public int Id { get; set; }
            public int? LeadId { get; set; }
            public string? contactName { get; set; }
            public int? CallTypes { get; set; }
            public string? CallType { get; set; }
            public DateTime? CallDate { get; set; }
            public string? Comments { get; set; }
            public string? CallerNotes { get; set; }
            public string? CustomerNotes { get; set; }
            public int ReasonForCall { get; set; }
            public string ReasonForCallDescription { get; set; }
            public int? CustomerId { get; set; }
        }
        [HttpPost]
        [Authorize]
        [Route("GetAllCallLogs")]
        public IActionResult GetAllCallLogs([FromBody] dynamic model)
        {
            CrmCallLogsDto callLogDto = JsonConvert.DeserializeObject<CrmCallLogsDto>(model.ToString());
            var callLogs = db.CrmCallLogs.Where(x=>x.branchid==callLogDto.branchid && x.customercode==callLogDto.customercode && x.leadid==callLogDto.leadid).Select(c => new CrmCallLogsDto
            {
                Id = c.Id,
                ContactId = c.ContactId,
                LeadOwnerName = c.LeadOwnerId != null ? db.HrdEmployees.Where(e => e.RecordId == c.LeadOwnerId).FirstOrDefault().Empname : null,
                CallTypesName = c.CallTypes != null ? db.CrmCallTypes.Where(ct => ct.Id == c.CallTypes).FirstOrDefault().Description : null,
                CallDate = c.CallDate,
                Comments = c.Comments,
                callernotes=c.callernotes,
                customernotes=c.customernotes,
                reasonforcall=c.reasonforcall,
                branchid=callLogDto.branchid,
                customercode=callLogDto.customercode,
                leadid=callLogDto.leadid,
                reasonforcallname=c.reasonforcall>0? db.crmcallforreasons.Where(x=>x.id==c.reasonforcall).FirstOrDefault().description:null,
            }).ToList();

            return Ok(callLogs);
        }
        [HttpPost]
        [Authorize]
        [Route("GetAllCallLogsCustomer")]
        public IActionResult GetAllCallLogsCustomer([FromBody] dynamic model)
        {
            CrmCallLogsDto callLogDto = JsonConvert.DeserializeObject<CrmCallLogsDto>(model.ToString());
            var callLogs = db.CrmCallLogs.Where(x => x.branchid == callLogDto.branchid && x.customercode == callLogDto.customercode && x.customer_id == callLogDto.customer_id).Select(c => new CrmCallLogsDto
            {
                Id = c.Id,
                ContactId = c.ContactId,
                LeadOwnerName = c.LeadOwnerId != null ? db.HrdEmployees.Where(e => e.RecordId == c.LeadOwnerId).FirstOrDefault().Empname : null,
                CallTypesName = c.CallTypes != null ? db.CrmCallTypes.Where(ct => ct.Id == c.CallTypes).FirstOrDefault().Description : null,
                CallDate = c.CallDate,
                Comments = c.Comments,
                callernotes = c.callernotes,
                customernotes = c.customernotes,
                reasonforcall = c.reasonforcall,
                branchid = callLogDto.branchid,
                customercode = callLogDto.customercode,
                leadid = callLogDto.leadid,
                reasonforcallname = c.reasonforcall > 0 ? db.crmcallforreasons.Where(x => x.id == c.reasonforcall).FirstOrDefault().description : null,
            }).ToList();

            return Ok(callLogs);
        }
        [HttpPost]
        [Authorize]
        [Route("GetAllCallLogsbyCompany")]
        public async Task<IActionResult> GetAllCallLogsbyCompany([FromBody] CrmCalllisting req)
        {
            try
            {
                if (req == null || req.usr.cCode <= 0)
                {
                    return BadRequest("Invalid or missing Customer Code.");
                }
                if (!DateTime.TryParse(req.fromDate, out DateTime fromDate))
                {
                    return BadRequest("Invalid fromDate format.");
                }
                if (!DateTime.TryParse(req.toDate, out DateTime toDate))
                {
                    return BadRequest("Invalid toDate format.");
                }
                toDate = toDate.Date.AddDays(1).AddTicks(-1);
                var calllogs = await (from callLog in db.CrmCallLogs
                                      join reason in db.crmcallforreasons
                                      on callLog.reasonforcall equals reason.id
                                      join callType in db.CrmCallTypes
                                      on callLog.CallTypes equals callType.Id
                                      where callLog.customercode == req.usr.cCode
                                      && callLog.CreatedAt >= fromDate
                                      && callLog.CreatedAt <= toDate
                                      select new CrmCallLogWithReasonDto
                                      {
                                          Id = callLog.Id,
                                          LeadId = callLog.leadid,
                                          contactName = callLog.ContactId,
                                          CallTypes = callLog.CallTypes,
                                          CallType = callType.Description,
                                          CallDate = callLog.CallDate,
                                          Comments = callLog.Comments,
                                          CallerNotes = callLog.callernotes,
                                          CustomerNotes = callLog.customernotes,
                                          ReasonForCall = callLog.reasonforcall,
                                          ReasonForCallDescription = reason.description,
                                          CustomerId = callLog.customer_id,
                                          
                                      }).ToListAsync();
                if (calllogs == null || !calllogs.Any())
                {
                    return NotFound($"No calllogs found with CustomerCode = {req.usr.cCode}");
                }
                return Ok(calllogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        [Authorize]
        [HttpPost("SaveCallLog")]
        public IActionResult SaveCallLog([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            var callLogDto = JsonConvert.DeserializeObject<CrmCallLogsDto>(model.ToString());

            if (callLogDto == null)
            {
                return BadRequest("Invalid call log data.");
            }

            // Create a new CrmCallLogs entity and map the DTO fields
            var callLog = new CrmCallLogs
            {
                branchid=callLogDto.branchid,
                customercode=callLogDto.customercode,
                leadid =callLogDto.leadid,
                customer_id=callLogDto.customer_id,
                callernotes =callLogDto.callernotes,
                customernotes=callLogDto.customernotes,
                reasonforcall=callLogDto.reasonforcall,
                ContactId = callLogDto.ContactId,
                LeadOwnerId = callLogDto.LeadOwnerId,
                CallTypes = callLogDto.CallTypes,
                CallDate = callLogDto.CallDate,
                Comments = callLogDto.Comments,
                CreatedAt = DateTime.UtcNow, // Set the created date to now
                CreatedBy = callLogDto.CreatedBy, // Assuming you pass the CreatedBy ID in the DTO
                ModifiedBy = null, // Initially set to null, can be updated later
                ModifiedAt = null // Initially set to null, can be updated later
            };

            // Add the call log to the database context and save changes
            db.CrmCallLogs.Add(callLog);
            db.SaveChanges();

            // Return a success message with the ID of the created call log
            return Ok(new { message = "Call log saved successfully", callLogId = callLog.Id });
        }

        [HttpPost]
        [Authorize]
        [Route("UpdateCallLog")]
        public IActionResult UpdateCallLog([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            CrmCallLogsDto callLogDto = JsonConvert.DeserializeObject<CrmCallLogsDto>(model.ToString());

            if (callLogDto == null || callLogDto.Id <= 0)
            {
                return BadRequest("Invalid call log data or missing Id.");
            }

            // Fetch the existing call log using the Id
            var callLogRecord = db.CrmCallLogs.Where(x => x.Id == callLogDto.Id).FirstOrDefault();

            if (callLogRecord == null)
            {
                return NotFound("Call log record not found.");
            }
            callLogRecord.branchid = callLogDto.branchid;
            callLogRecord.customercode = callLogDto.customercode;
            // Update the existing call log record with new values from the DTO
            callLogRecord.ContactId = callLogDto.ContactId;
            callLogRecord.LeadOwnerId = callLogDto.LeadOwnerId;
            callLogRecord.CallTypes = callLogDto.CallTypes;
            callLogRecord.callernotes = callLogDto.callernotes;
            callLogRecord.customernotes = callLogDto.customernotes;
            callLogRecord.reasonforcall = callLogDto.reasonforcall;
            callLogRecord.CallDate = callLogDto.CallDate;
            callLogRecord.Comments = callLogDto.Comments;
            callLogRecord.ModifiedBy = callLogDto.ModifiedBy; // Assuming ModifiedBy is passed in DTO
            callLogRecord.ModifiedAt = DateTime.UtcNow; // Set the modified date to now
            callLogRecord.customer_id = callLogDto.customer_id;
            // Update the record in the database
            db.CrmCallLogs.Update(callLogRecord);
            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("GetCallLogById/{id}")]
        public IActionResult GetCallLogById(int id)
        {
            // Deserialize the incoming model to the DTO
            // CrmCallLogsDto callLogDto = JsonConvert.DeserializeObject<CrmCallLogsDto>(model.ToString());

            // Fetch the call log details by Id
            var callLogDetails = db.CrmCallLogs
                .Where(c => c.Id == id)
                .Select(callLog => new CrmCallLogsDto
                {
                    Id = callLog.Id,
                    ContactId = callLog.ContactId,
                    LeadOwnerId = callLog.LeadOwnerId,
                    CallTypes = callLog.CallTypes,
                    CallDate = callLog.CallDate,
                    Comments = callLog.Comments,
                    callernotes=callLog.callernotes,
                    customernotes=callLog.customernotes,
                    reasonforcall=callLog.reasonforcall
                })
                .FirstOrDefault();

            // Return the call log details if found, or a not found response if not
            if (callLogDetails != null)
            {
                return Ok(callLogDetails);
            }
            else
            {
                return NotFound("Call log details not found.");
            }
        }

        // Delete a call log by ID
        [HttpPost]
        [Authorize]
        [Route("DeleteCallLogById")]
        public IActionResult DeleteCallLogById([FromBody] dynamic model)
        {
            // Deserialize the incoming model to the DTO
            CrmCallLogsDto callLogDto = JsonConvert.DeserializeObject<CrmCallLogsDto>(model.ToString());

            // Find the call log record by Id
            var callLogDetails = db.CrmCallLogs.Where(x => x.Id == callLogDto.Id).FirstOrDefault();

            // Check if the call log record exists
            if (callLogDetails == null)
            {
                return NotFound(new { message = "Call log details not found." });
            }

            // Remove the call log record from the database
            db.CrmCallLogs.Remove(callLogDetails);
            db.SaveChanges();

            // Return a success message
            return Ok(new { message = "Call log details deleted successfully." });
        }


        [HttpGet]
        [Authorize]
        [Route("GetAllCrmCallTypes")]
        public IActionResult GetAllCrmCallTypes()
        {
            // Retrieve all crmCallTypes records and map them to the DTO
            var callTypes = db.CrmCallTypes
                .Select(c => new CrmCallTypeDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    CreatedBy = c.CreatedBy,
                    ModifiedAt = c.ModifiedAt,
                    ModifiedBy = c.ModifiedBy
                }).ToList();

            // Return the list of call types
            return Ok(callTypes);
        }




        //EVENTS -------------------------------------------

        [HttpPost]
        [Authorize]
        [HttpPost("SaveEvent")]
        public IActionResult SaveEvent([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            var eventDto = JsonConvert.DeserializeObject<CrmLeadsEventDto>(model.ToString());

            if (eventDto == null)
            {
                return BadRequest("Invalid event data.");
            }

            // Create a new CrmLeadsEvent entity and map the DTO fields
            var crmEvent = new CrmLeadsEvent
            {
                EventTitle = eventDto.EventTitle,
                EventTime = eventDto.EventTime,
                EventGuests = eventDto.EventGuests,
                MeetingLink = eventDto.MeetingLink,
                branchid=eventDto.branchid,
                customercode=eventDto.customercode,
                customer_id = eventDto.customer_id,
                MeetingLocation = eventDto.MeetingLocation,
                leadid=eventDto.leadid,
                CreatedAt = DateTime.UtcNow, // Set the created date to now
                CreatedBy = eventDto.CreatedBy, // Assuming CreatedBy ID is in the DTO
                ModifiedBy = null, // Initially set to null, can be updated later
                ModifiedAt = null // Initially set to null, can be updated later
            };
            var partyId = (crmEvent.customer_id > 0) ? crmEvent.customer_id : crmEvent.leadid;
            var leadDetails = db.crmleads
                  .Where(e => e.id == partyId)
                  .FirstOrDefault();
            // Add the event to the database context and save changes
            db.CrmLeadsEvents.Add(crmEvent);
            db.SaveChanges();

            others.sendEmail sendEmail = new others.sendEmail();
            sendEmail.EmailSend("Meeting Invite",
                leadDetails.business_email,
                "Dear " + leadDetails.company + ",\n\nYou are invited to a scheduled meeting. Please find the meeting details below:\n\n" +
                "Date & Time: " + crmEvent.EventTime + "\n\n" +
                "Location: " + crmEvent.MeetingLocation + "\n\n" +
                "Link: " + crmEvent.MeetingLink + "\n\n" +
                "Please ensure to join the meeting on time. If you have any questions, feel free to reach out.\n\n" +
                "Thanks,\nTeam Cortracker",
                null,
                "amrutha@cortracker360.com",
                leadDetails.secondary_email);
            // Return a success message with the ID of the created event
            return Ok(new { message = "Event saved successfully", eventId = crmEvent.Id });
        }

        [HttpPost]
        [Authorize]
        [Route("GetAllupcomingEvents")]
        public IActionResult GetAllupcomingEvents([FromBody] UserInfo usr)
        {
            var events = db.CrmLeadsEvents.Where(x => x.customercode == usr.cCode && x.EventTime > DateTime.Now)
            .Select(e => new CrmLeadsEventDto
            {
                Id = e.Id,
                EventTitle = e.EventTitle,
                EventTime = e.EventTime,
                branchid = e.branchid,
                customercode = e.customercode,
                EventGuests = e.EventGuests,
                MeetingLink = e.MeetingLink,
                MeetingLocation = e.MeetingLocation,
                customer_id = e.customer_id,
                CustomerName = db.crmleads
                        .Where(c => c.id == e.customer_id)
                        .Select(c => c.company)
                        .FirstOrDefault(),
                leadid = e.leadid,
                LeadName = db.crmleads
                        .Where(c => c.id == e.leadid)
                        .Select(c => c.company)
                        .FirstOrDefault(),
                CreatedBy = e.CreatedBy,
                CreatedAt = e.CreatedAt,
                ModifiedBy = e.ModifiedBy,
                ModifiedAt = e.ModifiedAt,
            }).ToList();
            return Ok(events);
        }

        [HttpPost]
        [Authorize]
        [Route("GetAllEvents")]
        public IActionResult GetAllEvents([FromBody] dynamic model)
        {
            CrmLeadsEventDto eventDto = JsonConvert.DeserializeObject<CrmLeadsEventDto>(model.ToString());
            if (eventDto.leadid > 0)
            {
                var events = db.CrmLeadsEvents.Where(x => x.branchid == eventDto.branchid && x.customercode == eventDto.customercode && (x.leadid == eventDto.leadid))
                .Select(e => new CrmLeadsEventDto
                {
                    Id = e.Id,
                    EventTitle = e.EventTitle,
                    EventTime = e.EventTime,
                    branchid = e.branchid,
                    customercode = e.customercode,
                    EventGuests = e.EventGuests,
                    MeetingLink = e.MeetingLink,
                    MeetingLocation = e.MeetingLocation,
                    customer_id = e.customer_id,
                    CreatedBy = e.CreatedBy,
                    CreatedAt = e.CreatedAt,
                    ModifiedBy = e.ModifiedBy,
                    ModifiedAt = e.ModifiedAt,
                }).ToList();
                return Ok(events);
            }
            else
            {
                var events = db.CrmLeadsEvents.Where(x => x.branchid == eventDto.branchid && x.customercode == eventDto.customercode && (x.customer_id == eventDto.customer_id))
                .Select(e => new CrmLeadsEventDto
                {
                    Id = e.Id,
                    EventTitle = e.EventTitle,
                    EventTime = e.EventTime,
                    branchid = e.branchid,
                    customercode = e.customercode,
                    EventGuests = e.EventGuests,
                    MeetingLink = e.MeetingLink,
                    MeetingLocation = e.MeetingLocation,
                    customer_id = e.customer_id,
                    CreatedBy = e.CreatedBy,
                    CreatedAt = e.CreatedAt,
                    ModifiedBy = e.ModifiedBy,
                    ModifiedAt = e.ModifiedAt,
                }).ToList();
                return Ok(events);

            }

            
        }

        [HttpPost]
        [Authorize]
        [Route("UpdateEvent")]
        public IActionResult UpdateEvent([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            CrmLeadsEventDto eventDto = JsonConvert.DeserializeObject<CrmLeadsEventDto>(model.ToString());

            if (eventDto == null || eventDto.Id <= 0)
            {
                return BadRequest("Invalid event data or missing Id.");
            }

            // Fetch the existing event using the Id
            var eventRecord = db.CrmLeadsEvents.Where(x => x.Id == eventDto.Id).FirstOrDefault();

            if (eventRecord == null)
            {
                return NotFound("Event record not found.");
            }

            eventRecord.branchid = eventDto.branchid;
            eventRecord.customercode = eventDto.customercode;
            // Update the existing event record with new values from the DTO
            eventRecord.EventTitle = eventDto.EventTitle;
            eventRecord.EventTime = eventDto.EventTime;
            eventRecord.EventGuests = eventDto.EventGuests;
            eventRecord.MeetingLink = eventDto.MeetingLink;
            eventRecord.MeetingLocation = eventDto.MeetingLocation;
            eventRecord.ModifiedBy = eventDto.ModifiedBy; // Assuming ModifiedBy is passed in DTO
            eventRecord.ModifiedAt = DateTime.UtcNow; // Set the modified date to now
            eventRecord.leadid = eventDto.leadid;
            eventRecord.customer_id=eventDto.customer_id;
            // Update the record in the database
            db.CrmLeadsEvents.Update(eventRecord);
            db.SaveChanges();

            return Ok(new { message = "Event updated successfully" });
        }


        [HttpPost]
        [Authorize]
        [Route("GetEventById")]
        public IActionResult GetEventById([FromBody] dynamic model)
        {
            CrmLeadsEventDto eventDto = JsonConvert.DeserializeObject<CrmLeadsEventDto>(model.ToString());
            // Fetch the event details by Id
            var eventDetails = db.CrmLeadsEvents
                .Where(e => e.Id == eventDto.Id)
                .Select(e => new CrmLeadsEventDto
                {
                    Id = e.Id,
                    EventTitle = e.EventTitle,
                    EventTime = e.EventTime,
                    EventGuests = e.EventGuests,
                    MeetingLink = e.MeetingLink,
                    MeetingLocation = e.MeetingLocation,
                    CreatedBy = e.CreatedBy,
                    CreatedAt = e.CreatedAt,
                    ModifiedBy = e.ModifiedBy,
                    ModifiedAt = e.ModifiedAt
                })
                .FirstOrDefault();

            // Return the event details if found, or a not found response if not
            if (eventDetails != null)
            {
                return Ok(eventDetails);
            }
            else
            {
                return NotFound("Event details not found.");
            }
        }



        [HttpPost]
        [Authorize]
        [Route("DeleteEventById")]
        public IActionResult DeleteEventById([FromBody] dynamic model)
        {
            // Deserialize the incoming model to the Event DTO (Data Transfer Object)
            CrmLeadsEventDto eventDto = JsonConvert.DeserializeObject<CrmLeadsEventDto>(model.ToString());

            // Find the event record by Id
            var eventDetails = db.CrmLeadsEvents.Where(x => x.Id == eventDto.Id).FirstOrDefault();

            // Check if the event record exists
            if (eventDetails == null)
            {
                return NotFound(new { message = "Event details not found." });
            }

            // Remove the event record from the database
            db.CrmLeadsEvents.Remove(eventDetails);
            db.SaveChanges();

            // Return a success message
            return Ok(new { message = "Event details deleted successfully." });
        }




        // Reminders-----------------------------------------------------------------------------------


        [HttpPost]
        [Authorize]
        [Route("SaveRemainder")]
        public IActionResult SaveRemainder([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            CrmRemainderDto remainderDto = JsonConvert.DeserializeObject<CrmRemainderDto>(model.ToString());

            if (remainderDto == null)
            {
                return BadRequest("Invalid remainder data.");
            }

            // Create a new CrmRemainder entity and map the DTO fields
            var crmRemainder = new CrmRemainder
            {
                leadid = remainderDto.leadid,
                customer_id = remainderDto.customer_id,
                ReminderName = remainderDto.ReminderName,
                ReminderDate = remainderDto.ReminderDate,
                ReminderTime = remainderDto.ReminderTime,
                Notes = remainderDto.Notes,
                reminder_type = remainderDto.reminder_type,
                BranchId = remainderDto.BranchId,
                CustomerCode = remainderDto.CustomerCode,
                CreatedDate = DateTime.UtcNow, // Set the created date to now
                CreatedBy = remainderDto.CreatedBy, // Assuming CreatedBy ID is in the DTO
                ModifiedBy = null, // Initially set to null, can be updated later
                ModifiedDate = null // Initially set to null, can be updated later
            };

            // Add the remainder to the database context and save changes
            db.CrmRemainders.Add(crmRemainder);
            db.SaveChanges();
            var result = db.crmleads.Where(x => x.id == remainderDto.leadid).FirstOrDefault();
            var email = db.HrdEmployees.Where(x => x.RecordId == result.lead_owner).FirstOrDefault();
            sendEmail sendEmail = new sendEmail();
            sendEmail.EmailSend("Reminder Created for " +result.company+","+"\n\n",email.Email,"This is gentle remainder about scheduled meeting. Please find the details below:"+"\n\n"+"Reminder Name:"+remainderDto.ReminderName+"\n\n"+"Reminder Date:"+remainderDto.ReminderDate+"\n\n"+"Reminder Time:"+remainderDto.ReminderTime+"\n\n"+"Thanks"+"\n\n"+"Cortracker");

            // Return a success message with the ID of the created remainder
            return Ok(new { message = "Remainder saved successfully", remainderId = crmRemainder.RecordId });
        }

        [HttpPost]
        [Authorize]
        [Route("GetAllRemainders")]
        public IActionResult GetAllRemainders([FromBody] dynamic model)
        {
            
            CrmRemainderDto crmreminderdto = JsonConvert.DeserializeObject<CrmRemainderDto>(model.ToString());
            if (crmreminderdto.customer_id > 0)
            {
                var remainders = db.CrmRemainders.Where(x => x.BranchId == crmreminderdto.BranchId && x.CustomerCode == crmreminderdto.CustomerCode && x.customer_id == crmreminderdto.customer_id).Select(r => new CrmRemainderDto
                {
                    RecordId = r.RecordId,
                    ReminderName = r.ReminderName,
                    ReminderDate = r.ReminderDate,
                    ReminderTime = r.ReminderTime,
                    Notes = r.Notes,
                    leadid = r.leadid,
                    BranchId = r.BranchId,
                    CustomerCode = r.CustomerCode,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate = r.ModifiedDate
                }).ToList();
                return Ok(remainders);
            }
            else
            {
                var remainders = db.CrmRemainders.Where(x => x.BranchId == crmreminderdto.BranchId && x.CustomerCode == crmreminderdto.CustomerCode && x.leadid == crmreminderdto.leadid).Select(r => new CrmRemainderDto
                {
                    RecordId = r.RecordId,
                    ReminderName = r.ReminderName,
                    ReminderDate = r.ReminderDate,
                    ReminderTime = r.ReminderTime,
                    Notes = r.Notes,
                    leadid = r.leadid,
                    BranchId = r.BranchId,
                    CustomerCode = r.CustomerCode,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate = r.ModifiedDate
                }).ToList();
                return Ok(remainders);
            }
            return null;
           
        }


        [HttpPost]
        [Authorize]
        [Route("UpdateReminder")]
        public IActionResult UpdateReminder([FromBody] dynamic model)
        {
            // Deserialize the incoming dynamic model to a specific DTO
            CrmRemainderDto remainderDto = JsonConvert.DeserializeObject<CrmRemainderDto>(model.ToString());

            if (remainderDto == null || remainderDto.RecordId <= 0)
            {
                return BadRequest("Invalid reminder data or missing RecordId.");
            }

            // Fetch the existing reminder using the RecordId
            var remainderRecord = db.CrmRemainders.Where(x => x.RecordId == remainderDto.RecordId).FirstOrDefault();

            if (remainderRecord == null)
            {
                return NotFound("Reminder record not found.");
            }

            remainderRecord.leadid = remainderDto.leadid;
            // Update the existing reminder record with new values from the DTO
            remainderRecord.ReminderName = remainderDto.ReminderName;
            remainderRecord.ReminderDate = remainderDto.ReminderDate;
            remainderRecord.ReminderTime = remainderDto.ReminderTime;
            remainderRecord.reminder_type = remainderDto.reminder_type;
            remainderRecord.Notes = remainderDto.Notes;
            remainderRecord.ModifiedBy = remainderDto.ModifiedBy; // Assuming ModifiedBy is passed in DTO
            remainderRecord.ModifiedDate = DateTime.UtcNow; // Set the modified date to now

            // Update the record in the database
            db.CrmRemainders.Update(remainderRecord);
            db.SaveChanges();

            return Ok(new { message = "Reminder updated successfully" });
        }

        [HttpGet]
        [Authorize]
        [Route("GetReminderById/{id}")]
        public IActionResult GetReminderById(int id)
        {
            // Fetch the reminder details by Id
            var reminderDetails = db.CrmRemainders
                .Where(r => r.RecordId == id)
                .Select(r => new CrmRemainderDto
                {
                    RecordId = r.RecordId,
                    ReminderName = r.ReminderName,
                    ReminderDate = r.ReminderDate,
                    ReminderTime = r.ReminderTime,
                    Notes = r.Notes,
                    BranchId = r.BranchId,
                    CustomerCode = r.CustomerCode,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate = r.ModifiedDate,
                    reminder_type=r.reminder_type
                })
                .FirstOrDefault();

            // Return the reminder details if found, or a not found response if not
            if (reminderDetails != null)
            {
                return Ok(reminderDetails);
            }
            else
            {
                return NotFound("Reminder details not found.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("DeleteReminderById")]
        public IActionResult DeleteReminderById([FromBody] dynamic model)
        {
            try
            {
                // Deserialize the incoming model to the Reminder DTO
                CrmRemainderDto reminderDto = JsonConvert.DeserializeObject<CrmRemainderDto>(model.ToString());

                // Find the reminder record by RecordId
                var reminderDetails = db.CrmRemainders.FirstOrDefault(x => x.RecordId == reminderDto.RecordId);

                // Check if the reminder record exists
                if (reminderDetails == null)
                {
                    // Log not found and return a 404 response
                    Console.WriteLine($"Reminder with RecordId {reminderDto.RecordId} not found.");
                    return NotFound(new { message = "Reminder details not found." });
                }

                // Remove the reminder record from the database
                db.CrmRemainders.Remove(reminderDetails);
                db.SaveChanges();

                // Return a success message
                return Ok(new { message = "Reminder details deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error deleting reminder: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the reminder." });
            }
        }


    }
}

