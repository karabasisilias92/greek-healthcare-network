namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createuniqueindexforemail : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AspNetUsers", "Email", true, "EmailIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "EmailIndex");
        }
    }
}
