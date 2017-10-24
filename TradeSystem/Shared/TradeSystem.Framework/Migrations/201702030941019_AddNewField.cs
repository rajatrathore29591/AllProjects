namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "LastWeeklyWithdrawEnableDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "LastWeeklyWithdrawEnableDate");
        }
    }
}
