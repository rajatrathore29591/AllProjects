namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class docket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Docket",
                c => new
                    {
                        DocketId = c.Guid(nullable: false),
                        BookingId = c.Guid(),
                        SiteId = c.Guid(),
                        FleetRegistrationId = c.Guid(),
                        DocumentId = c.Guid(),
                        DocketNo = c.String(nullable: false, maxLength: 30),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        StartKMs = c.Int(),
                        FinishKMsA = c.Int(),
                        LunchBreak1From = c.Time(nullable: false, precision: 7),
                        LunchBreak1End = c.Time(nullable: false, precision: 7),
                        LunchBreak2From = c.Time(nullable: false, precision: 7),
                        LunchBreak2End = c.Time(nullable: false, precision: 7),
                        AttachmentIds = c.String(),
                        DocketCheckListId = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocketId)
                .ForeignKey("dbo.Booking", t => t.BookingId)
                .ForeignKey("dbo.FleetsRegistration", t => t.FleetRegistrationId)
                .ForeignKey("dbo.Sites", t => t.SiteId)
                .Index(t => t.BookingId)
                .Index(t => t.SiteId)
                .Index(t => t.FleetRegistrationId);
            
            CreateTable(
                "dbo.LoadDocket",
                c => new
                    {
                        DocketLoadtId = c.Guid(nullable: false),
                        DocketId = c.Guid(),
                        LoadingSite = c.String(),
                        Weight = c.Int(nullable: false),
                        LoadTime = c.Time(nullable: false, precision: 7),
                        TipOffSite = c.String(),
                        TipOffTime = c.Time(nullable: false, precision: 7),
                        Material = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocketLoadtId)
                .ForeignKey("dbo.Docket", t => t.DocketId)
                .Index(t => t.DocketId);
            
            CreateTable(
                "dbo.DocketCheckList",
                c => new
                    {
                        DocketCheckListId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocketCheckListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Docket", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.LoadDocket", "DocketId", "dbo.Docket");
            DropForeignKey("dbo.Docket", "FleetRegistrationId", "dbo.FleetsRegistration");
            DropForeignKey("dbo.Docket", "BookingId", "dbo.Booking");
            DropIndex("dbo.LoadDocket", new[] { "DocketId" });
            DropIndex("dbo.Docket", new[] { "FleetRegistrationId" });
            DropIndex("dbo.Docket", new[] { "SiteId" });
            DropIndex("dbo.Docket", new[] { "BookingId" });
            DropTable("dbo.DocketCheckList");
            DropTable("dbo.LoadDocket");
            DropTable("dbo.Docket");
        }
    }
}
