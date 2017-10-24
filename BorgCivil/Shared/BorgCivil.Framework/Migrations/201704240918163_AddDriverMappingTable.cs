namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDriverMappingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DriverInductionCard",
                c => new
                    {
                        DriverInductionCardId = c.Guid(nullable: false),
                        DriverId = c.Guid(),
                        CardNumber = c.String(maxLength: 50),
                        SiteCost = c.String(maxLength: 50),
                        IssueDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DriverInductionCardId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.DriverVocCard",
                c => new
                    {
                        DriverVocCardId = c.Guid(nullable: false),
                        DriverId = c.Guid(),
                        CardNumber = c.String(maxLength: 50),
                        RTONumber = c.String(maxLength: 50),
                        IssueDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DriverVocCardId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.DriverWhiteCard",
                c => new
                    {
                        DriverWhiteCardId = c.Guid(nullable: false),
                        DriverId = c.Guid(),
                        CardNumber = c.String(maxLength: 50),
                        IssueDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        IsActive = c.Boolean(),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DriverWhiteCardId)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.DriverId);
            
            AddColumn("dbo.Drivers", "EmergencyContactName", c => c.String(maxLength: 50));
            AddColumn("dbo.Drivers", "EmergencyContactNumber", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverWhiteCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverVocCard", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.DriverInductionCard", "DriverId", "dbo.Drivers");
            DropIndex("dbo.DriverWhiteCard", new[] { "DriverId" });
            DropIndex("dbo.DriverVocCard", new[] { "DriverId" });
            DropIndex("dbo.DriverInductionCard", new[] { "DriverId" });
            DropColumn("dbo.Drivers", "EmergencyContactNumber");
            DropColumn("dbo.Drivers", "EmergencyContactName");
            DropTable("dbo.DriverWhiteCard");
            DropTable("dbo.DriverVocCard");
            DropTable("dbo.DriverInductionCard");
        }
    }
}
