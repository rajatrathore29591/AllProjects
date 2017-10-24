namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class withdraawadd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "WithdrawStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "WithdrawStatus");
        }
    }
}
