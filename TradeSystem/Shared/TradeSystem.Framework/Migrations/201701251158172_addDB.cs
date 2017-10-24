namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "LastWeeklyWithdrawDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "LastWeeklyWithdrawDate");
        }
    }
}
