namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "OfficeState", c => c.String(maxLength: 20));
            DropColumn("dbo.Customer", "OfficeStreetPoBox");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customer", "OfficeStreetPoBox", c => c.String(maxLength: 20));
            DropColumn("dbo.Customer", "OfficeState");
        }
    }
}
