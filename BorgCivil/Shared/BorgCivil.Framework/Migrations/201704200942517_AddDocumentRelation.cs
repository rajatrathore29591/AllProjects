namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentRelation : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Employee", "DocumentId");
            AddForeignKey("dbo.Employee", "DocumentId", "dbo.Document", "DocumentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "DocumentId", "dbo.Document");
            DropIndex("dbo.Employee", new[] { "DocumentId" });
        }
    }
}
