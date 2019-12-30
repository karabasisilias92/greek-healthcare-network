namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAMKAtypetostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "AMKA", c => c.String(nullable: false, maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "AMKA", c => c.Long(nullable: false));
        }
    }
}
