namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompanyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        OfficeStreet = c.String(maxLength: 100),
                        OfficeSuburb = c.String(maxLength: 50),
                        MobileNumber1 = c.String(maxLength: 12),
                        OfficePostalCode = c.String(maxLength: 10),
                        Fax = c.String(maxLength: 12),
                        Email = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
        
        }
        
        public override void Down()
        {
           
            DropTable("dbo.Companies");
        }
    }
}
