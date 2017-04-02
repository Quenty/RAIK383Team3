namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", "dbo.OrderRequestInfoes");
            DropForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes");
            DropIndex("dbo.Orders", new[] { "RequestInfo_OrderRequestInfoGuid" });
            DropIndex("dbo.Orders", new[] { "TrackInfo_OrderTrackInfoGuid" });
            AlterColumn("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "RequestInfo_OrderRequestInfoGuid");
            CreateIndex("dbo.Orders", "TrackInfo_OrderTrackInfoGuid");
            AddForeignKey("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", "dbo.OrderRequestInfoes", "OrderRequestInfoGuid", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes", "OrderTrackInfoGuid", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes");
            DropForeignKey("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", "dbo.OrderRequestInfoes");
            DropIndex("dbo.Orders", new[] { "TrackInfo_OrderTrackInfoGuid" });
            DropIndex("dbo.Orders", new[] { "RequestInfo_OrderRequestInfoGuid" });
            AlterColumn("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", c => c.Guid());
            AlterColumn("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", c => c.Guid());
            CreateIndex("dbo.Orders", "TrackInfo_OrderTrackInfoGuid");
            CreateIndex("dbo.Orders", "RequestInfo_OrderRequestInfoGuid");
            AddForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes", "OrderTrackInfoGuid");
            AddForeignKey("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", "dbo.OrderRequestInfoes", "OrderRequestInfoGuid");
        }
    }
}
