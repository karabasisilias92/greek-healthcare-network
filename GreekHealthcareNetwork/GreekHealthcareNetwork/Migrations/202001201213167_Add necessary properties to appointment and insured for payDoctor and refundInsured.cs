namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnecessarypropertiestoappointmentandinsuredforpayDoctorandrefundInsured : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "PaidByCompany", c => c.Boolean(nullable: false));
            AddColumn("dbo.Insureds", "RefundPending", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Insureds", "RefundPending");
            DropColumn("dbo.Appointments", "PaidByCompany");
        }
    }
}
