namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FleetHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FleetHistory",
                c => new
                    {
                        FleetHistoryId = c.Guid(nullable: false),
                        BookingFleetId = c.Guid(),
                        BookingId = c.Guid(),
                        FleetTypeId = c.Guid(),
                        FleetRegistrationId = c.Guid(),
                        DriverId = c.Guid(),
                        FleetStatus = c.Guid(),
                        IsDayShift = c.Boolean(nullable: false),
                        Iswethire = c.Boolean(nullable: false),
                        AttachmentIds = c.String(),
                        NotesForDrive = c.String(),
                        Reason = c.String(),
                        IsfleetCustomerSite = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FleetHistoryId)
                .ForeignKey("dbo.Booking", t => t.BookingId)
                .ForeignKey("dbo.BookingFleets", t => t.BookingFleetId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.FleetsRegistration", t => t.FleetRegistrationId)
                .ForeignKey("dbo.FleetTypes", t => t.FleetTypeId)
                .ForeignKey("dbo.StatusLookup", t => t.FleetStatus)
                .Index(t => t.BookingFleetId)
                .Index(t => t.BookingId)
                .Index(t => t.FleetTypeId)
                .Index(t => t.FleetRegistrationId)
                .Index(t => t.DriverId)
                .Index(t => t.FleetStatus);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FleetHistory", "FleetStatus", "dbo.StatusLookup");
            DropForeignKey("dbo.FleetHistory", "FleetTypeId", "dbo.FleetTypes");
            DropForeignKey("dbo.FleetHistory", "FleetRegistrationId", "dbo.FleetsRegistration");
            DropForeignKey("dbo.FleetHistory", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.FleetHistory", "BookingFleetId", "dbo.BookingFleets");
            DropForeignKey("dbo.FleetHistory", "BookingId", "dbo.Booking");
            DropIndex("dbo.FleetHistory", new[] { "FleetStatus" });
            DropIndex("dbo.FleetHistory", new[] { "DriverId" });
            DropIndex("dbo.FleetHistory", new[] { "FleetRegistrationId" });
            DropIndex("dbo.FleetHistory", new[] { "FleetTypeId" });
            DropIndex("dbo.FleetHistory", new[] { "BookingId" });
            DropIndex("dbo.FleetHistory", new[] { "BookingFleetId" });
            DropTable("dbo.FleetHistory");
        }
    }
}
