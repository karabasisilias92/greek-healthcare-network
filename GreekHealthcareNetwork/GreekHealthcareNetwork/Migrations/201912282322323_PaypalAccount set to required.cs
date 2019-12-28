namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaypalAccountsettorequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "PaypalAccount", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PaypalAccount", c => c.String());
        }
    }
}
