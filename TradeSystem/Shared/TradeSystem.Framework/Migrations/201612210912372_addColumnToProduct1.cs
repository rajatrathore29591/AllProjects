namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnToProduct1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "LastWeeklyWithdrawDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "LastWeeklyWithdrawDate");
        }
    }
}
