namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anonymousField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnonymousField",
                c => new
                    {
                        AnonymousFieldId = c.Guid(nullable: false),
                        DriverId = c.Guid(),
                        Title = c.String(maxLength: 50),
                        Other1 = c.String(maxLength: 150),
                        Other2 = c.String(maxLength: 150),
                        IssueDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AnonymousFieldId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnonymousField", "DriverId", "dbo.Drivers");
            DropIndex("dbo.AnonymousField", new[] { "DriverId" });
            DropTable("dbo.AnonymousField");
        }
    }
}
