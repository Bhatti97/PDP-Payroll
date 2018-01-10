using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class NotificationModel
    {
        [Key]
        public int notification_id { get; set; }
        public string notification_type { get; set; }
        public int notification_no { get; set; }

    }
}