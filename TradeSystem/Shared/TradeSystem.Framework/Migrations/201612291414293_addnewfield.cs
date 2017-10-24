namespace TradeSystem.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "OpenPayCustomerId", c => c.String(maxLength: 200));
            AddColumn("dbo.Country", "Description", c => c.String(maxLength: 200));
            AddColumn("dbo.State", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.Customer", "MiddleName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "MiddleName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.State", "Description");
            DropColumn("dbo.Country", "Description");
            DropColumn("dbo.Customer", "OpenPayCustomerId");
        }
    }
}
