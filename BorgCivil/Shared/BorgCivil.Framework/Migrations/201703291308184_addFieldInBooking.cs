namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldInBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "CancelNote", c => c.String());
            AddColumn("dbo.Booking", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BookingFleets", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingFleets", "Reason");
            DropColumn("dbo.Booking", "Rate");
            DropColumn("dbo.Booking", "CancelNote");
        }
    }
}
