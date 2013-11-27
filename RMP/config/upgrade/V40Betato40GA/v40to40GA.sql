				set nocount on
				go
				DROP INDEX [dbo].[t_acc_usage].[fk3idx_t_acc_usage]
				GO
				DROP INDEX [dbo].[t_acc_usage].[fk4idx_t_acc_usage]
				GO
				ALTER TABLE T_message alter column tx_sc_serialized text
				go
				truncate table t_session_set
				go
				IF EXISTS (SELECT * FROM sysobjects WHERE id=OBJECT_ID('t_session_set')) DROP TABLE t_session_set
				GO
				CREATE TABLE [dbo].[t_session_set]
				(
				[id_message] [int] NOT NULL,
				[id_ss] [int] NOT NULL,
				[id_svc] [int] NOT NULL,
				[b_root] [char] (1) NOT NULL,
				[session_count] [int] NOT NULL
				)
				GO
				ALTER TABLE [dbo].[t_session_set] ADD CONSTRAINT [pk_t_session_set] PRIMARY KEY CLUSTERED ([id_ss])
				GO
				truncate table t_session
				go
				truncate table t_message
				go
				truncate table t_session_state
				go
				declare @var1 nvarchar(4000)
				declare c1 cursor for select table_name from information_schema.tables
				where table_name like 't_svc_%'
				and table_type = 'BASE TABLE'
				open c1
				fetch next from c1 into @var1
				while (@@fetch_status = 0)
				begin
					exec ('truncate table ' + @var1)
				fetch next from c1 into @var1
				end
				close c1
				deallocate c1
				go
				declare @var1 nvarchar(4000)
				declare c1 cursor for select table_name from information_schema.tables
				where table_name like 't_rerun_session_%'
				and table_type = 'BASE TABLE'
				open c1
				fetch next from c1 into @var1
				while (@@fetch_status = 0)
				begin
					exec ('drop table ' + @var1)
				fetch next from c1 into @var1
				end
				close c1
				deallocate c1
				go
				if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[archive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
				drop procedure [dbo].[archive]
				GO
				if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dearchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
				drop procedure [dbo].[dearchive]
				go
				if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ApprovePayments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
				drop procedure [dbo].[ApprovePayments]
				go
				if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReversePayments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
				drop procedure [dbo].[ReversePayments]
				go
				if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SequencedDeleteOwnedAccounts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
				drop procedure [dbo].[SequencedDeleteOwnedAccounts]
				go
				CREATE NONCLUSTERED INDEX [t_session_set_fk1idx] ON [dbo].[t_session_set] ([id_message])
				GO				
				
				
