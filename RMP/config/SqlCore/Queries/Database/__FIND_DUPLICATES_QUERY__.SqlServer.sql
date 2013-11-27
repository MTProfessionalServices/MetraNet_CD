
		begin
			if object_id('tempdb..#tmp_duplicates') is not null drop table #tmp_duplicates
				select a.id_sess,key_constraint=%%KEY_COLUMNS%%,1 as dbconflict into #tmp_duplicates from %%%NETMETERSTAGE_PREFIX%%%%%SRC_TABLE_NAME%%
				a %%%READCOMMITTED%%%, %%%NETMETER_PREFIX%%%%%DST_TABLE_NAME%% b %%%READCOMMITTED%%% where %%WHERE_CLAUSE%%
				insert into #tmp_duplicates select id_sess,%%KEY_COLUMNS%%,0 from %%%NETMETERSTAGE_PREFIX%%%%%SRC_TABLE_NAME%% a
				%%%READCOMMITTED%%% where exists (SELECT %%SELECT_COLUMNS%% FROM %%%NETMETERSTAGE_PREFIX%%%%%SRC_TABLE_NAME%% b
				%%%READCOMMITTED%%% where %%WHERE_CLAUSE%% GROUP BY %%GROUP_BY_LIST%% HAVING COUNT(1) > 1)
				and %%KEY_COLUMNS%% not in (select key_constraint from #tmp_duplicates)
		end
		