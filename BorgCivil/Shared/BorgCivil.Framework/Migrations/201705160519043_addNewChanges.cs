namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Attachments", newName: "Attachment");
            RenameTable(name: "dbo.Companies", newName: "Company");
            RenameTable(name: "dbo.BookingFleets", newName: "BookingFleet");
            RenameTable(name: "dbo.Drivers", newName: "Driver");
            RenameTable(name: "dbo.BookingSiteGates", newName: "BookingSiteGate");
            RenameTable(name: "dbo.Sites", newName: "Site");
            RenameTable(name: "dbo.FleetTypes", newName: "FleetType");
            RenameTable(name: "dbo.WorkTypes", newName: "WorkType");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.WorkType", newName: "WorkTypes");
            RenameTable(name: "dbo.FleetType", newName: "FleetTypes");
            RenameTable(name: "dbo.Site", newName: "Sites");
            RenameTable(name: "dbo.BookingSiteGate", newName: "BookingSiteGates");
            RenameTable(name: "dbo.Driver", newName: "Drivers");
            RenameTable(name: "dbo.BookingFleet", newName: "BookingFleets");
            RenameTable(name: "dbo.Company", newName: "Companies");
            RenameTable(name: "dbo.Attachment", newName: "Attachments");
        }
    }
}
