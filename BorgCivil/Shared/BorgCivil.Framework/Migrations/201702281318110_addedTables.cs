namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmploymentCategory",
                c => new
                    {
                        EmploymentCategoryId = c.Guid(nullable: false),
                        Category = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmploymentCategoryId);
            
            CreateTable(
                "dbo.LicenseClass",
                c => new
                    {
                        LicenseClassId = c.Guid(nullable: false),
                        Class = c.String(nullable: false, maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                        Gate_GateId = c.Guid(),
                    })
                .PrimaryKey(t => t.LicenseClassId)
                .ForeignKey("dbo.Gate", t => t.Gate_GateId)
                .Index(t => t.Gate_GateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LicenseClass", "Gate_GateId", "dbo.Gate");
            DropIndex("dbo.LicenseClass", new[] { "Gate_GateId" });
            DropTable("dbo.LicenseClass");
            DropTable("dbo.EmploymentCategory");
        }
    }
}
