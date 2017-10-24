namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newchange : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomerProduct", "StopCalculationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerProduct", "StopCalculationDate", c => c.DateTime(nullable: false));
        }
    }
}
