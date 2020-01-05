namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsuredAppointmentChargepropertyinAppointments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "InsuredAppointmentCharge", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "InsuredAppointmentCharge");
        }
    }
}
