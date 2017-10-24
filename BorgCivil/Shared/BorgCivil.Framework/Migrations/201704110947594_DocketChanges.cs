namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocketChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Docket", "BookingId", "dbo.Booking");
            DropForeignKey("dbo.Docket", "SiteId", "dbo.Sites");
            DropIndex("dbo.Docket", new[] { "BookingId" });
            DropIndex("dbo.Docket", new[] { "SiteId" });
            AddColumn("dbo.Docket", "BookingFleetId", c => c.Guid());
            CreateIndex("dbo.Docket", "BookingFleetId");
            AddForeignKey("dbo.Docket", "BookingFleetId", "dbo.BookingFleets", "BookingFleetId");
            DropColumn("dbo.Docket", "BookingId");
            DropColumn("dbo.Docket", "SiteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Docket", "SiteId", c => c.Guid());
            AddColumn("dbo.Docket", "BookingId", c => c.Guid());
            DropForeignKey("dbo.Docket", "BookingFleetId", "dbo.BookingFleets");
            DropIndex("dbo.Docket", new[] { "BookingFleetId" });
            DropColumn("dbo.Docket", "BookingFleetId");
            CreateIndex("dbo.Docket", "SiteId");
            CreateIndex("dbo.Docket", "BookingId");
            AddForeignKey("dbo.Docket", "SiteId", "dbo.Sites", "SiteId");
            AddForeignKey("dbo.Docket", "BookingId", "dbo.Booking", "BookingId");
        }
    }
}
