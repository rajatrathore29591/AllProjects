namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverLicense : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Drivers", "LicenseClassId");
            AddForeignKey("dbo.Drivers", "LicenseClassId", "dbo.LicenseClass", "LicenseClassId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "LicenseClassId", "dbo.LicenseClass");
            DropIndex("dbo.Drivers", new[] { "LicenseClassId" });
        }
    }
}
