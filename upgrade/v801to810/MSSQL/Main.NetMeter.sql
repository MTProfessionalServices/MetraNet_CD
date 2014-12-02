
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

PRINT N'Creating [dbo].[t_localized_items]'
GO

CREATE TABLE [dbo].[t_localized_items]
(
	[id_local_type] int NOT NULL,     				/* Composite key: This is foreign key to t_localized_items_type.*/
	[id_item] int NOT NULL,							/* Composite key: Localize identifier. This is foreign key to t_recevent and other tables */
	[id_item_second_key] int NOT NULL DEFAULT -1 ,	/* Composite key: Second localize identifier. This is foreign key, for example, to t_compositor (it is atomoc capability) and other tables with composite PK. In case second key is not used set -1 as default value */
	[id_lang_code] int NOT NULL,      				/* Composite key: Language identifier displayed on the MetraNet Presentation Server */
	[tx_name] [nvarchar](255) NULL, 				/* The localized DisplayName */
	[tx_desc] [nvarchar](2000) NULL,				/* The localized Description */
CONSTRAINT [PK_t_localized_items] PRIMARY KEY CLUSTERED 
(
[id_local_type],
[id_item],
[id_item_second_key],
[id_lang_code]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_localized_items] descriptions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The t_localized_items table contains the localized DisplayName and Description of entyties (for example t_recevent, t_composite_capability_type, t_atomic_capability_type tables) for the languages supported by the MetraTech platform.(Package:Pipeline) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Composite key: This is foreign key to t_localized_items_type.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items', @level2type=N'COLUMN',@level2name=N'id_local_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Composite key: Localize identifier. This is foreign key to t_recevent and other tables' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items', @level2type=N'COLUMN',@level2name=N'id_item'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Composite key: Second localize identifier. This is foreign key, for example, to t_compositor (it is atomoc capability) and other tables with composite PK. In case second key is not used set -1 as default value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items', @level2type=N'COLUMN',@level2name=N'id_item_second_key'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Composite key: Language identifier displayed on the MetraNet Presentation Server' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items', @level2type=N'COLUMN',@level2name=N'id_lang_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The localized DisplayName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items', @level2type=N'COLUMN',@level2name=N'tx_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The localized Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items', @level2type=N'COLUMN',@level2name=N'tx_desc'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Creating [dbo].[t_localized_items_type]'
GO
CREATE TABLE [dbo].[t_localized_items_type]
(
	[id_local_type] int NOT NULL,     	/* PK, where '1' - Localization type for Recurring Adapters,
													 '2' - 'Localization type for Composite Capability,
													 '3' - 'Localization type for Atomic Capability */	
	[local_type_description] [nvarchar](255) NULL,	/* The type description */
CONSTRAINT [PK_t_localized_items_type] PRIMARY KEY CLUSTERED 
(
[id_local_type]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_localized_items_type] descriptions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dictionary table for t_localized_items.id_local_type colum. Contains id localization type and their description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key.', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items_type', @level2type=N'COLUMN',@level2name=N'id_local_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description type.', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_localized_items_type', @level2type=N'COLUMN',@level2name=N'local_type_description'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_localized_items] adding FK1_LOCALIZE_TO_T_LOCALIZED_ITEMS_TYPE'
GO
	ALTER TABLE [dbo].[t_localized_items] WITH CHECK ADD CONSTRAINT [FK1_LOCALIZE_TO_T_LOCALIZED_ITEMS_TYPE] 
				FOREIGN KEY([id_local_type]) REFERENCES [t_localized_items_type] ([id_local_type])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Altering [dbo].[t_localized_items] adding FK2_LOCALIZE_TO_T_LANGUAGE'
GO
	ALTER TABLE [dbo].[t_localized_items] WITH CHECK ADD CONSTRAINT [FK2_LOCALIZE_TO_T_LANGUAGE] 
				FOREIGN KEY([id_lang_code]) REFERENCES [t_language] ([id_lang_code])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
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