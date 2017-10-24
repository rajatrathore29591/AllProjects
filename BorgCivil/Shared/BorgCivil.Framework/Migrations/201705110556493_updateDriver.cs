namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDriver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "DocumentId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "DocumentId");
        }
    }
}
