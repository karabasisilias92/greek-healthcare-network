namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScripttosetAppointmentStatustoCompleteautomatically : DbMigration
    {
        public override void Up()
        {
            Sql(@"USE [GreekHealthcareNetwork]
					GO

					/****** Object:  Job [UpdateAppointmentsToCompleted]    Script Date: 09-Jan-20 10:10:04 ******/
					BEGIN TRANSACTION
					DECLARE @ReturnCode INT
					SELECT @ReturnCode = 0
					/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 09-Jan-20 10:10:04 ******/
					IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
					BEGIN
					EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

					END
					DECLARE @jobId BINARY(16)
					EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'UpdateAppointmentsToCompleted', 
							@enabled=1, 
							@notify_level_eventlog=0, 
							@notify_level_email=0, 
							@notify_level_netsend=0, 
							@notify_level_page=0, 
							@delete_level=0, 
							@description=N'Updates appointmentStatus to Completed (1) when the appointmentDate  currentDate and the appointmentStatus = 0', 
							@category_name=N'[Uncategorized (Local)]', 
							@owner_login_name=N'sa', @job_id = @jobId OUTPUT
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					/****** Object:  Step [SetUpcomingToCompletedAfterAppDate]    Script Date: 09-Jan-20 10:10:05 ******/
					EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SetUpcomingToCompletedAfterAppDate', 
							@step_id=1, 
							@cmdexec_success_code=0, 
							@on_success_action=1, 
							@on_success_step_id=0, 
							@on_fail_action=2, 
							@on_fail_step_id=0, 
							@retry_attempts=0, 
							@retry_interval=0, 
							@os_run_priority=0, @subsystem=N'TSQL', 
							@command=N'SET QUOTED_IDENTIFIER ON;
										UPDATE dbo.Appointments 
										SET AppointmentStatus = 1 
										WHERE AppointmentDate < CONVERT(date,GETDATE()) AND AppointmentStatus = 0;', 
							@database_name=N'GreekHealthcareNetwork', 
							@flags=0
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'UpdateAppointmentStatus', 
							@enabled=1, 
							@freq_type=4, 
							@freq_interval=1, 
							@freq_subday_type=1, 
							@freq_subday_interval=0, 
							@freq_relative_interval=0, 
							@freq_recurrence_factor=0, 
							@active_start_date=20200109, 
							@active_end_date=99991231, 
							@active_start_time=1, 
							@active_end_time=235959, 
							@schedule_uid=N'ad68ae26-28b8-47f4-b7de-3477c0ec4123'
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					COMMIT TRANSACTION
					GOTO EndSave
					QuitWithRollback:
						IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
					EndSave:
					GO
                ");
        }
        
        public override void Down()
        {
			Sql(@"USE [GreekHealthcareNetwork]
					GO

					/****** Object:  Job [UpdateAppointmentsToCompleted]    Script Date: 09-Jan-20 11:40:19 ******/
					EXEC msdb.dbo.sp_delete_job @job_id=N'2304e19b-8232-4eba-8cf4-9fd4e91b88e2', @delete_unused_schedule=1
					GO");
        }
    }
}
