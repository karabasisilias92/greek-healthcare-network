namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUniqueIndexForAppointments : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Appointments", new[] { "DoctorId", "AppointmentDate", "AppointmentStartTime" });
            DropIndex("dbo.Appointments", new[] { "InsuredId" });
            CreateIndex("dbo.Appointments", new[] { "DoctorId", "InsuredId", "AppointmentDate", "AppointmentStartTime" }, unique: true, name: "IX_DoctorId_AppointmentDate_AppointmentStartTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Appointments", "IX_DoctorId_AppointmentDate_AppointmentStartTime");
            CreateIndex("dbo.Appointments", "InsuredId");
            CreateIndex("dbo.Appointments", new[] { "DoctorId", "AppointmentDate", "AppointmentStartTime" }, unique: true);
        }
    }
}
