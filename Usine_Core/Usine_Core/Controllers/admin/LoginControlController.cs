using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Usine_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Usine_Core.others;
using Usine_Core.ModelsAdmin;
using Newtonsoft.Json;
using Usine_Core.others.dtos;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Usine_Core.Controllers.Admin
{
    public class ProductInfo
    {
        public string productcode { get; set; }
        public string customercode { get; set; }
    }
    public class ChangePassword
    {
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
        public UserInfo usr { get; set; }
    }
    public class LoginControlController : ControllerBase
    {
        UsineContext db = new UsineContext();
        StringConversions sc = new StringConversions();



        [HttpPost]
        [Route("api/LoginControl/LoginVerification")]
        public UserCompleteInfo LoginVerification([FromBody] LoginControls usr)
        {
            UserCompleteInfo uc = new UserCompleteInfo();
            AdminControl ac = new AdminControl();

            var newUser = sc.makeStringToAscii(usr.userName.ToLower());
            var newpwd = sc.makeStringToAscii(usr.password);

            var log = db.UsrAut.Where(a => a.CustomerCode == usr.customerCode && a.UsrName == newUser && a.Pwd == newpwd && a.Pos > 0).FirstOrDefault();

            if (log == null)
            {
                return null;
            }
            else
            {
                String[] roles = { "Ravi", "Raja", "Gopal", "Ananth", "Ratnam", "Nagendra" };

                UserInfo uinf = new UserInfo();
                uinf.uCode = usr.userName.ToUpper();
                uinf.rCode = sc.makeAsciitoString(log.RoleName);
                uinf.cCode = usr.customerCode;
                uinf.kCode = GenerateAccessToken(log.Email, Guid.NewGuid().ToString(), roles);
                uinf.vCode = AdminControl.versionNumber;
                uinf.pCode = findProductCode(usr.customerCode);//"D-VEN";
                uinf.webCheck = log.WebFreeEnable;
                uinf.mobileCheck = log.MobileFreeEnable;
                uc.roles = findRoles(uinf.rCode, usr.customerCode);
                uc.assg = findAssignings(uinf.uCode, uinf.rCode, usr.customerCode);

                var br = uc.assg.Where(b => b.typ == "BRA").FirstOrDefault();
                if (br != null)
                {
                    uinf.bCode = br.code;
                }
                else
                {
                    uinf.bCode = "E001";
                }


                uc.usr = uinf;

                uc.addr = makeBranchAddress(uinf.bCode, uinf.cCode);
                var pursets = (from a in db.PurSetup.Where(a => a.BranchId == uinf.bCode && a.CustomerCode == uinf.cCode)
                               select new PurSetup
                               {
                                   SetupCode = a.SetupCode,
                                   SetupDescription = a.SetupDescription,
                                   SetupValue = a.SetupValue,
                                   BranchId = "PUR"
                               }).ToList();
                var crmsets = (from a in db.CrmSetup.Where(a => a.BranchId == uinf.bCode && a.CustomerCode == uinf.cCode)
                               select new PurSetup
                               {
                                   SetupCode = a.SetupCode,
                                   SetupDescription = a.PosDescription,
                                   SetupValue = a.Pos,
                                   BranchId = "CRM"
                               }).ToList();

                var sets = pursets.Union(crmsets).ToList();
                uc.sets = sets;
                if (uc.addr.country != "REGPROB")
                {
                    usrVisitorTrack usrVisitor = new usrVisitorTrack();
                    usrVisitor.customer_code = Convert.ToString(usr.customerCode);
                    usrVisitor.customer_visit_date = DateTime.Now;
                    db.usrVisitorTrack.Add(usrVisitor);
                    db.SaveChanges();
                }
                return uc;
            }
        }

        private List<UserRoles> findRoles(String rCode, int cCode)
        {
            UsineContext db = new UsineContext();
            List<UserRoles> uroles = new List<UserRoles>();
            var roles = db.Admroles.Where(a => a.RoleName == rCode && a.CustomerCode == cCode && a.Pos == 1);
            foreach (Admroles role in roles)
            {
                uroles.Add(new UserRoles { moduleCode = role.ModuleCode, menuCode = role.MenuCode, screenCode = role.ScreenCode, transCode = role.TransCode });
            }
            return uroles;
        }

        [HttpGet]
        [Route("api/LoginControl/makeBranchAddress/{bCode}/{cCode}")]
        public UserAddress makeBranchAddress(string bCode, int cCode)
        {



            PrismProductsAdminContext dc = new PrismProductsAdminContext();
            AdminControl ac = new AdminControl();
            var det = dc.CustomerRegistrations.Where(a => a.CustomerId == cCode).FirstOrDefault();
            UserAddress addr = new UserAddress();
            DateTime dat = ac.getPresentDateTime();
            if (det != null)
            {
                if (det.ExpDate.Value >= dat)
                {
                    addr.company = det.Customer;
                    addr.branchName = det.Customer;
                    addr.address = det.Addr;
                    addr.country = det.Country;
                    addr.stat = det.Stat;
                    addr.district = det.District;
                    addr.city = det.City;
                    addr.zip = det.Zip;
                    addr.mobile = det.Mobile;
                    addr.tel = det.Tel;
                    addr.fax = det.Fax;
                    addr.email = det.Email;
                    addr.web = det.Web;
                }
                else
                {
                    addr.country = "REGPROB";
                }
            }
            else
            {
                addr = null;
            }

            return addr;
        }
        [HttpPost]
        [Route("api/LoginControl/logChangePassword")]
        public TransactionResult logChangePassword([FromBody] ChangePassword det)
        {
            string result = "";
            TransactionResult res = new TransactionResult();
            AdminControl ac = new AdminControl();
            try
            {
                string oldpass = sc.makeStringToAscii(det.oldpassword);
                string newpass = sc.makeStringToAscii(det.newpassword);
                string user = sc.makeStringToAscii(det.usr.uCode.ToLower());
                var userdet = db.UsrAut.Where(a => a.UsrName == user && a.CustomerCode == det.usr.cCode).FirstOrDefault();
                if (userdet != null)
                {
                    if (userdet.Pwd == oldpass)
                    {
                        userdet.Pwd = newpass;
                        db.SaveChanges();
                        result = "OK";
                    }
                    else
                    {
                        result = "Incorrect current password";
                    }
                }
                else
                {
                    result = "No user is existed";
                }

            }
            catch (Exception ee)
            {
                result = ee.Message;
            }
            res.result = result;
            return res;
        }
        private List<UserAssignings> findAssignings(string uCode, String rCode, int cCode)
        {
            List<UserAssignings> assgs = new List<UserAssignings>();

            UsineContext db = new UsineContext();
            PrismProductsAdminContext dc = new PrismProductsAdminContext();
            if (rCode.ToUpper() == "ADMINISTRATOR")
            {
                var accs = db.FinAccounts.Where(a => (a.AcType == "CAS" || a.AcType == "BAN" || a.AcType == "MOB") && a.CustomerCode == cCode);
                foreach (FinAccounts acc in accs)
                {
                    assgs.Add(new UserAssignings { code = acc.Recordid.ToString(), detail = acc.Accname, typ = acc.AcType });
                }
                var outlets = db.ResOutletMaster.Where(a => a.CustomerCode == cCode);
                foreach (ResOutletMaster outlet in outlets)
                {
                    assgs.Add(new UserAssignings { code = outlet.RestaCode, detail = outlet.RestaName, typ = "RES" });
                }
                var branches = dc.CustomerBranches.Where(a => a.CustomerId == cCode).OrderBy(b => b.BranchId).ToList(); //db.Sertaxes4.Where(a => a.SerTax4 == sc.makeStringToAsciiCustomer(cCode.ToString()) && a.SerTax1 == sc.makeStringToAsciiCustomer("bra_nam")).OrderBy(b => b.SerTax3);
                foreach (var branch in branches)
                {
                    assgs.Add(new UserAssignings { code = branch.BranchId, detail = branch.BranchName, typ = "BRA" });
                }

            }
            else
            {
                var furs = db.AdmUserwiseAssigns.Where(a => a.UserName.ToUpper() == uCode && a.CustomerCode == cCode).ToList();
                foreach (var fur in furs)
                {
                    assgs.Add(new UserAssignings { code = fur.AssignedId, typ = fur.AssignedTyp });
                }
            }
            return assgs;
        }
        private String findProductCode(int cCode)
        {
            PrismProductsAdminContext dc = new PrismProductsAdminContext();
            var det = dc.CustomerRegistrations.Where(a => a.CustomerId == cCode).FirstOrDefault();
            if (det != null)
            {
                return det.ProductId;
            }
            else
            {
                return " ";
            }
        }
        [HttpGet]
        [Authorize]
        [Route("api/LoginControl/GetCustomerRegistration")]
        public CustomerRegistrations GetCustomerRegistration(int cCode)
        {
            PrismProductsAdminContext dc = new PrismProductsAdminContext();
            return dc.CustomerRegistrations.Where(a => a.CustomerId == cCode).FirstOrDefault();
        }
        [HttpGet]
        [Authorize]
        [Route("api/LoginControl/GetCustomerRegistrationTrails")]
        public List<CustomerRegistrations> GetCustomerRegistrationTrails(int cCode)
        {
            PrismProductsAdminContext dc = new PrismProductsAdminContext();
            return dc.CustomerRegistrations.ToList();
        }
        [HttpPost]
        [Authorize]
        [Route("api/LoginControl/UpdateRegistrationTrails")]
        public List<CustomerRegistrations> UpdateRegistrationTrails([FromBody] CustomerRegistrations cus)
        {
            PrismProductsAdminContext dc = new PrismProductsAdminContext();
            var result = dc.CustomerRegistrations.Where(x => x.CustomerId == cus.CustomerId).FirstOrDefault();
            if (result != null)
            {
                result.dispatch_email = cus.dispatch_email;
                result.ExpDate = cus.ExpDate;
                result.RegDate = cus.RegDate;
                result.companyName = cus.companyName;
                result.Customer = cus.Customer;
                result.Addr = cus.Addr;
                result.Country = cus.Country;
                result.Stat = cus.Stat;
                result.City = cus.City;
                result.District = cus.District;
                result.Zip = cus.Zip;
                result.mode = cus.mode;
                result.ProductId = "D-USI";
                result.Mobile = cus.Mobile;
                result.Tel = cus.Tel;
                result.Email = cus.Email;
                result.Fax = cus.Fax;
                result.ownerName = cus.ownerName;
                result.ownerEmail = cus.ownerEmail;
                result.pocName = cus.pocName;
                result.pocEmail = cus.pocEmail;
                result.headCount = cus.headCount;
                result.websiteURL = cus.websiteURL;
                result.industry = cus.industry;
                result.location = cus.location;

                dc.CustomerRegistrations.Update(result);
                dc.SaveChanges();
            }
            return dc.CustomerRegistrations.ToList();
        }
        [HttpPost]
        [Route("api/LoginControl/sendEmail")]
        public IActionResult sendEmail([FromBody] dynamic model)
        {
            try
            {
                CustomerRegDto customerRegDto = JsonConvert.DeserializeObject<CustomerRegDto>(model.ToString());
                if (customerRegDto != null)
                {
                    CustomerRegistrations customerRegistrations = new CustomerRegistrations();
                    customerRegistrations.companyName = customerRegDto.companyName;
                    customerRegistrations.ownerName = customerRegDto.ownerName;
                    customerRegistrations.ownerEmail = customerRegDto.ownerEmail;
                    customerRegistrations.pocName = customerRegDto.pocName;
                    customerRegistrations.pocEmail = customerRegDto.pocEmail;
                    customerRegistrations.headCount = customerRegDto.headCount;
                    customerRegistrations.websiteURL = customerRegDto.websiteURL;
                    customerRegistrations.noofLogins = customerRegDto.noofLoggin;
                    customerRegistrations.industry = customerRegDto.industry;
                    customerRegistrations.location = customerRegDto.location;
                    customerRegistrations.contactNo = customerRegDto.phoneNumber;
                    customerRegistrations.Country = customerRegDto.Country;
                    customerRegistrations.ExpDate = DateTime.Now.AddDays(7);
                    customerRegistrations.Dat = DateTime.Now;
                    //customerRegistrations.productId = "D-USI";
                    if (customerRegDto.Country == "US") {
                        customerRegistrations.websiteURL = "https://ussales.cortracker360.com";
                    }
                    else
                    {
                        if (customerRegDto.industry.Trim() == "Textile Industry")
                        {
                            customerRegistrations.websiteURL = "https://crt.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Interior Design  Industry")
                        {
                            customerRegistrations.websiteURL = "https://interior.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "BPO Industry")
                        {
                            customerRegistrations.websiteURL = "https://bpo.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Food Manufacturing Industry")
                        {
                            customerRegistrations.websiteURL = "https://rdw.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Chemicals Industry")
                        {
                            customerRegistrations.websiteURL = "https://chemicals.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Home Automation Industry")
                        {
                            customerRegistrations.websiteURL = "https://ha.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Furniture Industry")
                        {
                            customerRegistrations.websiteURL = "https://rdw.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Plastics Manufacturing Industry")
                        {
                            customerRegistrations.websiteURL = "https://rdw.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Machinery Manufacturing Industry")
                        {
                            customerRegistrations.websiteURL = "https://machinery.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Bottle Manufacturing Industry")
                        {
                            customerRegistrations.websiteURL = "https://bottle.cortracker360.com";
                        }
                        else if (customerRegDto.industry.Trim() == "Discrete Manufacturing Industry")
                        {
                            customerRegistrations.websiteURL = "https://rdw.cortracker360.com";
                        }
                        else
                        {
                            customerRegistrations.websiteURL = "https://sales.cortracker360.com";
                        }
                    }
                    customerRegistrations.userName = "sa";
                    customerRegistrations.password = "C0rtr@ck3r@2024@0124";
                    customerRegistrations.servername = "192.168.29.53,49792";
                    customerRegistrations.databasename = "SalesDemoDB";
                    if (customerRegDto.Country == "US")
                    {
                        customerRegistrations.urldet = "https://ussales.cortracker360.com";
                    }
                    else
                    {
                        if(customerRegDto.industry== "Textile Industry")
                        {
                            customerRegistrations.urldet = "https://crt.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Interior Design  Industry")
                        {
                            customerRegistrations.urldet = "https://interior.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "BPO Industry")
                        {
                            customerRegistrations.urldet = "https://bpo.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Food Manufacturing Industry")
                        {
                            customerRegistrations.urldet = "https://rdw.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Chemicals Industry")
                        {
                            customerRegistrations.urldet = "https://chemicals.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Home Automation Industry")
                        {
                            customerRegistrations.urldet = "https://ha.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Furniture Industry")
                        {
                            customerRegistrations.urldet = "https://rdw.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Plastics Manufacturing Industry")
                        {
                            customerRegistrations.urldet = "https://rdw.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Machinery Manufacturing Industry")
                        {
                            customerRegistrations.urldet = "https://machinery.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Bottle Manufacturing Industry")
                        {
                            customerRegistrations.urldet = "https://bottle.cortracker360.com";
                        }
                        else if (customerRegDto.industry == "Discrete Manufacturing Industry")
                        {
                            customerRegistrations.urldet = "https://rdw.cortracker360.com";
                        }
                        else { 
                        customerRegistrations.urldet = "https://sales.cortracker360.com";
                        }
                    }
                    customerRegistrations.DefaultUser = "admin";
                    customerRegistrations.RegDate = DateTime.Now;
                    customerRegistrations.Customer = customerRegDto.ownerName;
                    customerRegistrations.Email = customerRegDto.Email;
                    customerRegistrations.Mobile = customerRegDto.Mobile;
                    customerRegistrations.ProductId = "D-USI";
                    customerRegistrations.MaxBranches = 1;
                    customerRegistrations.MaxOutlets = 1;
                    customerRegistrations.CurrencySymbol = "/-";
                    customerRegistrations.Currency = "Rupees";
                    customerRegistrations.Coins = "Paisa";
                    customerRegistrations.Schem = "yearly";
                    customerRegistrations.other = customerRegDto.other;
                    customerRegistrations.Fiscal = DateTime.Now.ToString("dd-MMM-yyyy");
                    PrismProductsAdminContext dc = new PrismProductsAdminContext();
                    dc.CustomerRegistrations.Add(customerRegistrations);
                    dc.SaveChanges();
                    UsrAut usrAut = new UsrAut();
                    // StringConversions stringConversions = new StringConversions();
                    customerRegistrations.userName = "admin";
                    customerRegistrations.password = CreateRandomPassword(8);
                    StringConversions stringConversions = new StringConversions();
                    usrAut.UsrName = stringConversions.makeStringToAscii("admin");
                    usrAut.RoleName = stringConversions.makeStringToAscii("administrator");
                    usrAut.Pwd = stringConversions.makeStringToAscii(customerRegistrations.password);
                    usrAut.Pos = 1;
                    usrAut.Email = "test@gmail.com";
                    usrAut.CustomerCode = customerRegistrations.CustomerId;
                    usrAut.WebFreeEnable = 1;
                    db.UsrAut.Add(usrAut);
                    db.SaveChanges();
                    sendEmail sendEmail = new sendEmail();
                    sendEmail.EmailSend("Get Started With Cortracker", customerRegDto.ownerEmail, "Hi " + customerRegDto.ownerName + ",\n\n" + "Welcome to Your Cortracker ERP Software Trial. \n\n" + "Thank you for registering for the trial version of our ERP software. We are pleased to have you on board and look forward to demonstrating how our solution can meet your needs.\r\n\r\nYour trial period will be active for the next 7 days. Should you have any questions or require assistance during this time, please do not hesitate to contact us. Our team is here to support you and ensure you fully explore the capabilities of our software..\n\n" + "Customer Code:" + customerRegistrations.CustomerId + "\n\n" + "Username:" + customerRegistrations.userName + "\n\n" + "Password:" + customerRegistrations.password + "\n\n" + "Url for Login: " + customerRegistrations.websiteURL + "\n \n For any additional information, please contact us accordingly.\n \n Thanks,\n \n Cortracker", null, "durgaprasad@cortracker360.com", "erp@cortracker360.com");
                    //string username = stringConversions.makeAsciitoString("108011101030103010501100100010501030105011600970108009701000109010501100");
                    //string password = stringConversions.makeAsciitoString("04900670089005000650083008000660");
                    //string rolename = stringConversions.makeAsciitoString("0970100010901050110010501150116011400970116011101140");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }

        }
        [HttpPost]
        [Route("api/LoginControl/sendCommonEmail")]
        public IActionResult sendCommonEmail([FromBody] dynamic model)
        {
            try
            {
                CustomerRegDto customerRegDto = JsonConvert.DeserializeObject<CustomerRegDto>(model.ToString());
                if (customerRegDto != null)
                {
                    CustomerRegistrations customerRegistrations = new CustomerRegistrations();
                    customerRegistrations.companyName = customerRegDto.companyName;
                    customerRegistrations.ownerName = customerRegDto.ownerName;
                    customerRegistrations.ownerEmail = customerRegDto.ownerEmail;
                    customerRegistrations.pocName = customerRegDto.pocName;
                    customerRegistrations.pocEmail = customerRegDto.pocEmail;
                    customerRegistrations.headCount = customerRegDto.headCount;
                    customerRegistrations.websiteURL = customerRegDto.websiteURL;
                    customerRegistrations.noofLogins = customerRegDto.noofLoggin;
                    customerRegistrations.industry = customerRegDto.industry;
                    customerRegistrations.location = customerRegDto.location;
                    customerRegistrations.contactNo = customerRegDto.phoneNumber;
                    customerRegistrations.Country = customerRegDto.Country;
                    customerRegistrations.ExpDate = DateTime.Now.AddDays(7);
                    customerRegistrations.Dat = DateTime.Now;
                    //customerRegistrations.productId = "D-USI";
                    if (customerRegDto.product == "crm")
                    {
                        customerRegistrations.websiteURL = "https://corcrm.cortracker360.com/";
                    }
                    else if (customerRegDto.product == "inventory")
                    {
                        customerRegistrations.websiteURL = "https://5xdemo.cortracker.com/";
                    }
                    else if (customerRegDto.product == "purchases")
                    {
                        customerRegistrations.websiteURL = "https://purchases.cortracker360.com";
                    }
                    else if (customerRegDto.product == "production")
                    {
                        customerRegistrations.websiteURL = "https://production.cortracker360.com";
                    }
                    else if (customerRegDto.product == "sales")
                    {
                        customerRegistrations.websiteURL = "https://corsales.cortracker360.com";
                    }
                    customerRegistrations.userName = "sa";
                    customerRegistrations.password = "C0rtr@ck3r@2024@0124";
                    customerRegistrations.servername = "192.168.29.53,49792";
                    customerRegistrations.databasename = "SalesDemoDB";
                    if (customerRegDto.product == "crm")
                    {
                        customerRegistrations.urldet = "https://corcrm.cortracker360.com/";
                    }
                    else if (customerRegDto.product == "inventory")
                    {
                        customerRegistrations.urldet = "https://5xdemo.cortracker.com/";
                    }
                    else if (customerRegDto.product == "purchases")
                    {
                        customerRegistrations.urldet = "https://purchases.cortracker360.com";
                    }
                    else if (customerRegDto.product == "production")
                    {
                        customerRegistrations.urldet = "https://production.cortracker360.com";
                    }
                    else if (customerRegDto.product == "sales")
                    {
                        customerRegistrations.urldet = "https://corsales.cortracker360.com";
                    }
                    customerRegistrations.DefaultUser = "admin";
                    customerRegistrations.RegDate = DateTime.Now;
                    customerRegistrations.Customer = customerRegDto.ownerName;
                    customerRegistrations.Email = customerRegDto.Email;
                    customerRegistrations.Mobile = customerRegDto.Mobile;
                    customerRegistrations.ProductId = "D-USI";
                    customerRegistrations.MaxBranches = 1;
                    customerRegistrations.MaxOutlets = 1;
                    customerRegistrations.CurrencySymbol = "/-";
                    customerRegistrations.Currency = "Rupees";
                    customerRegistrations.Coins = "Paisa";
                    customerRegistrations.Schem = "yearly";
                    customerRegistrations.other = customerRegDto.other;
                    customerRegistrations.Fiscal = DateTime.Now.ToString("dd-MMM-yyyy");
                    PrismProductsAdminContext dc = new PrismProductsAdminContext();
                    dc.CustomerRegistrations.Add(customerRegistrations);
                    dc.SaveChanges();
                    UsrAut usrAut = new UsrAut();
                    StringConversions stringConversions = new StringConversions();
                    if (customerRegDto.product == "inventory")
                    {
                        customerRegistrations.userName = "manager";
                        customerRegistrations.password = "Test@123456";
                        usrAut.UsrName = stringConversions.makeStringToAscii("manager");
                        usrAut.RoleName = stringConversions.makeStringToAscii("administrator");
                        usrAut.Pwd = stringConversions.makeStringToAscii(customerRegistrations.password);
                        usrAut.Pos = 1;
                        usrAut.Email = "test@gmail.com";
                        usrAut.CustomerCode = customerRegistrations.CustomerId;
                        usrAut.WebFreeEnable = 1;
                    }
                    else
                    {
                        // StringConversions stringConversions = new StringConversions();
                        customerRegistrations.userName = "admin";
                        customerRegistrations.password = CreateRandomPassword(8);

                        usrAut.UsrName = stringConversions.makeStringToAscii("admin");
                        usrAut.RoleName = stringConversions.makeStringToAscii("administrator");
                        usrAut.Pwd = stringConversions.makeStringToAscii(customerRegistrations.password);
                        usrAut.Pos = 1;
                        usrAut.Email = "test@gmail.com";
                        usrAut.CustomerCode = customerRegistrations.CustomerId;
                        usrAut.WebFreeEnable = 1;
                    }
                    db.UsrAut.Add(usrAut);
                    db.SaveChanges();
                    sendEmail sendEmail = new sendEmail();
                    sendEmail.EmailSend("Get Started With Cortracker", customerRegDto.ownerEmail, "Hi " + customerRegDto.ownerName + ",\n\n" + "Welcome to Your Cortracker ERP Software Trial. \n\n" + "Thank you for registering for the trial version of our ERP software. We are pleased to have you on board and look forward to demonstrating how our solution can meet your needs.\r\n\r\nYour trial period will be active for the next 7 days. Should you have any questions or require assistance during this time, please do not hesitate to contact us. Our team is here to support you and ensure you fully explore the capabilities of our software..\n\n" + "Customer Code:" + customerRegistrations.CustomerId + "\n\n" + "Username:" + customerRegistrations.userName + "\n\n" + "Password:" + customerRegistrations.password + "\n\n" + "Url for Login: " + customerRegistrations.websiteURL + "\n \n For any additional information, please contact us accordingly.\n \n Thanks,\n \n Cortracker", null, "durgaprasad@cortracker360.com", "erp@cortracker360.com");
                    //string username = stringConversions.makeAsciitoString("108011101030103010501100100010501030105011600970108009701000109010501100");
                    //string password = stringConversions.makeAsciitoString("04900670089005000650083008000660");
                    //string rolename = stringConversions.makeAsciitoString("0970100010901050110010501150116011400970116011101140");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }

        }
        [HttpPost]
        [Route("api/LoginControl/scheduledemo")]
        public IActionResult scheduledemo([FromBody] dynamic model)
        {
            try
            {
                scheduleDto scheduleDtos = JsonConvert.DeserializeObject<scheduleDto>(model.ToString());
                sendEmail sendEmail = new sendEmail();
                //sales people will know the details from erp@cortracker360.com
                sendEmail.EmailSend("Book A Demo With Cortracker", "erp@cortracker360.com", "Hi," + "\n\n" + "Below are the company details:" + "\n\n" + "First Name:" + scheduleDtos.firstName + "\n\n" + "Last Name:" + scheduleDtos.lastName + "\n\n" + "Email:" + scheduleDtos.email + "\n\n" + "Phone Number:" + scheduleDtos.phoneNumber + "\n\n" + "Company:" + scheduleDtos.company + "\n\n" + "Product:" + scheduleDtos.product + "\n\n" + "Message:" + scheduleDtos.message + "\n\n" + "Thanks,\nCortracker", null, "durgaprasad@cortracker360.com", "erp@cortracker360.com");
                sendEmail.EmailSend("Book A Demo With Cortracker", scheduleDtos.email, "Hi " + scheduleDtos.firstName + "," + "\n\n" + "Thank you for contacting us. One of our sales team will get in touch with you soon. \n\n" + "If you have any questions you can directly speak with them.Kindly Send an email to ERP@CORTRACKER360.COM." + "\n\n" + "Thanks,\n Cortracker", null, "durgaprasad@cortracker360.com");
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }

        }
        [HttpPost]
        [Route("api/LoginControl/ValidateEmail")]
        public IActionResult ValidateEmail([FromBody] dynamic model)
        {
            try
            {
                PrismProductsAdminContext context = new PrismProductsAdminContext();
                CustomerRegDto customerRegDto = JsonConvert.DeserializeObject<CustomerRegDto>(model.ToString());
                var result = context.CustomerRegistrations.Where(x => x.ownerEmail == customerRegDto.ownerEmail).Count();
                if (result > 0)
                {
                    return Ok(new { message = "Email Id Already Exists!. Please Contact Sales Team!" });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }

        }
        [HttpPost]
        [Route("api/LoginControl/customEmailSave")]
        public IActionResult customEmailSave([FromBody] dynamic model)
        {
            try
            {
                custom_emailDtos custom_EmailDtos = JsonConvert.DeserializeObject<custom_emailDtos>(model.ToString());
                if (custom_EmailDtos != null)
                {
                    admCustomEmail admCustomEmail = new admCustomEmail();
                    admCustomEmail.CustomerCode = custom_EmailDtos.CustomerCode;
                    admCustomEmail.EmailCc = custom_EmailDtos.EmailCc;
                    admCustomEmail.EmailSmtp = custom_EmailDtos.EmailSmtp;
                    admCustomEmail.EmailDefault = custom_EmailDtos.EmailDefault;
                    admCustomEmail.EmailPort = custom_EmailDtos.EmailPort;
                    admCustomEmail.BranchId = custom_EmailDtos.BranchId;
                    admCustomEmail.EmailFrom = custom_EmailDtos.EmailFrom;
                    admCustomEmail.EmailPassword = custom_EmailDtos.EmailPassword;
                    admCustomEmail.EmailTo = custom_EmailDtos.EmailTo;
                   // db.AdmcustomEmail.Add(admCustomEmail);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }

        }
        //[HttpGet]
        //[Route("api/LoginControl/customEmailSaveBycustomer")]
        //public IActionResult customEmailSaveBycustomer(string customercode)
        //{
        //    var result = db.AdmcustomEmail.Where(x => x.CustomerCode == customercode).ToList();
        //    return Ok(result);
        //}
        //[HttpGet]
        //[Route("api/LoginControl/customEmailBodygetBycustomer")]
        //public IActionResult customEmailBodygetBycustomer(string customercode)
        //{
        //    var result = db.admCustomBody.Where(x=>x.CustomerCode==customercode).Select(x=>new custom_emailbodyDto
        //    {
        //        admCustomEmail=x.admCustomEmailId!=null? db.AdmcustomEmail.Where(y=>y.RecordId==x.admCustomEmailId).FirstOrDefault().EmailFrom:null,
        //        customSubject=x.customSubject,
        //        customBody=x.customBody,
        //        IsDefault=x.IsDefault,
        //        titleName=x.titleName,
        //        RecordId=x.RecordId,
        //    }).ToList();
        //    return Ok(result);
        //}
        //[HttpGet]
        //[Route("api/LoginControl/customEmailsignaturegetBycustomer")]
        //public IActionResult customEmailsignaturegetBycustomer(string customercode)
        //{
        //    var result = db.admCustomSignature.Where(x => x.CustomerCode == customercode).Select(x=>new custom_emailsignatureDto
        //    {
        //        admCustomEmail =x.admCustomEmailId!=null? db.AdmcustomEmail.Where(y => y.RecordId == x.admCustomEmailId).FirstOrDefault().EmailFrom:null,
        //        customSignature=x.customSignature,
        //        customSignatureText=x.customSignatureText,
        //        IsDefault=x.IsDefault,
        //        RecordId = x.RecordId,
        //    }).ToList();
        //    return Ok(result);
        //}
        [HttpPost]
        [Route("api/LoginControl/saveCustomBody")]
        public IActionResult saveCustomBody([FromBody] dynamic model)
        {
            try
            {
                custom_emailbodyDto custom_EmailbodyDto = JsonConvert.DeserializeObject<custom_emailbodyDto>(model.ToString());

                if (custom_EmailbodyDto != null)
                {
                    admCustomBody admCustomBody = new admCustomBody();

                    admCustomBody.admCustomEmailId = custom_EmailbodyDto.admCustomEmailId;
                    admCustomBody.titleName = custom_EmailbodyDto.titleName;
                    admCustomBody.BranchId = custom_EmailbodyDto.BranchId;
                    admCustomBody.customSubject = custom_EmailbodyDto.customSubject;
                    admCustomBody.customBody = custom_EmailbodyDto.customBody;
                    admCustomBody.IsDefault = custom_EmailbodyDto.IsDefault;
                    admCustomBody.CustomerCode = custom_EmailbodyDto.CustomerCode;
                   
                    db.admCustomBody.Add(admCustomBody);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }
        }
        [HttpPost]
        [Route("api/LoginControl/saveCustomsignature")]
        public IActionResult saveCustomsignature([FromBody] dynamic model)
        {
            try
            {
                custom_emailsignatureDto custom_EmailsignatureDto = JsonConvert.DeserializeObject<custom_emailsignatureDto>(model.ToString());

                if (custom_EmailsignatureDto != null)
                {
                    admCustomSignature admCustomSignature = new admCustomSignature();

                    admCustomSignature.admCustomEmailId = custom_EmailsignatureDto.admCustomEmailId;
                    admCustomSignature.BranchId = custom_EmailsignatureDto.BranchId;
                    admCustomSignature.customSignature = custom_EmailsignatureDto.customSignature;
                    admCustomSignature.customSignatureText = custom_EmailsignatureDto.customSignatureText;
                    admCustomSignature.IsDefault = custom_EmailsignatureDto.IsDefault;
                    admCustomSignature.CustomerCode = custom_EmailsignatureDto.CustomerCode;

                    db.admCustomSignature.Add(admCustomSignature);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(null);
            }
        }

        private static string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, and special characters that are allowed in the password
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        private string GenerateAccessToken(string email, string userId, string[] roles)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("secretsecretsecret"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, email)
            };

            claims = claims.Concat(roles.Select(role => new Claim(ClaimTypes.Role, role))).ToList();


            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "issuer",
                "audience",
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public Task<string> Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

    }
}
