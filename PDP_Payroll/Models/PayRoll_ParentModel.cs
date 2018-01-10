using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace PDP_Payroll.Models
{
    public class PayRoll_ParentModel
    {
        [Key]
        public int PayRoll_Parent_id { get; set; }
        public string Payroll_Title { get; set; }
        public DateTime Date { get; set; }
        public int user_id { get; set; }
        public virtual UsersModel users { get; set; }
    }

}