namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFleetR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "TypeFromDate", c => c.DateTime());
            AddColumn("dbo.Drivers", "TypeToDate", c => c.DateTime());
            AddColumn("dbo.Drivers", "TypeNote", c => c.String());
            AddColumn("dbo.Drivers", "LeaveFromDate", c => c.DateTime());
            AddColumn("dbo.Drivers", "LeaveToDate", c => c.DateTime());
            AddColumn("dbo.Drivers", "LeaveNote", c => c.String());
            AddColumn("dbo.FleetsRegistration", "UnavailableFromDate", c => c.DateTime());
            AddColumn("dbo.FleetsRegistration", "UnavailableToDate", c => c.DateTime());
            AddColumn("dbo.FleetsRegistration", "UnavailableNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FleetsRegistration", "UnavailableNote");
            DropColumn("dbo.FleetsRegistration", "UnavailableToDate");
            DropColumn("dbo.FleetsRegistration", "UnavailableFromDate");
            DropColumn("dbo.Drivers", "LeaveNote");
            DropColumn("dbo.Drivers", "LeaveToDate");
            DropColumn("dbo.Drivers", "LeaveFromDate");
            DropColumn("dbo.Drivers", "TypeNote");
            DropColumn("dbo.Drivers", "TypeToDate");
            DropColumn("dbo.Drivers", "TypeFromDate");
        }
    }
}
