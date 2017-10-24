namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChangesOfGate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GateContactPerson",
                c => new
                    {
                        GateContactPersonId = c.Guid(nullable: false),
                        GateId = c.Guid(nullable: false),
                        ContactPerson = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(nullable: false, maxLength: 12),
                        IsDefault = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        EditedBy = c.Guid(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GateContactPersonId)
                .ForeignKey("dbo.Gate", t => t.GateId, cascadeDelete: true)
                .Index(t => t.GateId);
            
            AddColumn("dbo.Gate", "TipOffRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Gate", "TippingSite", c => c.String(maxLength: 300));
            DropColumn("dbo.Gate", "ContactPerson");
            DropColumn("dbo.Gate", "Email");
            DropColumn("dbo.Gate", "MobileNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gate", "MobileNumber", c => c.String(maxLength: 12));
            AddColumn("dbo.Gate", "Email", c => c.String(maxLength: 100));
            AddColumn("dbo.Gate", "ContactPerson", c => c.String(maxLength: 100));
            DropForeignKey("dbo.GateContactPerson", "GateId", "dbo.Gate");
            DropIndex("dbo.GateContactPerson", new[] { "GateId" });
            DropColumn("dbo.Gate", "TippingSite");
            DropColumn("dbo.Gate", "TipOffRate");
            DropTable("dbo.GateContactPerson");
        }
    }
}
