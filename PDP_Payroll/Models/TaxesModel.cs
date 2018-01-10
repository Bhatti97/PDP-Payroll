using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class TaxesModel
    {
        [Key]
        public int tax_id { get; set; }
        public string tax_title { get; set; }
        public float tax_percentage { get; set; }
        public int state_id { get; set; }
        public virtual StateModel state { get; set; }
    }
}