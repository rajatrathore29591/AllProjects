namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnToProduct : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Product", "LastWeeklyWithdrawDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "LastWeeklyWithdrawDate", c => c.DateTime(nullable: false));
        }
    }
}
