namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createtriggerafterinsertatappointmentstable : DbMigration
    {
        public override void Up()
        {
            Sql(
                @"
                CREATE TRIGGER CallDeleteAppointmentProcedureAfterInsert ON Appointments
                AFTER INSERT, UPDATE AS
                BEGIN
                    SET NOCOUNT ON;
	                DECLARE @Id INT;
                    DECLARE @AppointmentChargePaid BIT;
                    SELECT @Id = Id, @AppointmentChargePaid = AppointmentChargePaid FROM inserted;  
	                DECLARE @jobname NVARCHAR(MAX); -- job name
	                DECLARE @command NVARCHAR(MAX); -- command
                    
                    IF @AppointmentChargePaid = 0
                        BEGIN
	                        SET @jobname = CONCAT('DeleteIfAppointmentIsNotPaidOnTime', @Id);
	                        SET @command = CONCAT('EXEC dbo.DeleteIfAppointmentIsNotPaidOnTime @AppointmentId = ', @Id);

	                        exec msdb.dbo.sp_add_job
	                        @job_name = @jobname,
	                        @enabled=1,
                            @owner_login_name = 'sa',
	                        @start_step_id=1,
	                        @delete_level=1 --Job will delete itself after success

	                        exec msdb.dbo.sp_add_jobstep
	                        @job_name=@jobname,
                            @database_name = 'GreekHealthcareNetwork',
                            @subsystem = 'TSQL',
	                        @step_id=1,
	                        @step_name='step1',
	                        @command=@command

	                        exec msdb.dbo.sp_add_jobserver
	                        @job_name = @jobname,
                            @server_name = N'(LOCAL)'

	                        exec msdb.dbo.sp_start_job
	                        @job_name=@jobname
                        END
                    ELSE
                        BEGIN
							SET @jobname = CONCAT('DeleteIfAppointmentIsNotPaidOnTime', @Id);
							DECLARE @jobId binary(16)
							SELECT @jobId = job_id FROM msdb.dbo.sysjobs WHERE name = @jobname
							IF (@jobId IS NOT NULL)
							BEGIN
								exec msdb.dbo.sp_stop_job   
								@job_name = @jobname
								exec msdb.dbo.sp_delete_job
								@job_name = @jobname
							END                       
                        END
                END;"
            );
        }
        
        public override void Down()
        {
            Sql(@"
                DROP TRIGGER IF EXISTS CallDeleteAppointmentProcedureAfterInsert;
            ");
        }
    }
}
