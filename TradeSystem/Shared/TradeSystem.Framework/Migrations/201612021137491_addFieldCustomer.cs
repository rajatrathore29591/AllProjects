namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldCustomer : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Customer", "DeviceToken", c => c.String());
            //AddColumn("dbo.Customer", "DeviceType", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Customer", "DeviceType");
            //DropColumn("dbo.Customer", "DeviceToken");
        }
    }
}
