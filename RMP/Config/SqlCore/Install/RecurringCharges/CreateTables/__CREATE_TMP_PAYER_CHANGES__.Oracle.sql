CREATE GLOBAL TEMPORARY TABLE TMP_PAYER_CHANGES(
  C_RCINTERVALSTART DATE NOT NULL ENABLE,
  C_RCINTERVALEND DATE NOT NULL ENABLE,
  C_BILLINGINTERVALSTART DATE NOT NULL ENABLE,
  C_BILLINGINTERVALEND DATE NOT NULL ENABLE,
  C_RCINTERVALSUBSCRIPTIONSTART DATE NOT NULL ENABLE,
  C_RCINTERVALSUBSCRIPTIONEND DATE NOT NULL ENABLE,
  C_SUBSCRIPTIONSTART DATE NOT NULL ENABLE,
  C_SUBSCRIPTIONEND DATE,
  C_ADVANCE CHAR(1 BYTE) NOT NULL ENABLE,
  C_PRORATEONSUBSCRIPTION CHAR(1 BYTE) NOT NULL ENABLE,
  C_PRORATEINSTANTLY CHAR(1 BYTE) NOT NULL ENABLE,
  C_UNITVALUESTART DATE NOT NULL ENABLE,
  C_UNITVALUEEND DATE NOT NULL ENABLE,
  c_UnitValue DECIMAL(22,10),
  c_RatingType NUMBER(10,0) NOT NULL ENABLE,
  C_PRORATEONUNSUBSCRIPTION CHAR(1 BYTE) NOT NULL ENABLE,
  C_PRORATIONCYCLELENGTH NUMBER(10,0) NOT NULL ENABLE,
  C__ACCOUNTID NUMBER(10,0) NOT NULL ENABLE,
  C__PAYINGACCOUNT NUMBER(10,0) NOT NULL ENABLE,
  C__PRICEABLEITEMINSTANCEID NUMBER(10,0) NOT NULL ENABLE,
  C__PRICEABLEITEMTEMPLATEID NUMBER(10,0) NOT NULL ENABLE,
  C__PRODUCTOFFERINGID NUMBER(10,0) NOT NULL ENABLE,
  C_BILLEDRATEDATE DATE,
  C__SUBSCRIPTIONID NUMBER(10,0) NOT NULL ENABLE,
  C__INTERVALID NUMBER(10,0)
) ON COMMIT PRESERVE ROWS