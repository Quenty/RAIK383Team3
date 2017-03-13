namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addlicensetotruck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trucks", "LicenseNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trucks", "LicenseNumber");
        }
    }
}
