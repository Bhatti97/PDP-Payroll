using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class BranchModel
    {
        [Key]
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string branch_address { get; set; }
        public string branch_phone { get; set; }
        public string branch_email { get; set; }
        public int user_id { get; set; }
        public virtual UsersModel user{ get; set; }
        public int company_id { get; set; }
        public virtual CompanyModel Company { get; set; }
        public int state_id { get; set; }
        public virtual StateModel State { get; set; }
    }
}