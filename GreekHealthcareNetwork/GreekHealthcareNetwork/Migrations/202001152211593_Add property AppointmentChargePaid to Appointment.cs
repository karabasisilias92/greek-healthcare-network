namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddpropertyAppointmentChargePaidtoAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentChargePaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "AppointmentChargePaid");
        }
    }
}
