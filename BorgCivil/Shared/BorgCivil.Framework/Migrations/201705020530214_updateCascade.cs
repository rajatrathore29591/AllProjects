namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sites", "CustomerId", "dbo.Customer");
            AddForeignKey("dbo.Sites", "CustomerId", "dbo.Customer", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "CustomerId", "dbo.Customer");
            AddForeignKey("dbo.Sites", "CustomerId", "dbo.Customer", "CustomerId", cascadeDelete: true);
        }
    }
}
