namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomerID1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Withdraw", "CustomerProductId", c => c.Guid(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
