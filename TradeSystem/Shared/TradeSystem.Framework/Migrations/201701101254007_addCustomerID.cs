namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Withdraw", "CustomerId", c => c.Guid(nullable: true));
           

        }

        public override void Down()
        {
            DropColumn("dbo.Withdraw", "CustomerId");
        }
    }
}
