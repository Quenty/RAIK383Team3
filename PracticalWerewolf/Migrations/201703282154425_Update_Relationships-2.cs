namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Relationships2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", "dbo.CustomerInfoes");
            DropForeignKey("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropIndex("dbo.OrderRequestInfoes", new[] { "Requester_CustomerInfoGuid" });
            DropIndex("dbo.OrderRequestInfoes", new[] { "Size_TruckCapacityUnitGuid" });
            AlterColumn("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", c => c.Guid(nullable: false));
            AlterColumn("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid");
            CreateIndex("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid");
            AddForeignKey("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", "dbo.CustomerInfoes", "CustomerInfoGuid", cascadeDelete: true);
            AddForeignKey("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropForeignKey("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", "dbo.CustomerInfoes");
            DropIndex("dbo.OrderRequestInfoes", new[] { "Size_TruckCapacityUnitGuid" });
            DropIndex("dbo.OrderRequestInfoes", new[] { "Requester_CustomerInfoGuid" });
            AlterColumn("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", c => c.Guid());
            AlterColumn("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", c => c.Guid());
            CreateIndex("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid");
            CreateIndex("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid");
            AddForeignKey("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid");
            AddForeignKey("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", "dbo.CustomerInfoes", "CustomerInfoGuid");
        }
    }
}
