namespace PracticalWerewolf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasicDataClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContractorInfoes",
                c => new
                    {
                        ContractorInfoGuid = c.Guid(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        Truck_TruckGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.ContractorInfoGuid)
                .ForeignKey("dbo.Trucks", t => t.Truck_TruckGuid)
                .Index(t => t.Truck_TruckGuid);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        TruckGuid = c.Guid(nullable: false),
                        Location_Latitude = c.Double(nullable: false),
                        Location_Longitude = c.Double(nullable: false),
                        Location_Altitude = c.Double(nullable: false),
                        Location_HorizontalAccuracy = c.Double(nullable: false),
                        Location_VerticalAccuracy = c.Double(nullable: false),
                        Location_Speed = c.Double(nullable: false),
                        Location_Course = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TruckGuid);
            
            CreateTable(
                "dbo.CustomerInfoes",
                c => new
                    {
                        CustomerInfoGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerInfoGuid);
            
            CreateTable(
                "dbo.EmployeeInfoes",
                c => new
                    {
                        CustomerInfoGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerInfoGuid);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderGuid = c.Guid(nullable: false),
                        RequestInfo_OrderRequestInfoGuid = c.Guid(),
                        TrackInfo_OrderTrackInfoGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .ForeignKey("dbo.OrderRequestInfoes", t => t.RequestInfo_OrderRequestInfoGuid)
                .ForeignKey("dbo.OrderTrackInfoes", t => t.TrackInfo_OrderTrackInfoGuid)
                .Index(t => t.RequestInfo_OrderRequestInfoGuid)
                .Index(t => t.TrackInfo_OrderTrackInfoGuid);
            
            CreateTable(
                "dbo.OrderRequestInfoes",
                c => new
                    {
                        OrderRequestInfoGuid = c.Guid(nullable: false),
                        Requester_CustomerInfoGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.OrderRequestInfoGuid)
                .ForeignKey("dbo.CustomerInfoes", t => t.Requester_CustomerInfoGuid)
                .Index(t => t.Requester_CustomerInfoGuid);
            
            CreateTable(
                "dbo.OrderTrackInfoes",
                c => new
                    {
                        OrderTrackInfoGuid = c.Guid(nullable: false),
                        Assignee_ContractorInfoGuid = c.Guid(),
                        CurrentTruck_TruckGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.OrderTrackInfoGuid)
                .ForeignKey("dbo.ContractorInfoes", t => t.Assignee_ContractorInfoGuid)
                .ForeignKey("dbo.Trucks", t => t.CurrentTruck_TruckGuid)
                .Index(t => t.Assignee_ContractorInfoGuid)
                .Index(t => t.CurrentTruck_TruckGuid);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserInfoGuid = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UserInfoGuid);
            
            AddColumn("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "CustomerInfo_CustomerInfoGuid", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "UserInfo_UserInfoGuid", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid");
            CreateIndex("dbo.AspNetUsers", "CustomerInfo_CustomerInfoGuid");
            CreateIndex("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid");
            CreateIndex("dbo.AspNetUsers", "UserInfo_UserInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", "dbo.ContractorInfoes", "ContractorInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "CustomerInfo_CustomerInfoGuid", "dbo.CustomerInfoes", "CustomerInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid", "dbo.EmployeeInfoes", "CustomerInfoGuid");
            AddForeignKey("dbo.AspNetUsers", "UserInfo_UserInfoGuid", "dbo.UserInfoes", "UserInfoGuid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserInfo_UserInfoGuid", "dbo.UserInfoes");
            DropForeignKey("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid", "dbo.EmployeeInfoes");
            DropForeignKey("dbo.AspNetUsers", "CustomerInfo_CustomerInfoGuid", "dbo.CustomerInfoes");
            DropForeignKey("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid", "dbo.ContractorInfoes");
            DropForeignKey("dbo.Orders", "TrackInfo_OrderTrackInfoGuid", "dbo.OrderTrackInfoes");
            DropForeignKey("dbo.OrderTrackInfoes", "CurrentTruck_TruckGuid", "dbo.Trucks");
            DropForeignKey("dbo.OrderTrackInfoes", "Assignee_ContractorInfoGuid", "dbo.ContractorInfoes");
            DropForeignKey("dbo.Orders", "RequestInfo_OrderRequestInfoGuid", "dbo.OrderRequestInfoes");
            DropForeignKey("dbo.OrderRequestInfoes", "Requester_CustomerInfoGuid", "dbo.CustomerInfoes");
            DropForeignKey("dbo.ContractorInfoes", "Truck_TruckGuid", "dbo.Trucks");
            DropIndex("dbo.AspNetUsers", new[] { "UserInfo_UserInfoGuid" });
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeInfo_CustomerInfoGuid" });
            DropIndex("dbo.AspNetUsers", new[] { "CustomerInfo_CustomerInfoGuid" });
            DropIndex("dbo.AspNetUsers", new[] { "ContractorInfo_ContractorInfoGuid" });
            DropIndex("dbo.OrderTrackInfoes", new[] { "CurrentTruck_TruckGuid" });
            DropIndex("dbo.OrderTrackInfoes", new[] { "Assignee_ContractorInfoGuid" });
            DropIndex("dbo.OrderRequestInfoes", new[] { "Requester_CustomerInfoGuid" });
            DropIndex("dbo.Orders", new[] { "TrackInfo_OrderTrackInfoGuid" });
            DropIndex("dbo.Orders", new[] { "RequestInfo_OrderRequestInfoGuid" });
            DropIndex("dbo.ContractorInfoes", new[] { "Truck_TruckGuid" });
            DropColumn("dbo.AspNetUsers", "UserInfo_UserInfoGuid");
            DropColumn("dbo.AspNetUsers", "EmployeeInfo_CustomerInfoGuid");
            DropColumn("dbo.AspNetUsers", "CustomerInfo_CustomerInfoGuid");
            DropColumn("dbo.AspNetUsers", "ContractorInfo_ContractorInfoGuid");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.OrderTrackInfoes");
            DropTable("dbo.OrderRequestInfoes");
            DropTable("dbo.Orders");
            DropTable("dbo.EmployeeInfoes");
            DropTable("dbo.CustomerInfoes");
            DropTable("dbo.Trucks");
            DropTable("dbo.ContractorInfoes");
        }
    }
}
