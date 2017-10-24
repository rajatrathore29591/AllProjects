namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "InvestmentWithdrawDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "InvestmentWithdrawDate");
        }
    }
}
