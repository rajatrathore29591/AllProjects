namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "Password", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "Password");
        }
    }
}
