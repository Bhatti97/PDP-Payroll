using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class EmployeeModel
    {
        [Key]
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_Fathername { get; set; }
        public string employee_email { get; set; }
        public string employee_phoneNo { get; set; }
        public string employee_Address { get; set; }
        public string employee_NIC { get; set; }
        public string employee_MartialStatus { get; set; }
        public int employee_NoOfDependency { get; set; }
        public int employee_Wages { get; set; }
        public string Payroll_Type  { get; set; }
        public DateTime Date_of_Join { get; set; }
        public int state_id { get; set; }
        public virtual StateModel state { get; set; }
        public int branch_id { get; set; }
        public virtual BranchModel branch { get; set; }
        public int user_id { get; set; }
        public virtual UsersModel user{ get; set; }
    }
}