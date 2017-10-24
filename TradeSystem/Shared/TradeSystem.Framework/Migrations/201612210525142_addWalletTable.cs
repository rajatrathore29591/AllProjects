namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWalletTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wallet",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerId = c.Guid(),
                        AvailableBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.CustomerTransaction", "PaymentType", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.CustomerTransaction", "TransactionFor", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.CustomerTransaction", "Amount", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wallet", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Wallet", new[] { "CustomerId" });
            DropColumn("dbo.CustomerTransaction", "Amount");
            DropColumn("dbo.CustomerTransaction", "TransactionFor");
            DropColumn("dbo.CustomerTransaction", "PaymentType");
            DropTable("dbo.Wallet");
        }
    }
}
