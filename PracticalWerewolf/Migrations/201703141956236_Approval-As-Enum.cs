namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovalAsEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContractorInfoes", "ApprovalState", c => c.Int(nullable: false));
            DropColumn("dbo.ContractorInfoes", "IsApproved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContractorInfoes", "IsApproved", c => c.Boolean(nullable: false));
            DropColumn("dbo.ContractorInfoes", "ApprovalState");
        }
    }
}
