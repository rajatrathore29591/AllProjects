namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFleetRegistration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnonymousField", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverInductionCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverVocCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverWhiteCard", "DriverId", "dbo.Drivers");
            AddForeignKey("dbo.AnonymousField", "DriverId", "dbo.Drivers", "DriverId", cascadeDelete: true);
            AddForeignKey("dbo.DriverInductionCard", "DriverId", "dbo.Drivers", "DriverId", cascadeDelete: true);
            AddForeignKey("dbo.DriverVocCard", "DriverId", "dbo.Drivers", "DriverId", cascadeDelete: true);
            AddForeignKey("dbo.DriverWhiteCard", "DriverId", "dbo.Drivers", "DriverId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverWhiteCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverVocCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverInductionCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.AnonymousField", "DriverId", "dbo.Drivers");
            AddForeignKey("dbo.DriverWhiteCard", "DriverId", "dbo.Drivers", "DriverId");
            AddForeignKey("dbo.DriverVocCard", "DriverId", "dbo.Drivers", "DriverId");
            AddForeignKey("dbo.DriverInductionCard", "DriverId", "dbo.Drivers", "DriverId");
            AddForeignKey("dbo.AnonymousField", "DriverId", "dbo.Drivers", "DriverId");
        }
    }
}
