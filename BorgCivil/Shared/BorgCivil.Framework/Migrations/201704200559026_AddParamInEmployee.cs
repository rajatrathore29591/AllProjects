namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParamInEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "Address1", c => c.String());
            AddColumn("dbo.Employee", "Address2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "Address2");
            DropColumn("dbo.Employee", "Address1");
        }
    }
}
