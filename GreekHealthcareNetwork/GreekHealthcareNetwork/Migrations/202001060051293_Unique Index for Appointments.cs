namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueIndexforAppointments : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            CreateIndex("dbo.Appointments", new[] { "DoctorId", "AppointmentDate", "AppointmentStartTime" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Appointments", new[] { "DoctorId", "AppointmentDate", "AppointmentStartTime" });
            CreateIndex("dbo.Appointments", "DoctorId");
        }
    }
}
