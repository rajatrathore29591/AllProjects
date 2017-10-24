namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDocument : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Document", "Name", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Document", "Name", c => c.String(maxLength: 100));
        }
    }
}
