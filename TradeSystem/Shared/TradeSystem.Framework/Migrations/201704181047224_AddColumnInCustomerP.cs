namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnInCustomerP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "LastWithdrawMonthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "LastWithdrawMonthDate");
        }
    }
}
