namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDBAdd : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Ticket", "AutoIncrementedNo", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Ticket", "AutoIncrementedNo");
        }
    }
}
