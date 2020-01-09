namespace GreekHealthcareNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScripttoupdateIsActiveforuserswhenSubEndDatepasses : DbMigration
    {
        public override void Up()
        {
			Sql(@"USE [GreekHealthcareNetwork]
					GO

					/****** Object:  Job [UpdateUsersIsActive]    Script Date: 09-Jan-20 1:54:42 ******/
					BEGIN TRANSACTION
					DECLARE @ReturnCode INT
					SELECT @ReturnCode = 0
					/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 09-Jan-20 1:54:42 ******/
					IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
					BEGIN
					EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

					END

					DECLARE @jobId BINARY(16)
					EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'UpdateUsersIsActive', 
							@enabled=1, 
							@notify_level_eventlog=0, 
							@notify_level_email=0, 
							@notify_level_netsend=0, 
							@notify_level_page=0, 
							@delete_level=0, 
							@description=N'Checks if the users SubscriptonEndDate < Today and IsActive = 1 and makes them inactive.', 
							@category_name=N'[Uncategorized (Local)]', 
							@owner_login_name=N'sa', @job_id = @jobId OUTPUT
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					/****** Object:  Step [IsActiveCheckAndUpdate]    Script Date: 09-Jan-20 1:54:43 ******/
					EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'IsActiveCheckAndUpdate', 
							@step_id=1, 
							@cmdexec_success_code=0, 
							@on_success_action=1, 
							@on_success_step_id=0, 
							@on_fail_action=2, 
							@on_fail_step_id=0, 
							@retry_attempts=0, 
							@retry_interval=0, 
							@os_run_priority=0, @subsystem=N'TSQL', 
							@command=N'UPDATE AspNetUsers
										SET IsActive = 0
										WHERE SubscriptionEndDate != ''0001-01-01'' AND SubscriptionEndDate < CONVERT(date,GETDATE()) AND IsActive = 1;', 
							@database_name=N'GreekHealthcareNetwork', 
							@flags=0
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'IsActiveCheckAndUpdate', 
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
							@schedule_uid=N'87c38176-7f12-4ea2-bd96-de80d09b9a7c'
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
					COMMIT TRANSACTION
					GOTO EndSave
					QuitWithRollback:
						IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
					EndSave:
					GO");
        }
        
        public override void Down()
        {
			Sql(@"USE [GreekHealthcareNetwork]
				GO

				/****** Object:  Job [UpdateUsersIsActive]    Script Date: 09-Jan-20 11:41:43 ******/
				EXEC msdb.dbo.sp_delete_job @job_id=N'02601619-6b04-4467-b175-078396c977cf', @delete_unused_schedule=1
				GO");
        }
    }
}
