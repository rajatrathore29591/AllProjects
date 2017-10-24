namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleParam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "RoleId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employee", "RoleId");
            AddForeignKey("dbo.Employee", "RoleId", "dbo.AspNetRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.Employee", new[] { "RoleId" });
            DropColumn("dbo.Employee", "RoleId");
        }
    }
}
