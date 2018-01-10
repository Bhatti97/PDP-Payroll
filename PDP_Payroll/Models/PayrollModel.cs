using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class PayrollModel
    {
        [Key]
        public int payroll_id { get; set; }
        public string branch_name { get; set; }
        public string employee_name { get; set; }
        public string state_name { get; set; }
        public float employee_Wages { get; set; }
        public int total_hours { get; set; }
        public int worked_hours { get; set; }
        public float total_hours_amount { get; set; }
        public int over_time_hours { get; set; }
        public float over_time_amount { get; set; }
        public float TotalAmount { get; set; }
        public int state_tax { get; set; }
        public int city_Tax { get; set; }
        public int medicad { get; set; }
        public int SSTax { get; set; }
        public int Deduction { get; set; }
        public int NetPay { get; set; }
        public int PayRoll_Parent_id { get; set; }
        public virtual PayRoll_ParentModel  payroll_parent{ get; set; }
        
    }
}