namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SentEmail",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ToEmail = c.String(nullable: false),
                        CcEmail = c.String(maxLength: 255),
                        BccEmail = c.String(maxLength: 255),
                        FromEmail = c.String(nullable: false, maxLength: 255),
                        Subject = c.String(nullable: false, maxLength: 255),
                        Message = c.String(),
                        DateSent = c.DateTime(),
                        DateFailed = c.DateTime(),
                        FailedReason = c.String(),
                        ModuleName = c.String(),
                        Archived = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Docket", "DocketDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Docket", "DocketDate");
            DropTable("dbo.SentEmail");
        }
    }
}
