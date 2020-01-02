namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentCancelationRefund : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InsuredPlans", "CancelationRefundPercentage", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InsuredPlans", "CancelationRefundPercentage");
        }
    }
}
