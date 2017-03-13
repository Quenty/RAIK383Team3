namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCivicAddressGuids : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CivicAddressDbs",
                c => new
                    {
                        CivicAddressGuid = c.Guid(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        Building = c.String(),
                        City = c.String(),
                        CountryRegion = c.String(),
                        FloorLevel = c.String(),
                        PostalCode = c.String(),
                        StateProvince = c.String(),
                    })
                .PrimaryKey(t => t.CivicAddressGuid);
            
            AddColumn("dbo.OrderRequestInfoes", "RequestDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderRequestInfoes", "DropOffAddress_CivicAddressGuid", c => c.Guid(nullable: false));
            AddColumn("dbo.OrderRequestInfoes", "PickUpAddress_CivicAddressGuid", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderRequestInfoes", "DropOffAddress_CivicAddressGuid");
            CreateIndex("dbo.OrderRequestInfoes", "PickUpAddress_CivicAddressGuid");
            AddForeignKey("dbo.OrderRequestInfoes", "DropOffAddress_CivicAddressGuid", "dbo.CivicAddressDbs", "CivicAddressGuid", cascadeDelete: false);
            AddForeignKey("dbo.OrderRequestInfoes", "PickUpAddress_CivicAddressGuid", "dbo.CivicAddressDbs", "CivicAddressGuid", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderRequestInfoes", "PickUpAddress_CivicAddressGuid", "dbo.CivicAddressDbs");
            DropForeignKey("dbo.OrderRequestInfoes", "DropOffAddress_CivicAddressGuid", "dbo.CivicAddressDbs");
            DropIndex("dbo.OrderRequestInfoes", new[] { "PickUpAddress_CivicAddressGuid" });
            DropIndex("dbo.OrderRequestInfoes", new[] { "DropOffAddress_CivicAddressGuid" });
            DropColumn("dbo.OrderRequestInfoes", "PickUpAddress_CivicAddressGuid");
            DropColumn("dbo.OrderRequestInfoes", "DropOffAddress_CivicAddressGuid");
            DropColumn("dbo.OrderRequestInfoes", "RequestDate");
            DropTable("dbo.CivicAddressDbs");
        }
    }
}
