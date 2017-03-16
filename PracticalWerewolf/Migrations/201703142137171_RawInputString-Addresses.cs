namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RawInputStringAddresses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CivicAddressDbs", "RawInputAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CivicAddressDbs", "RawInputAddress");
        }
    }
}
