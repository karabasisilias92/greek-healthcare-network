namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaypalAccounttoApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PaypalAccount", c => c.String());
            DropColumn("dbo.Doctors", "PaypalAccount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "PaypalAccount", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "PaypalAccount");
        }
    }
}
