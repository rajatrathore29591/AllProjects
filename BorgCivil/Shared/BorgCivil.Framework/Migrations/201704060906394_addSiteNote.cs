namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSiteNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "SiteNote", c => c.String());
            AddColumn("dbo.FleetTypes", "DocumentId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FleetTypes", "DocumentId");
            DropColumn("dbo.Booking", "SiteNote");
        }
    }
}
