namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCustomerColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "PostalStreetPoBox", c => c.String(maxLength: 30));
            AlterColumn("dbo.Customer", "OfficeState", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "OfficeState", c => c.String(maxLength: 20));
            DropColumn("dbo.Customer", "PostalStreetPoBox");
        }
    }
}
