namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPromotion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promotions", "To", c => c.String(nullable: false, maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Promotions", "To", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
