using PDP_Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PDP_Payroll.Controllers
{
    public class HomeController : Controller
    {
        PDP_DbContext db = new PDP_DbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult Login()
        {
            if (Session["email"] == null)
            {
                return View();
            }
            else
            {
                //redirect to Manage profile Page
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {

            var user = db.User.Where(m => m.email == email && m.password == password).SingleOrDefault();
            if (user != null)
            {
                if (user.IsVerified)
                {
                    Session["email"] = email;
                    return RedirectToAction("index");
                    //var company = db.Company.Where(m=>m.user==user).SingleOrDefault();

                }
                else
                {
                    return RedirectToAction("VerifiedEmail", new { Email = user.email });
                }
            }
            ViewBag.LoginError = "Invalid Username or Password";
            return View(user);
        }
        public ActionResult Signup()
        {
            if (Session["email"] == null)
            {
                ViewBag.UserExist = "";
                return View();
            }
            else
            {
                //redirect to Manage profile Page
                return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(string email, string password)
        {
            UsersModel user = db.User.Where(m => m.email == email).SingleOrDefault();
            if (user == null)
            {
                user = new UsersModel();
                user.email = email;
                user.password = password;
                user.VerificationCode = Guid.NewGuid().ToString("N").Substring(0, 8);
                user.IsVerified = false;


                MailMessage mail = new MailMessage();
                mail.To.Add(user.email);
                mail.From = new MailAddress("bhatti9t7@gmail.com");
                mail.Subject = "PdP Verification";
                string Body = "Verification code : " + user.VerificationCode;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("bhatti9t7@gmail.com", "satellite17"); // Enter seders User name and password   
                smtp.EnableSsl = true;
                smtp.Send(mail);

                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("VerifiedEmail", new { Email = user.email });
            }
            else
            {
                ViewBag.UserExist = "User Already Exist";
            }
            return View(user);
        }
        public ActionResult VerifiedEmail(string Email)
        {
            if (Session["email"] == null)
            {
                var user = db.User.Where(m => m.email == Email && !m.IsVerified).SingleOrDefault();
                if (user != null)
                    return View((object)Email);

                return RedirectToAction("Login");
            }
            else
            {
                //redirect to Manage profile Page
                return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifiedEmail(string Email, string VerificationCode)
        {
            var user = db.User.Where(m => m.email == Email && m.VerificationCode == VerificationCode).SingleOrDefault();
            if (user != null)
            {
                user.IsVerified = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                Session["Email"] = Email;
                return RedirectToAction("Index");

            }
            ViewBag.VerificationCode = VerificationCode;
            ViewBag.VerificationCodeError = "Invalid Verification code";
            return View((object)Email);
        }
        public ActionResult ResendVerificationCode(string Email)
        {
            var user = db.User.Where(m => m.email == Email && !m.IsVerified).SingleOrDefault();
            if (user != null)
            {
                string newVerificationCode = Guid.NewGuid().ToString("N").Substring(0, 8);
                MailMessage mail = new MailMessage();
                mail.To.Add(user.email);
                mail.From = new MailAddress("bhatti9t7@gmail.com");
                mail.Subject = "PdP Verification";
                string Body = "Verification code : " + newVerificationCode;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("bhatti9t7@gmail.com", "satellite17"); // Enter seders User name and password   
                smtp.EnableSsl = true;
                smtp.Send(mail);
                user.VerificationCode = newVerificationCode;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();


            }
            return RedirectToAction("VerifiedEmail", new { Email = Email });
        }
        public ActionResult Logout()
        {
            if (Session["Email"] != null)
            {
                Session["Email"] = null;
                return RedirectToAction("index");
            }
            return null;
        }
        public ActionResult ChangePassword()
        {
            if (Session["Email"] != null)
            {
                return View((object)Session["Email"]);
            }
            else
            {

                return RedirectToAction("Login");
            }
            return null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string Email, string OldPassword, string new_password)
        {
            var user = db.User.Where(m => m.email == Email && m.password == OldPassword).SingleOrDefault();
            if (user != null)
            {
                user.password = new_password;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                Session["Email"] = null;
                return RedirectToAction("Login");
            }
            ViewBag.ChangePasswordError = "Invalid Old Password";
            return View((object)Email);
        }
        public ActionResult CompanyView()
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company != null)
                {
                    return View(company);
                }
                else
                {
                    return RedirectToAction("CompanyEdit");
                }
            }
            return RedirectToAction("Login");


        }
        public ActionResult CompanyEdit()
        {
            if (Session["Email"] != null)
            {

                ViewBag.state_id = new SelectList(db.State, "state_id", "state_name");
                string Email = Session["Email"].ToString();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company != null)
                {
                    return View(company);
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyEdit([Bind(Include = "company_id,state_id,company_name,company_address,company_Email,company_phone")] CompanyModel company)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                company.user_id = db.User.Where(m => m.email == Email).SingleOrDefault().user_id;
                if (company.company_id == 0)
                {
                    db.Company.Add(company);
                    db.SaveChanges();
                    company.company_id = db.Company.Max(m => m.company_id);
                    var branch = new BranchModel();
                    branch.company_id = company.company_id;
                    branch.state_id = company.state_id;
                    branch.branch_name = company.company_name + " Head Branch";
                    branch.branch_address = company.company_address;
                    branch.branch_email = company.company_Email;
                    branch.branch_phone = company.company_phone;
                    branch.user_id = company.user_id;
                    db.Branch.Add(branch);
                }
                else
                {
                    db.Entry(company).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("CompanyView");
            }
            return RedirectToAction("Login");
        }
        public ActionResult BranchList()
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company == null)
                {
                    return RedirectToAction("CompanyEdit");
                }
                else
                {
                    var branch = db.Branch.Where(m => m.user.email == Email && m.Company.user.email == Email).ToList();
                    return View(branch);
                }
            }
            return RedirectToAction("Login");

        }
        [HttpGet]
        public ActionResult BranchEdit(int? id)
        {
            if (Session["Email"] != null)
            {

                ViewBag.state_id = new SelectList(db.State, "state_id", "state_name");
                string Email = Session["Email"].ToString();
                if (id == null)
                {
                    return View();
                }
                else
                {
                    var branch = db.Branch.Where(m => m.branch_id == id).SingleOrDefault();
                    if (branch == null)
                    {
                        return RedirectToAction("BranchList");
                    }
                    else
                    {
                        return View(branch);
                    }
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchEdit([Bind(Include = "branch_id,branch_name,branch_address,branch_phone,branch_email,state_id")] BranchModel branch)
        {
            if (Session["Email"] != null)
            {

                ViewBag.state_id = new SelectList(db.State, "state_id", "state_name");
                string Email = Session["Email"].ToString();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                branch.company_id = company.company_id;
                branch.user_id = company.user_id;
                if (branch.branch_id > 0)
                {
                    db.Entry(branch).State = EntityState.Modified;

                }
                else
                {
                    db.Branch.Add(branch);
                }
                db.SaveChanges();
                return RedirectToAction("BranchList");

            }
            return RedirectToAction("Login");
        }
        public ActionResult BranchView(int? id)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.branch_id == id).SingleOrDefault();
                if (branch != null)
                {
                    return View(branch);
                }
                else
                {
                    return RedirectToAction("BranchList");
                }
            }
            return RedirectToAction("Login");


        }
        public ActionResult BranchDelete(int? id)
        {
            if (Session["Email"] != null)
            {

                ViewBag.state_id = new SelectList(db.State, "state_id", "state_name");
                string Email = Session["Email"].ToString();
                if (id != null)
                {
                    var branch = db.Branch.Where(m => m.branch_id == id).SingleOrDefault();
                    if (branch != null)
                    {
                        db.Branch.Remove(branch);
                        db.SaveChanges();
                    }
                    return RedirectToAction("BranchList");
                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult EmployeeEdit(int? id)
        {
            if (Session["Email"] != null)
            {

                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then Enter Employee Record";
                    return RedirectToAction("CompanyEdit");

                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then Enter Employee Record";
                    return RedirectToAction("BranchEdit");
                }
                else
                {
                    ViewBag.state_id = new SelectList(db.State, "state_id", "state_name");
                    ViewBag.branch_id = new SelectList(db.Branch, "branch_id", "branch_name");
                    if (id == null)
                    {
                        return View();
                    }
                    else
                    {
                        var employee = db.Employee.Where(m => m.employee_id == id).SingleOrDefault();
                        if (employee == null)
                        {
                            return RedirectToAction("Employeelist");
                        }
                        else
                        {
                            return View(employee);
                        }
                    }
                }

            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeEdit([Bind(Include = "employee_id,employee_name,employee_Fathername,employee_email,employee_phoneNo,employee_Address,employee_NIC,employee_MartialStatus,employee_NoOfDependency,employee_Wages,state_id,branch_id,Date_of_Join,Payroll_Type")]EmployeeModel employee)
        {
            if (Session["Email"] != null)
            {

                string Email = Session["Email"].ToString();
                employee.user_id = db.User.Where(m => m.email == Email).SingleOrDefault().user_id;
                if(employee.employee_id==0)
                {
                    db.Employee.Add(employee);
                }
                else
                {
                    db.Entry(employee).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("EmplyeeList");
            }
            return RedirectToAction("Login");
        }
        public ActionResult EmployeeList()
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");

                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else
                {
                    var employee = db.Employee.Where(m => m.user.email == Email).ToList();
                    return View(employee);
                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult EmployeeDetail(int ?id)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else
                {
                    var employee = db.Employee.Where(m => m.employee_id == id).SingleOrDefault();
                    if (employee == null)
                    {
                        return RedirectToAction("Employeelist");
                    }
                    else
                    {
                        return View(employee);
                    }
                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult EmployeeDelete(int? id)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else
                {
                    var employee = db.Employee.Where(m => m.employee_id == id).SingleOrDefault();
                    if (employee != null)
                    {
                        db.Employee.Remove(employee);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Employeelist");

                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult CalculatePayroll()
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                var Employee= db.Employee.Where(m => m.user.email == Email).ToList();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else if (Employee.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter Emplyee Then proceed to Calclulate Payroll";
                    return RedirectToAction("EmployeeEdit");
                }
                else
                {
                    return View(Employee);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalculatePayroll(PayRoll_ParentModel parent_Payroll,List<PayrollModel> child_Payroll)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                var Employee = db.Employee.Where(m => m.user.email == Email).ToList();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else if (Employee.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter Emplyee Then proceed to Calclulate Payroll";
                    return RedirectToAction("EmployeeEdit");
                }
                else
                {
                    parent_Payroll.user_id = db.User.Where(m => m.email == Email).SingleOrDefault().user_id;
                    db.PayRoll_Parent.Add(parent_Payroll);
                    db.SaveChanges();
                    foreach (var item in child_Payroll)
                    {
                        item.PayRoll_Parent_id= db.PayRoll_Parent.Max(m=>m.PayRoll_Parent_id); 
                        db.PayRoll.Add(item);
                        db.SaveChanges();
                    }
                    return RedirectToAction("PayrollList");
                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult EditPayroll(int? id)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                var Employee = db.Employee.Where(m => m.user.email == Email).ToList();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else if (Employee.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter Emplyee Then proceed to Calclulate Payroll";
                    return RedirectToAction("EmployeeEdit");
                }
                else
                {
                    var Payroll = db.PayRoll_Parent.Where(m => m.PayRoll_Parent_id == id).SingleOrDefault();
                    if(Payroll==null)
                    {
                        return RedirectToAction("PayrollList");
                    }
                    else
                    {
                        return View(Payroll);
                    }
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayroll(PayRoll_ParentModel parent_Payroll, List<PayrollModel> child_Payroll)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                var Employee = db.Employee.Where(m => m.user.email == Email).ToList();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else if (Employee.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter Emplyee Then proceed to Calclulate Payroll";
                    return RedirectToAction("EmployeeEdit");
                }
                else
                {
                    parent_Payroll.user_id = db.User.Where(m => m.email == Email).SingleOrDefault().user_id;
                    db.Entry(parent_Payroll).State = EntityState.Modified;
                    db.SaveChanges();
                    foreach (var item in child_Payroll)
                    {
                        item.PayRoll_Parent_id = parent_Payroll.PayRoll_Parent_id;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("PayrollList");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult DeletePayroll(int ?id)
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                var Employee = db.Employee.Where(m => m.user.email == Email).ToList();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else if (Employee.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter Emplyee Then proceed to Calclulate Payroll";
                    return RedirectToAction("EmployeeEdit");
                }
                else
                {
                    var parent_Payroll = db.PayRoll_Parent.Where(m => m.PayRoll_Parent_id == id && m.users.email == Email).SingleOrDefault();
                    if (parent_Payroll != null)
                    {
                        var child_Payroll = db.PayRoll.Where(m => m.PayRoll_Parent_id == id).ToList();
                        foreach (var item in child_Payroll)
                        {
                            item.PayRoll_Parent_id = parent_Payroll.PayRoll_Parent_id;
                            db.Entry(item).State = EntityState.Deleted;
                            db.SaveChanges();
                        }

                        db.Entry(parent_Payroll).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("PayrollList");
                }
            }
            return RedirectToAction("Login");
        }
        public ActionResult PayrollList()
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                var branch = db.Branch.Where(m => m.user.email == Email).ToList();
                var company = db.Company.Where(m => m.user.email == Email).SingleOrDefault();
                var Employee = db.Employee.Where(m => m.user.email == Email).ToList();
                var Payroll = db.PayRoll_Parent.Where(m => m.users.email == Email).ToList();
                if (company == null)
                {
                    //If Company not exist Add it
                    TempData["Alert"] = "Enter your company info then View Employees";
                    return RedirectToAction("CompanyEdit");
                }
                else if (branch.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter your branch info then View Employees";
                    return RedirectToAction("BranchEdit");
                }
                else if (Employee.Count == 0)
                {
                    //If Branch not exist Add it
                    TempData["Alert"] = "Enter Emplyee Then proceed to Calclulate Payroll";
                    return RedirectToAction("EmployeeEdit");
                }
                else if(Payroll.Count==0)
                {
                    return RedirectToAction("CalculatePayroll");
                }
                else
                {
                    return View(Payroll);
                }
            }
            return RedirectToAction("Login");
        }
    }
}