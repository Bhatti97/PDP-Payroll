using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PDP_Payroll.Models
{
    public class PDP_DbContext:DbContext
    {
        public PDP_DbContext():base("Default")
        {

        }
        public DbSet<BranchModel> Branch { get; set; }
        public DbSet<CheckOutModel> CheckOut { get; set; }
        public DbSet<CompanyModel> Company { get; set; }
        public DbSet<EmployeeModel> Employee { get; set; }
        public DbSet<MessagesModel> Messages { get; set; }
        public DbSet<NotificationModel> Notification { get; set; }
        public DbSet<PayrollModel> PayRoll { get; set; }
        public DbSet<PayRoll_ParentModel> PayRoll_Parent { get; set; }
        public DbSet<SliderModel> Slider { get; set; }
        public DbSet<StateModel> State { get; set; }
        public DbSet<TaxesModel> Tax { get; set; }
        public DbSet<UsersModel> User { get; set; }


    }
}