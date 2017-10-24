namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Docket", "DocumentId");
            AddForeignKey("dbo.Docket", "DocumentId", "dbo.Document", "DocumentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Docket", "DocumentId", "dbo.Document");
            DropIndex("dbo.Docket", new[] { "DocumentId" });
        }
    }
}
