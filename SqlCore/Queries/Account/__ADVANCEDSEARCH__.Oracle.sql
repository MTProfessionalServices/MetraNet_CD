
    SELECT DISTINCT "_ACCOUNTID" %%SORT_COLUMN%%
	    FROM ( SELECT * 
		       FROM (%%INNER_QUERY%%) iq1 
		       WHERE "NAME_SPACE" = "ANCESTORACCOUNTNS"
			 )
			