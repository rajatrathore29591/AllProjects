namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFkInCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "CompanyId", c => c.Guid());
            CreateIndex("dbo.Customer", "CompanyId");
            AddForeignKey("dbo.Customer", "CompanyId", "dbo.Companies", "CompanyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Customer", new[] { "CompanyId" });
            DropColumn("dbo.Customer", "CompanyId");
        }
    }
}
