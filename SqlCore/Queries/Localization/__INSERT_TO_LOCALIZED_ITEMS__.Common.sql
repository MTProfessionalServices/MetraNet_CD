INSERT INTO t_localized_items
           (id_local_type
		   ,id_item
		   ,id_item_second_key
           ,id_lang_code
           ,tx_name
           ,tx_desc)
VALUES
           (%%LOCALIZED_TYPE%% 
		   ,%%ID_PARENT%%
		   ,%%ID_ITEM_SECOND_KEY%%
           ,%%ID_LANG%%
           ,'%%LOCALIZE_DISPLAY_NAME%%'
           ,'%%LOCALIZE_DESCRIPTION%%')

      