namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTruckCapacityUnitRequirements : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropIndex("dbo.Trucks", new[] { "MaxCapacity_TruckCapacityUnitGuid" });
            RenameColumn(table: "dbo.Trucks", name: "CurrentCapacity_TruckCapacityUnitGuid", newName: "AvailableCapacity_TruckCapacityUnitGuid");
            RenameIndex(table: "dbo.Trucks", name: "IX_CurrentCapacity_TruckCapacityUnitGuid", newName: "IX_AvailableCapacity_TruckCapacityUnitGuid");
            AlterColumn("dbo.Trucks", "LicenseNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", c => c.Guid(nullable: false));
            CreateIndex("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid");
            AddForeignKey("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits");
            DropIndex("dbo.Trucks", new[] { "MaxCapacity_TruckCapacityUnitGuid" });
            AlterColumn("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", c => c.Guid());
            AlterColumn("dbo.Trucks", "LicenseNumber", c => c.String());
            RenameIndex(table: "dbo.Trucks", name: "IX_AvailableCapacity_TruckCapacityUnitGuid", newName: "IX_CurrentCapacity_TruckCapacityUnitGuid");
            RenameColumn(table: "dbo.Trucks", name: "AvailableCapacity_TruckCapacityUnitGuid", newName: "CurrentCapacity_TruckCapacityUnitGuid");
            CreateIndex("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid");
            AddForeignKey("dbo.Trucks", "MaxCapacity_TruckCapacityUnitGuid", "dbo.TruckCapacityUnits", "TruckCapacityUnitGuid");
        }
    }
}
