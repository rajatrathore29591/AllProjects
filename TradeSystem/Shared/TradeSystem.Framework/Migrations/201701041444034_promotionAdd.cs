namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promotionAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "Viewed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Promotions", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Promotions", "ModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotions", "ModifiedDate");
            DropColumn("dbo.Promotions", "CreatedDate");
            DropColumn("dbo.Promotions", "Viewed");
        }
    }
}
