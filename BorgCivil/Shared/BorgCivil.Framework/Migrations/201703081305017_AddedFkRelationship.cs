namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFkRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "CompanyId", c => c.Guid());
            AddColumn("dbo.StatusLookup", "CompanyId", c => c.Guid());
            AddColumn("dbo.FleetTypes", "CompanyId", c => c.Guid());
            AddColumn("dbo.EmploymentCategory", "CompanyId", c => c.Guid());
            AddColumn("dbo.LicenseClass", "CompanyId", c => c.Guid());
            CreateIndex("dbo.Attachments", "CompanyId");
            CreateIndex("dbo.FleetTypes", "CompanyId");
            CreateIndex("dbo.StatusLookup", "CompanyId");
            CreateIndex("dbo.EmploymentCategory", "CompanyId");
            CreateIndex("dbo.LicenseClass", "CompanyId");
            AddForeignKey("dbo.FleetTypes", "CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.StatusLookup", "CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.EmploymentCategory", "CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.LicenseClass", "CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.Attachments", "CompanyId", "dbo.Companies", "CompanyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.LicenseClass", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.EmploymentCategory", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.StatusLookup", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.FleetTypes", "CompanyId", "dbo.Companies");
            DropIndex("dbo.LicenseClass", new[] { "CompanyId" });
            DropIndex("dbo.EmploymentCategory", new[] { "CompanyId" });
            DropIndex("dbo.StatusLookup", new[] { "CompanyId" });
            DropIndex("dbo.FleetTypes", new[] { "CompanyId" });
            DropIndex("dbo.Attachments", new[] { "CompanyId" });
            DropColumn("dbo.LicenseClass", "CompanyId");
            DropColumn("dbo.EmploymentCategory", "CompanyId");
            DropColumn("dbo.FleetTypes", "CompanyId");
            DropColumn("dbo.StatusLookup", "CompanyId");
            DropColumn("dbo.Attachments", "CompanyId");
        }
    }
}
