namespace BorgCivil.Framework.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDemoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Demo",
                c => new
                    {
                        DemoId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        Address = c.String(maxLength: 4000),
                        CurrentDate = c.DateTime(),
                        RadioGender = c.String(maxLength: 100),
                        CheckBoxGender = c.String(maxLength: 4000),
                        DropDownGender = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.DemoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Demo");
        }
    }
}
