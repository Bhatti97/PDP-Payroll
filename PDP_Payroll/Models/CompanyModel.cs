using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class CompanyModel
    {
        [Key]
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
        public string company_phone { get; set; }
        public string company_Email { get; set; }
        public int user_id { get; set; }
        public virtual UsersModel user { get; set; }
        public int state_id { get; set; }
        public virtual StateModel state { get; set; }
    }
}