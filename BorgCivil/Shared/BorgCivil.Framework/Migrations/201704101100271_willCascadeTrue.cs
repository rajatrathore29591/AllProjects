namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class willCascadeTrue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FleetHistory", "BookingFleetId", "dbo.BookingFleets");
            AddForeignKey("dbo.FleetHistory", "BookingFleetId", "dbo.BookingFleets", "BookingFleetId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FleetHistory", "BookingFleetId", "dbo.BookingFleets");
            AddForeignKey("dbo.FleetHistory", "BookingFleetId", "dbo.BookingFleets", "BookingFleetId");
        }
    }
}
