namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GetSetProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContractorInfoes", "DriversLicenseId", c => c.String(maxLength: 20));
            AddColumn("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", c => c.Guid());
            CreateIndex("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid");
            AddForeignKey("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", "dbo.CivicAddressDbs", "CivicAddressGuid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", "dbo.CivicAddressDbs");
            DropIndex("dbo.ContractorInfoes", new[] { "HomeAddress_CivicAddressGuid" });
            DropColumn("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid");
            DropColumn("dbo.ContractorInfoes", "DriversLicenseId");
        }
    }
}
