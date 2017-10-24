namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFKInBookingSiteGate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingSiteGates", "GateContactPersonId", c => c.Guid(nullable: true));
            CreateIndex("dbo.BookingSiteGates", "GateContactPersonId");
            AddForeignKey("dbo.BookingSiteGates", "GateContactPersonId", "dbo.GateContactPerson", "GateContactPersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingSiteGates", "GateContactPersonId", "dbo.GateContactPerson");
            DropIndex("dbo.BookingSiteGates", new[] { "GateContactPersonId" });
            DropColumn("dbo.BookingSiteGates", "GateContactPersonId");
        }
    }
}
