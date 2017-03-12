namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreGuids : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderTrackInfoes", "Assignee_ContractorInfoGuid", "dbo.ContractorInfoes");
            DropForeignKey("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", "dbo.ContractorInfoes");
            DropPrimaryKey("dbo.ContractorInfoes");
            AlterColumn("dbo.ContractorInfoes", "ContractorInfoGuid", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.ContractorInfoes", "ContractorInfoGuid");
            AddForeignKey("dbo.OrderTrackInfoes", "Assignee_ContractorInfoGuid", "dbo.ContractorInfoes", "ContractorInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", "dbo.ContractorInfoes", "ContractorInfoGuid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", "dbo.ContractorInfoes");
            DropForeignKey("dbo.OrderTrackInfoes", "Assignee_ContractorInfoGuid", "dbo.ContractorInfoes");
            DropPrimaryKey("dbo.ContractorInfoes");
            AlterColumn("dbo.ContractorInfoes", "ContractorInfoGuid", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.ContractorInfoes", "ContractorInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", "dbo.ContractorInfoes", "ContractorInfoGuid");
            AddForeignKey("dbo.OrderTrackInfoes", "Assignee_ContractorInfoGuid", "dbo.ContractorInfoes", "ContractorInfoGuid");
        }
    }
}
