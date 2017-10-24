namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewFiled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerProduct", "BarCodeUrl", c => c.String());
            AddColumn("dbo.CustomerProduct", "BarCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerProduct", "BarCode");
            DropColumn("dbo.CustomerProduct", "BarCodeUrl");
        }
    }
}
