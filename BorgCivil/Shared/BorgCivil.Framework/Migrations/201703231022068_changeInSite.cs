namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sites", "Note", c => c.String(maxLength: 4000));
            AddColumn("dbo.BookingSiteGates", "FleetRegistrationId", c => c.Guid(nullable: false));
            CreateIndex("dbo.BookingSiteGates", "FleetRegistrationId");
            AddForeignKey("dbo.BookingSiteGates", "FleetRegistrationId", "dbo.FleetsRegistration", "FleetRegistrationId", cascadeDelete: true);
            DropColumn("dbo.BookingSiteGates", "Note");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingSiteGates", "Note", c => c.String(maxLength: 4000));
            DropForeignKey("dbo.BookingSiteGates", "FleetRegistrationId", "dbo.FleetsRegistration");
            DropIndex("dbo.BookingSiteGates", new[] { "FleetRegistrationId" });
            DropColumn("dbo.BookingSiteGates", "FleetRegistrationId");
            DropColumn("dbo.Sites", "Note");
        }
    }
}
