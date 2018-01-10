using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class UsersModel
    {
        [Key]  
        public int user_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
    }
}