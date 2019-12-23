namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatetypeofAMKAdatatypeofDoBandSubscriptionEndDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "AMKA", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "AMKA", c => c.Int(nullable: false));
        }
    }
}
