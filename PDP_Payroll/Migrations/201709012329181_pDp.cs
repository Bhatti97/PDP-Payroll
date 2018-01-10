namespace PDP_Payroll.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pDp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PayRoll_ParentModel", "user_id", c => c.Int(nullable: false));
            CreateIndex("dbo.PayRoll_ParentModel", "user_id");
            AddForeignKey("dbo.PayRoll_ParentModel", "user_id", "dbo.UsersModels", "user_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayRoll_ParentModel", "user_id", "dbo.UsersModels");
            DropIndex("dbo.PayRoll_ParentModel", new[] { "user_id" });
            DropColumn("dbo.PayRoll_ParentModel", "user_id");
        }
    }
}
