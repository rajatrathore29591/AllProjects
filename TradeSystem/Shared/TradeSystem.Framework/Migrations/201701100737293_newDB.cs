namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "WalletAmount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "WalletAmount");
        }
    }
}
