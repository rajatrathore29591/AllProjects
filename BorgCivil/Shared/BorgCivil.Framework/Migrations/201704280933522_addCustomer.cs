namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sites", "PoNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.Sites", "CreditTermAgreed", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sites", "CreditTermAgreed");
            DropColumn("dbo.Sites", "PoNumber");
        }
    }
}
