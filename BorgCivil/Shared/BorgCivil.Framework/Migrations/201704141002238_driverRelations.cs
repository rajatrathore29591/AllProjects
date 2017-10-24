namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverRelations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "StateId", c => c.Guid());
            AlterColumn("dbo.Drivers", "EmploymentCategoryId", c => c.Guid());
            CreateIndex("dbo.Drivers", "CountryId");
            CreateIndex("dbo.Drivers", "EmploymentCategoryId");
            CreateIndex("dbo.Drivers", "StateId");
            AddForeignKey("dbo.Drivers", "CountryId", "dbo.Country", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Drivers", "EmploymentCategoryId", "dbo.EmploymentCategory", "EmploymentCategoryId");
            AddForeignKey("dbo.Drivers", "StateId", "dbo.State", "Id");
            DropColumn("dbo.Drivers", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Drivers", "State", c => c.String(maxLength: 30));
            DropForeignKey("dbo.Drivers", "StateId", "dbo.State");
            DropForeignKey("dbo.Drivers", "EmploymentCategoryId", "dbo.EmploymentCategory");
            DropForeignKey("dbo.Drivers", "CountryId", "dbo.Country");
            DropIndex("dbo.Drivers", new[] { "StateId" });
            DropIndex("dbo.Drivers", new[] { "EmploymentCategoryId" });
            DropIndex("dbo.Drivers", new[] { "CountryId" });
            AlterColumn("dbo.Drivers", "EmploymentCategoryId", c => c.Guid(nullable: false));
            DropColumn("dbo.Drivers", "StateId");
        }
    }
}
