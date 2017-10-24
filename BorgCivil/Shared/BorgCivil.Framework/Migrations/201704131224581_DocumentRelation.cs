namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "DocumentId", c => c.Guid());
            CreateIndex("dbo.FleetTypes", "DocumentId");
            AddForeignKey("dbo.FleetTypes", "DocumentId", "dbo.Document", "DocumentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FleetTypes", "DocumentId", "dbo.Document");
            DropIndex("dbo.FleetTypes", new[] { "DocumentId" });
            DropColumn("dbo.Employee", "DocumentId");
        }
    }
}
