namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FleetsRegistration", "DocumentId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FleetsRegistration", "DocumentId");
        }
    }
}
