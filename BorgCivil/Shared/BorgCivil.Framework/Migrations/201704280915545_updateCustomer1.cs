namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCustomer1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "PostalState", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "PostalState", c => c.String(maxLength: 10));
        }
    }
}
