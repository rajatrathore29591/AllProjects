namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesEmployee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employee", "EmploymentStatusId", "dbo.StatusLookup");
            DropForeignKey("dbo.Employee", "EmploymentCategoryId", "dbo.EmploymentCategory");
            DropIndex("dbo.Employee", new[] { "EmploymentCategoryId" });
            DropIndex("dbo.Employee", new[] { "EmploymentStatusId" });
            AddColumn("dbo.Employee", "Email", c => c.String(maxLength: 100));
            AlterColumn("dbo.Employee", "EmploymentCategoryId", c => c.Guid());
            AlterColumn("dbo.Employee", "EmploymentStatusId", c => c.Guid());
            CreateIndex("dbo.Employee", "UserId");
            CreateIndex("dbo.Employee", "EmploymentCategoryId");
            CreateIndex("dbo.Employee", "EmploymentStatusId");
            AddForeignKey("dbo.Employee", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employee", "EmploymentStatusId", "dbo.StatusLookup", "StatusLookupId");
            AddForeignKey("dbo.Employee", "EmploymentCategoryId", "dbo.EmploymentCategory", "EmploymentCategoryId");
            DropColumn("dbo.Employee", "Awards");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employee", "Awards", c => c.String(maxLength: 600));
            DropForeignKey("dbo.Employee", "EmploymentCategoryId", "dbo.EmploymentCategory");
            DropForeignKey("dbo.Employee", "EmploymentStatusId", "dbo.StatusLookup");
            DropForeignKey("dbo.Employee", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Employee", new[] { "EmploymentStatusId" });
            DropIndex("dbo.Employee", new[] { "EmploymentCategoryId" });
            DropIndex("dbo.Employee", new[] { "UserId" });
            AlterColumn("dbo.Employee", "EmploymentStatusId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Employee", "EmploymentCategoryId", c => c.Guid(nullable: false));
            DropColumn("dbo.Employee", "Email");
            CreateIndex("dbo.Employee", "EmploymentStatusId");
            CreateIndex("dbo.Employee", "EmploymentCategoryId");
            AddForeignKey("dbo.Employee", "EmploymentCategoryId", "dbo.EmploymentCategory", "EmploymentCategoryId", cascadeDelete: true);
            AddForeignKey("dbo.Employee", "EmploymentStatusId", "dbo.StatusLookup", "StatusLookupId", cascadeDelete: true);
        }
    }
}
