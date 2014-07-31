/*
Run this script on:

        (local).NetMeter    -  This database will be modified

to synchronize it with:

        10.200.86.158.NetMeter

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.7.0 from Red Gate Software Ltd at 7/30/2014 8:53:39 AM

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping constraints from [dbo].[mvm_scheduled_tasks]'
GO
DECLARE @SQLString NVARCHAR(500);
DECLARE @constrName VARCHAR(50);
SELECT @constrName = constraint_name FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE TABLE_NAME = 'mvm_scheduled_tasks' AND column_name = 'mvm_poll_guid';
SET @SQLString = 'ALTER TABLE dbo.mvm_scheduled_tasks DROP CONSTRAINT ' + @constrName;
EXECUTE sp_executesql @SQLString;
/*ALTER TABLE [dbo].[mvm_scheduled_tasks] DROP CONSTRAINT [DF__mvm_sched__mvm_p__7E22B05D]*/
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping index [mvm_scheduled_tasks_ndx2] from [dbo].[mvm_scheduled_tasks]'
GO
DROP INDEX [mvm_scheduled_tasks_ndx2] ON [dbo].[mvm_scheduled_tasks]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[mvm_scheduled_tasks]'
GO
ALTER TABLE [dbo].[mvm_scheduled_tasks] ALTER COLUMN [mvm_poll_guid] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[mvm_scheduled_tasks] SET ( LOCK_ESCALATION = DISABLE )
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [mvm_scheduled_tasks_ndx2] on [dbo].[mvm_scheduled_tasks]'
GO
CREATE NONCLUSTERED INDEX [mvm_scheduled_tasks_ndx2] ON [dbo].[mvm_scheduled_tasks] ([mvm_poll_guid])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
