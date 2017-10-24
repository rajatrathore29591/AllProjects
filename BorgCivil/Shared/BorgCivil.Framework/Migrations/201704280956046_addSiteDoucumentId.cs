namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSiteDoucumentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sites", "DocumentId", c => c.Guid());
            CreateIndex("dbo.Sites", "DocumentId");
            AddForeignKey("dbo.Sites", "DocumentId", "dbo.Document", "DocumentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sites", "DocumentId", "dbo.Document");
            DropIndex("dbo.Sites", new[] { "DocumentId" });
            DropColumn("dbo.Sites", "DocumentId");
        }
    }
}
