namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFleetRegistration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subcontractor",
                c => new
                    {
                        SubcontractorId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SubcontractorId);
            
            AddColumn("dbo.FleetsRegistration", "SubcontractorId", c => c.Guid());
            CreateIndex("dbo.FleetsRegistration", "SubcontractorId");
            AddForeignKey("dbo.FleetsRegistration", "SubcontractorId", "dbo.Subcontractor", "SubcontractorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FleetsRegistration", "SubcontractorId", "dbo.Subcontractor");
            DropIndex("dbo.FleetsRegistration", new[] { "SubcontractorId" });
            DropColumn("dbo.FleetsRegistration", "SubcontractorId");
            DropTable("dbo.Subcontractor");
        }
    }
}
