namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Relationships2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes");
            DropIndex("dbo.Orders", new[] { "TrackInfo_OrderTrackInfoGuid" });
            RenameColumn(table: "dbo.OrderTrackInfoes", name: "TrackInfo_OrderTrackInfoGuid", newName: "OrderTrackInfoGuid_Test");
            CreateIndex("dbo.OrderTrackInfoes", "OrderTrackInfoGuid_Test");
            AddForeignKey("dbo.OrderTrackInfoes", "OrderTrackInfoGuid_Test", "dbo.Orders", "OrderGuid");
            DropColumn("dbo.Orders", "TrackInfo_OrderTrackInfoGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", c => c.Guid(nullable: false));
            DropForeignKey("dbo.OrderTrackInfoes", "OrderTrackInfoGuid_Test", "dbo.Orders");
            DropIndex("dbo.OrderTrackInfoes", new[] { "OrderTrackInfoGuid_Test" });
            RenameColumn(table: "dbo.OrderTrackInfoes", name: "OrderTrackInfoGuid_Test", newName: "TrackInfo_OrderTrackInfoGuid");
            CreateIndex("dbo.Orders", "TrackInfo_OrderTrackInfoGuid");
            AddForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes", "OrderTrackInfoGuid", cascadeDelete: true);
        }
    }
}
