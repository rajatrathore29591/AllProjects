namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsdeletedColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Booking", "IsDeleted");
        }
    }
}
