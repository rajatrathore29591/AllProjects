namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCascade1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Booking", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Booking", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Booking", "StatusLookupId", "dbo.StatusLookup");
            DropForeignKey("dbo.Booking", "WorktypeId", "dbo.WorkTypes");
            AddForeignKey("dbo.Booking", "CustomerId", "dbo.Customer", "CustomerId");
            AddForeignKey("dbo.Booking", "SiteId", "dbo.Sites", "SiteId", cascadeDelete: true);
            AddForeignKey("dbo.Booking", "StatusLookupId", "dbo.StatusLookup", "StatusLookupId");
            AddForeignKey("dbo.Booking", "WorktypeId", "dbo.WorkTypes", "WorkTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Booking", "WorktypeId", "dbo.WorkTypes");
            DropForeignKey("dbo.Booking", "StatusLookupId", "dbo.StatusLookup");
            DropForeignKey("dbo.Booking", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Booking", "CustomerId", "dbo.Customer");
            AddForeignKey("dbo.Booking", "WorktypeId", "dbo.WorkTypes", "WorkTypeId", cascadeDelete: true);
            AddForeignKey("dbo.Booking", "StatusLookupId", "dbo.StatusLookup", "StatusLookupId", cascadeDelete: true);
            AddForeignKey("dbo.Booking", "SiteId", "dbo.Sites", "SiteId");
            AddForeignKey("dbo.Booking", "CustomerId", "dbo.Customer", "CustomerId", cascadeDelete: true);
        }
    }
}
