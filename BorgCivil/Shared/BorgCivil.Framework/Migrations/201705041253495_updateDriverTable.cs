namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDriverTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "StatusLookupId", c => c.Guid());
            CreateIndex("dbo.Drivers", "StatusLookupId");
            AddForeignKey("dbo.Drivers", "StatusLookupId", "dbo.StatusLookup", "StatusLookupId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "StatusLookupId", "dbo.StatusLookup");
            DropIndex("dbo.Drivers", new[] { "StatusLookupId" });
            DropColumn("dbo.Drivers", "StatusLookupId");
        }
    }
}
