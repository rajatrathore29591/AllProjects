namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldInFleet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingFleets", "FleetBookingDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.BookingFleets", "FleetBookingEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookingFleets", "FleetBookingEndDate");
            DropColumn("dbo.BookingFleets", "FleetBookingDateTime");
        }
    }
}
