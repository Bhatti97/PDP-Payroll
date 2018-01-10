using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class MessagesModel
    {
        [Key]
        public int messages_id { get; set; }
        public string sender_Name { get; set; }
        public string sender_Email { get; set; }
        public string sender_phoneNo { get; set; }
        public string sender_messages { get; set; }
        public bool isSeen { get; set; }
    }
}