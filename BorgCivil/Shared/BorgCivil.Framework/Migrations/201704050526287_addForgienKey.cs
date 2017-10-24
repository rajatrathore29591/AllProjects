namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForgienKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingSiteGates", "BookingId", c => c.Guid(nullable: true));
            CreateIndex("dbo.BookingSiteGates", "BookingId");
            AddForeignKey("dbo.BookingSiteGates", "BookingId", "dbo.Booking", "BookingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingSiteGates", "BookingId", "dbo.Booking");
            DropIndex("dbo.BookingSiteGates", new[] { "BookingId" });
            DropColumn("dbo.BookingSiteGates", "BookingId");
        }
    }
}
