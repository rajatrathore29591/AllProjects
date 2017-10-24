namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "ContactNumber", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "ContactNumber");
        }
    }
}
