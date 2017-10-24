namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "AllocationNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Booking", "AllocationNotes");
        }
    }
}
