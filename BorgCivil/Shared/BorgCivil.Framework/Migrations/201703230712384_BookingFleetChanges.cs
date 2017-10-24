namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingFleetChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingFleets", "IsfleetCustomerSite", c => c.Boolean(nullable: false));
            AddColumn("dbo.BookingFleets", "StatusLookupId", c => c.Guid(nullable: true));

        }
        
        public override void Down()
        {
            //AddColumn("dbo.BookingFleets", "IsActIsfleetCustomerSiteive", c => c.Boolean(nullable: false));
            
        }
    }
}
