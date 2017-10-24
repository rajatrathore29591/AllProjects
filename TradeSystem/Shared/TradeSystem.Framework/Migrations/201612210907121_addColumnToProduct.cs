namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "LastWeeklyWithdrawDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "LastWeeklyWithdrawDate");
        }
    }
}
