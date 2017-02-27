namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid", "dbo.EmployeeInfoes");
            RenameColumn(table: "dbo.AspNetUsers", name: "EmployeeInfo_CustomerInfoGuid", newName: "EmployeeInfo_EmployeeInfoGuid");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_EmployeeInfo_CustomerInfoGuid", newName: "IX_EmployeeInfo_EmployeeInfoGuid");
            DropPrimaryKey("dbo.EmployeeInfoes");
            AddColumn("dbo.EmployeeInfoes", "EmployeeInfoGuid", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.EmployeeInfoes", "EmployeeInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "EmployeeInfo_EmployeeInfoGuid", "dbo.EmployeeInfoes", "EmployeeInfoGuid");
            DropColumn("dbo.EmployeeInfoes", "CustomerInfoGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmployeeInfoes", "CustomerInfoGuid", c => c.Guid(nullable: false));
            DropForeignKey("dbo.AspNetUsers", "EmployeeInfo_EmployeeInfoGuid", "dbo.EmployeeInfoes");
            DropPrimaryKey("dbo.EmployeeInfoes");
            DropColumn("dbo.EmployeeInfoes", "EmployeeInfoGuid");
            AddPrimaryKey("dbo.EmployeeInfoes", "CustomerInfoGuid");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_EmployeeInfo_EmployeeInfoGuid", newName: "IX_EmployeeInfo_CustomerInfoGuid");
            RenameColumn(table: "dbo.AspNetUsers", name: "EmployeeInfo_EmployeeInfoGuid", newName: "EmployeeInfo_CustomerInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid", "dbo.EmployeeInfoes", "CustomerInfoGuid");
        }
    }
}
