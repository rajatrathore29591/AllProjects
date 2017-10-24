namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedocument : DbMigration
    {
        public override void Up()
        {
            //DropTable("dbo.Documents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        OriginalName = c.String(maxLength: 100),
                        URL = c.String(maxLength: 255),
                        Title = c.String(maxLength: 100),
                        Description = c.String(maxLength: 4000),
                        Extension = c.String(maxLength: 20),
                        FileSize = c.Int(nullable: false),
                        Private = c.Boolean(nullable: false),
                        Tags = c.String(maxLength: 4000),
                        ThumbnailFileName = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId);
            
        }
    }
}
