namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addParamInFleetReg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FleetsRegistration", "AttachmentId", c => c.Guid());
            CreateIndex("dbo.FleetsRegistration", "AttachmentId");
            AddForeignKey("dbo.FleetsRegistration", "AttachmentId", "dbo.Attachments", "AttachmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FleetsRegistration", "AttachmentId", "dbo.Attachments");
            DropIndex("dbo.FleetsRegistration", new[] { "AttachmentId" });
            DropColumn("dbo.FleetsRegistration", "AttachmentId");
        }
    }
}
