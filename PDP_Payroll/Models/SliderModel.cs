using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class SliderModel
    {
        [Key]
        public int slider_id { get; set; }
        public string slider_image { get; set; }
        public string slider_title { get; set; }
        public string slider_subTitle { get; set; }
        public string object_image { get; set; }
        public bool IsObject { get; set; }
    }
}