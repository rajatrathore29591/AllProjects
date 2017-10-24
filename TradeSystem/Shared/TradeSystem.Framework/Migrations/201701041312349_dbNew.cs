namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "BankName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "BankName");
        }
    }
}
