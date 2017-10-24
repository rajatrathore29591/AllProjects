namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocketChanges1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Docket", "SupervisorId", c => c.Guid());
            CreateIndex("dbo.Docket", "SupervisorId");
            AddForeignKey("dbo.Docket", "SupervisorId", "dbo.Supervisor", "SupervisorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Docket", "SupervisorId", "dbo.Supervisor");
            DropIndex("dbo.Docket", new[] { "SupervisorId" });
            DropColumn("dbo.Docket", "SupervisorId");
        }
    }
}
