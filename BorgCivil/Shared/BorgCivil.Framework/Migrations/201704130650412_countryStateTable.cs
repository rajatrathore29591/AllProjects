namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryStateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CountryId = c.Guid(),
                        Name = c.String(maxLength: 100),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateIndex("dbo.Employee", "CountryId");
            CreateIndex("dbo.Employee", "StateId");
            AddForeignKey("dbo.Employee", "CountryId", "dbo.Country", "Id");
            AddForeignKey("dbo.Employee", "StateId", "dbo.State", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "StateId", "dbo.State");
            DropForeignKey("dbo.Employee", "CountryId", "dbo.Country");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.Employee", new[] { "StateId" });
            DropIndex("dbo.Employee", new[] { "CountryId" });
            DropTable("dbo.State");
            DropTable("dbo.Country");
        }
    }
}
