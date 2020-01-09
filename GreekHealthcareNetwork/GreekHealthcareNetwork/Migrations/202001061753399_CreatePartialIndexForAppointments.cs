namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePartialIndexForAppointments : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Appointments", "IX_DoctorId_AppointmentDate_AppointmentStartTime");
            CreateIndex("dbo.Appointments", "DoctorId");
            CreateIndex("dbo.Appointments", "InsuredId");
            Sql(@"SET ANSI_NULLS ON;
                SET QUOTED_IDENTIFIER ON;
                GO
                CREATE UNIQUE NONCLUSTERED INDEX
             [IX_FilteredIndexDoctorId_AppointmentDate_AppointmentStartTime_Upcoming] ON [dbo].[Appointments]
             (
                [DoctorId] ASC,
                [AppointmentDate] ASC,
                [AppointmentStartTime] ASC,
                [AppointmentStatus] ASC
             )
             WHERE ([AppointmentStatus]=(0))");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Appointments", "IX_FilteredIndexDoctorId_AppointmentDate_AppointmentStartTime_Upcoming");
            DropIndex("dbo.Appointments", new[] { "InsuredId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            CreateIndex("dbo.Appointments", new[] { "DoctorId", "InsuredId", "AppointmentDate", "AppointmentStartTime" }, unique: true, name: "IX_DoctorId_AppointmentDate_AppointmentStartTime");
        }
    }
}
