namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDataModelsOrderStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContractorInfoes", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderTrackInfoes", "OrderStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderTrackInfoes", "OrderStatus");
            DropColumn("dbo.ContractorInfoes", "IsApproved");
        }
    }
}
