namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoutePlanning : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RouteStops",
                c => new
                    {
                        RouteStopGuid = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        TimeToNextStop = c.Time(nullable: false, precision: 7),
                        StopIndex = c.Long(nullable: false),
                        Order_OrderGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.RouteStopGuid)
                .ForeignKey("dbo.Orders", t => t.Order_OrderGuid, cascadeDelete: true)
                .Index(t => t.Order_OrderGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteStops", "Order_OrderGuid", "dbo.Orders");
            DropIndex("dbo.RouteStops", new[] { "Order_OrderGuid" });
            DropTable("dbo.RouteStops");
        }
    }
}
