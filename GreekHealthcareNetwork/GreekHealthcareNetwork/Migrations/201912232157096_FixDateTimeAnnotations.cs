namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDateTimeAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DoB", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.AspNetUsers", "SubscriptionEndDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "SubscriptionEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "DoB", c => c.DateTime(nullable: false));
        }
    }
}
