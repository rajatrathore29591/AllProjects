namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "CountryId", c => c.Guid());
            AddColumn("dbo.Employee", "StateId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "StateId");
            DropColumn("dbo.Employee", "CountryId");
        }
    }
}
