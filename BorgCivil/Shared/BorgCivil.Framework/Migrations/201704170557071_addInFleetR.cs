namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInFleetR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FleetsRegistration", "IsUpdated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FleetsRegistration", "IsUpdated");
        }
    }
}
