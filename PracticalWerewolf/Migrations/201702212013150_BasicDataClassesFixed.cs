namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasicDataClassesFixed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TruckCapacityUnits",
                c => new
                    {
                        TruckCapacityUnitGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TruckCapacityUnitGuid);
            
            AddColumn("dbo.Trucks", "CurrentCapacity_TruckCapacityUnitGuid", c => c.Guid());
            AddColumn("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", c => c.Guid());
            AddColumn("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", c => c.Guid());
            CreateIndex("dbo.Trucks", "CurrentCapacity_TruckCapacityUnitGuid");
            CreateIndex("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid");
            CreateIndex("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid");
            AddForeignKey("dbo.Trucks", "CurrentCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid");
            AddForeignKey("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid");
            AddForeignKey("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropForeignKey("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropForeignKey("dbo.Trucks", "CurrentCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropIndex("dbo.OrderRequestInfoes", new[] { "Size_TruckCapacityUnitGuid" });
            DropIndex("dbo.Trucks", new[] { "MaxCapacity_TruckCapacityUnitGuid" });
            DropIndex("dbo.Trucks", new[] { "CurrentCapacity_TruckCapacityUnitGuid" });
            DropColumn("dbo.OrderRequestInfoes", "Size_TruckCapacityUnitGuid");
            DropColumn("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid");
            DropColumn("dbo.Trucks", "CurrentCapacity_TruckCapacityUnitGuid");
            DropTable("dbo.TruckCapacityUnits");
        }
    }
}
