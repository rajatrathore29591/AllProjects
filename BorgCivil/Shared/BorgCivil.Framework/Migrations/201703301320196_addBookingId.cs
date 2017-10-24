namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBookingId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingSiteSupervisor", "BookingId", c => c.Guid(nullable: false));
            CreateIndex("dbo.BookingSiteSupervisor", "BookingId");
            AddForeignKey("dbo.BookingSiteSupervisor", "BookingId", "dbo.Booking", "BookingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingSiteSupervisor", "BookingId", "dbo.Booking");
            DropIndex("dbo.BookingSiteSupervisor", new[] { "BookingId" });
            DropColumn("dbo.BookingSiteSupervisor", "BookingId");
        }
    }
}
