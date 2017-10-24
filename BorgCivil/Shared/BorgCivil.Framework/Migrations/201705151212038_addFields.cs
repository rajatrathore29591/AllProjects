namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "AnnualLeaveBalance", c => c.Int());
            AddColumn("dbo.Drivers", "SickLeaveBalance", c => c.Int());
            AddColumn("dbo.Drivers", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "City");
            DropColumn("dbo.Drivers", "SickLeaveBalance");
            DropColumn("dbo.Drivers", "AnnualLeaveBalance");
        }
    }
}
