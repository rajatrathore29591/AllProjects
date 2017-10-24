namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "WeeklyEarningStatus", c => c.String());
            AddColumn("dbo.CustomerProduct", "SaleEarningStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "SaleEarningStatus");
            DropColumn("dbo.CustomerProduct", "WeeklyEarningStatus");
        }
    }
}
