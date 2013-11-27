
    CREATE GLOBAL TEMPORARY TABLE TMP_NRC 
   ( C_NRCEVENTTYPE NVARCHAR2(255) NULL, 
	 C_NRCINTERVALSTART DATE NOT NULL ENABLE, 
	 C_NRCINTERVALEND DATE NOT NULL ENABLE, 
	 C_NRCINTERVALSUBSCRIPTIONSTART DATE NOT NULL ENABLE, 
	 C_NRCINTERVALSUBSCRIPTIONEND DATE NOT NULL ENABLE, 	
     C__ACCOUNTID NUMBER(10,0) NOT NULL ENABLE, 
	 C__RESUBMIT NUMBER(10,0) NOT NULL ENABLE, 
	 C__PRICEABLEITEMINSTANCEID NUMBER(10,0) NOT NULL ENABLE, 
	 C__PRICEABLEITEMTEMPLATEID NUMBER(10,0) NOT NULL ENABLE, 
	 C__PRODUCTOFFERINGID NUMBER(10,0) NOT NULL ENABLE, 
	 C__COLLECTIONID BLOB, 
	 C__SUBSCRIPTIONID NUMBER(10,0) NOT NULL ENABLE, 
	 C__INTERVALID NUMBER(10,0),
	 ID_SOURCE_SESS RAW(16) null
	)ON COMMIT preserve ROWS 
	