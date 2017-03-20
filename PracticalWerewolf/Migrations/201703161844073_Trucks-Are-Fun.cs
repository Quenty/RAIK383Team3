namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class TrucksAreFun : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trucks", "LicenseNumber", c => c.String());
            AddColumn("dbo.Trucks", "Location", c => c.Geography());
            AddColumn("dbo.TruckCapacityUnits", "Mass", c => c.Double(nullable: false));
            AddColumn("dbo.TruckCapacityUnits", "Volume", c => c.Double(nullable: false));
            DropColumn("dbo.Trucks", "Location_Latitude");
            DropColumn("dbo.Trucks", "Location_Longitude");
            DropColumn("dbo.Trucks", "Location_Altitude");
            DropColumn("dbo.Trucks", "Location_HorizontalAccuracy");
            DropColumn("dbo.Trucks", "Location_VerticalAccuracy");
            DropColumn("dbo.Trucks", "Location_Speed");
            DropColumn("dbo.Trucks", "Location_Course");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trucks", "Location_Course", c => c.Double(nullable: false));
            AddColumn("dbo.Trucks", "Location_Speed", c => c.Double(nullable: false));
            AddColumn("dbo.Trucks", "Location_VerticalAccuracy", c => c.Double(nullable: false));
            AddColumn("dbo.Trucks", "Location_HorizontalAccuracy", c => c.Double(nullable: false));
            AddColumn("dbo.Trucks", "Location_Altitude", c => c.Double(nullable: false));
            AddColumn("dbo.Trucks", "Location_Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Trucks", "Location_Latitude", c => c.Double(nullable: false));
            DropColumn("dbo.TruckCapacityUnits", "Volume");
            DropColumn("dbo.TruckCapacityUnits", "Mass");
            DropColumn("dbo.Trucks", "Location");
            DropColumn("dbo.Trucks", "LicenseNumber");
        }
    }
}
