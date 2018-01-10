using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class StateModel
    {
        [Key]
        public int state_id { get; set; }
        public string state_name { get; set; }
    }
}