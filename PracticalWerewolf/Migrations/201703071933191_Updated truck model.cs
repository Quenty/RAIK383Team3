namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatedtruckmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckCapacityUnits", "Mass", c => c.Double(nullable: false));
            AddColumn("dbo.TruckCapacityUnits", "Volume", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TruckCapacityUnits", "Volume");
            DropColumn("dbo.TruckCapacityUnits", "Mass");
        }
    }
}
