namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateStoredProceduretodeleteunpaidappointments : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                USE GreekHealthcareNetwork;
                GO

                CREATE PROCEDURE DeleteIfAppointmentIsNotPaidOnTime
	                @AppointmentId INT
                AS
                BEGIN
	                WAITFOR DELAY '00:10:03';
	                DECLARE @AppointmentChargePaid BIT;
	                SELECT @AppointmentChargePaid = AppointmentChargePaid FROM Appointments WHERE Id = @AppointmentId;
	                If @AppointmentChargePaid = 0 
	                BEGIN
		                DELETE FROM Appointments WHERE Id = @AppointmentId;
	                END
                END;
            ");
        }
        
        public override void Down()
        {
            Sql(@"
                USE GreekHealthcareNetwork;
                GO
                
                DROP PROCEDURE IF EXISTS DeleteIfAppointmentIsNotPaidOnTime;
                GO
            ");
        }
    }
}
