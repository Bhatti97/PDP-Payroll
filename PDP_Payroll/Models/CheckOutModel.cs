using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class CheckOutModel
    {
        [Key]
        public int CheckOut_id { get; set; }
        public int Payroll_id { get; set; }
        public virtual PayrollModel payroll { get; set; }
        public bool isSeen { get; set; }
    }

}