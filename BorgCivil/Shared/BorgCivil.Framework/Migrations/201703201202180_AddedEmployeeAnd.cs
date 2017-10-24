namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmployeeAnd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeId = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        EmploymentCategoryId = c.Guid(nullable: false),
                        EmploymentStatusId = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 30),
                        SurName = c.String(maxLength: 30),
                        Awards = c.String(maxLength: 600),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                        CompanyId = c.Guid(),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.EmploymentCategory", t => t.EmploymentCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.StatusLookup", t => t.EmploymentStatusId, cascadeDelete: true)
                .Index(t => t.EmploymentCategoryId)
                .Index(t => t.EmploymentStatusId)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.Booking", "BookingNumber", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "EmploymentStatusId", "dbo.StatusLookup");
            DropForeignKey("dbo.Employee", "EmploymentCategoryId", "dbo.EmploymentCategory");
            DropForeignKey("dbo.Employee", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Employee", new[] { "CompanyId" });
            DropIndex("dbo.Employee", new[] { "EmploymentStatusId" });
            DropIndex("dbo.Employee", new[] { "EmploymentCategoryId" });
            DropColumn("dbo.Booking", "BookingNumber");
            DropTable("dbo.Employee");
        }
    }
}
