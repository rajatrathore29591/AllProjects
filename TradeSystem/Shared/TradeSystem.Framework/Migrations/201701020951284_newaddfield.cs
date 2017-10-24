namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newaddfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "PaymentType", c => c.String());
            AddColumn("dbo.CustomerProduct", "ChargeId", c => c.String());
            AlterColumn("dbo.CustomerProduct", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerProduct", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.CustomerProduct", "ChargeId");
            DropColumn("dbo.CustomerProduct", "PaymentType");
        }
    }
}
