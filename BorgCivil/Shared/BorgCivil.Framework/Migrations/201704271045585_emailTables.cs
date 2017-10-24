namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SmtpServer = c.String(nullable: false, maxLength: 50),
                        SmtpPort = c.Int(nullable: false),
                        SmtpUserName = c.String(nullable: false, maxLength: 50),
                        SmtpPassword = c.String(nullable: false, maxLength: 50),
                        SmtpEmail = c.String(nullable: false, maxLength: 50),
                        IntervelTime = c.Int(nullable: false),
                        OrganisationId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Subject = c.String(maxLength: 255),
                        Body = c.String(),
                        Template = c.Int(nullable: false),
                        OrganisationId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailTemplate");
            DropTable("dbo.EmailSetting");
        }
    }
}
