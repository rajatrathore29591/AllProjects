namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "StartCalculationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "StartCalculationDate");
        }
    }
}
