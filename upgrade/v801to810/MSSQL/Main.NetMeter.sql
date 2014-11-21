
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
DECLARE @version varchar(50)
SET @version='8.1.0'

    INSERT INTO [dbo].[t_sys_upgrade]
      (       
        target_db_version,
        dt_start_db_upgrade,
        db_upgrade_status
      )
    VALUES
      (
        @version,
        GETDATE(),
        'R'
      );
GO

PRINT N'Creating [dbo].[t_recevent_localize]'
GO
CREATE TABLE [dbo].[t_recevent_localize]
(
[id_local] [int] NOT NULL, --Localize identifier. This is foreign key to t_recevent
[id_lang_code] [int] NOT NULL, -- Language identifier displayed on the MetraNet Presentation Server
[tx_name] [nvarchar](255) NULL, -- The localized DisplayName
[tx_desc] [nvarchar](2048) NULL, -- The localized Description
CONSTRAINT [PK_t_recevent_localize] PRIMARY KEY CLUSTERED 
(
[id_local],
[id_lang_code]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_recevent_localize] adding FK1_LOCALIZE_TO_T_RECEVENT'
GO
	alter table [dbo].[t_recevent_localize] add constraint FK1_LOCALIZE_TO_T_RECEVENT
	foreign key (id_local) references t_recevent(id_event);
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_recevent_localize] adding FK2_LOCALIZE_TO_T_LANGUAGE'
GO
	alter table [dbo].[t_recevent_localize] add constraint FK2_LOCALIZE_TO_T_LANGUAGE
	foreign key (id_lang_code) references t_language(id_lang_code);
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_recevent_localize] descriptions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The t_recevent_localize table contains the localized DisplayName and DEscription of t_recevent table for the languages supported by the MetraTech platform.(Package:Pipeline) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_recevent_localize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Localize identifier. This is foreign key to t_recevent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_recevent_localize', @level2type=N'COLUMN',@level2name=N'id_desc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Language identifier displayed on the MetraNet Presentation Server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_recevent_localize', @level2type=N'COLUMN',@level2name=N'id_lang_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The localized DisplayName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_recevent_localize', @level2type=N'COLUMN',@level2name=N'tx_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The localized Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_recevent_localize', @level2type=N'COLUMN',@level2name=N'tx_desc'IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO




IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
UPDATE
    [dbo].[t_sys_upgrade]
SET
    [dt_end_db_upgrade] = GETDATE(),
	[db_upgrade_status] = 'C'
where [dt_end_db_upgrade] is null 
		and [db_upgrade_status] = 'R'
 
PRINT 'The database update succeeded on'	 
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO