namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverDocument : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Drivers", "ProfilePic");
            AddForeignKey("dbo.Drivers", "ProfilePic", "dbo.Document", "DocumentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "ProfilePic", "dbo.Document");
            DropIndex("dbo.Drivers", new[] { "ProfilePic" });
        }
    }
}
