namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCityState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "City", c => c.String());
            AddColumn("dbo.Employee", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "ZipCode");
            DropColumn("dbo.Employee", "City");
        }
    }
}
