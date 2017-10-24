namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Country", "IsActive", c => c.Boolean(nullable: true));
            AlterColumn("dbo.State", "IsActive", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.State", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Country", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
