namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Office_Homeaddressmaxminlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Doctors", "OfficeAddress", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Doctors", "OfficeAddress", c => c.String(nullable: false));
        }
    }
}
