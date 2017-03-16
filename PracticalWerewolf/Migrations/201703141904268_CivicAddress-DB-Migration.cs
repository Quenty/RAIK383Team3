namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CivicAddressDBMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CivicAddressDbs", "Route", c => c.String(nullable: false));
            AddColumn("dbo.CivicAddressDbs", "State", c => c.String(nullable: false));
            AddColumn("dbo.CivicAddressDbs", "ZipCode", c => c.String(nullable: false, maxLength: 14));
            AddColumn("dbo.CivicAddressDbs", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.CivicAddressDbs", "City", c => c.String(nullable: false));
            DropColumn("dbo.CivicAddressDbs", "AddressLine1");
            DropColumn("dbo.CivicAddressDbs", "AddressLine2");
            DropColumn("dbo.CivicAddressDbs", "Building");
            DropColumn("dbo.CivicAddressDbs", "CountryRegion");
            DropColumn("dbo.CivicAddressDbs", "FloorLevel");
            DropColumn("dbo.CivicAddressDbs", "PostalCode");
            DropColumn("dbo.CivicAddressDbs", "StateProvince");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CivicAddressDbs", "StateProvince", c => c.String());
            AddColumn("dbo.CivicAddressDbs", "PostalCode", c => c.String());
            AddColumn("dbo.CivicAddressDbs", "FloorLevel", c => c.String());
            AddColumn("dbo.CivicAddressDbs", "CountryRegion", c => c.String());
            AddColumn("dbo.CivicAddressDbs", "Building", c => c.String());
            AddColumn("dbo.CivicAddressDbs", "AddressLine2", c => c.String());
            AddColumn("dbo.CivicAddressDbs", "AddressLine1", c => c.String());
            AlterColumn("dbo.CivicAddressDbs", "City", c => c.String());
            DropColumn("dbo.CivicAddressDbs", "Country");
            DropColumn("dbo.CivicAddressDbs", "ZipCode");
            DropColumn("dbo.CivicAddressDbs", "State");
            DropColumn("dbo.CivicAddressDbs", "Route");
        }
    }
}
