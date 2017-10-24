namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "StopCalculationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "StopCalculationDate");
        }
    }
}
