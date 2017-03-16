namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CivicAddressDBMigrationUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CivicAddressDbs", "StreetNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CivicAddressDbs", "StreetNumber");
        }
    }
}
