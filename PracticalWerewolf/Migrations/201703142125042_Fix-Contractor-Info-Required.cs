namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixContractorInfoRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", "dbo.CivicAddressDbs");
            DropIndex("dbo.ContractorInfoes", new[] { "HomeAddress_CivicAddressGuid" });
            AlterColumn("dbo.ContractorInfoes", "DriversLicenseId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", c => c.Guid(nullable: false));
            CreateIndex("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid");
            AddForeignKey("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", "dbo.CivicAddressDbs", "CivicAddressGuid", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", "dbo.CivicAddressDbs");
            DropIndex("dbo.ContractorInfoes", new[] { "HomeAddress_CivicAddressGuid" });
            AlterColumn("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", c => c.Guid());
            AlterColumn("dbo.ContractorInfoes", "DriversLicenseId", c => c.String(maxLength: 20));
            CreateIndex("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid");
            AddForeignKey("dbo.ContractorInfoes", "HomeAddress_CivicAddressGuid", "dbo.CivicAddressDbs", "CivicAddressGuid");
        }
    }
}
