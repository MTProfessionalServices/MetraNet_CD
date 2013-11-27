select seq_t_sys_upgrade.nextval from dual;

INSERT INTO t_sys_upgrade
(upgrade_id,target_db_version, dt_start_db_upgrade, db_upgrade_status)
VALUES
(seq_t_sys_upgrade.nextval,'6.0.1', sysdate, 'R');

commit;
/*
*********** New tables
*/

CREATE GLOBAL TEMPORARY TABLE TMP_GETMVIEWQUERYTAGS
(
  COLUMN_VALUE  NVARCHAR2(2000)
)
ON COMMIT DELETE ROWS
NOCACHE;


CREATE TABLE T_WF_COMPLETEDSCOPE
(
  ID_INSTANCE        NVARCHAR2(36)              NOT NULL,
  ID_COMPLETEDSCOPE  NVARCHAR2(36)              NOT NULL,
  STATE              BLOB                       NOT NULL,
  DT_MODIFIED        DATE                       NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE INDEX IDX_CMPLTDSCOPE_COMPLETEDSCOPE ON T_WF_COMPLETEDSCOPE
(ID_COMPLETEDSCOPE)
LOGGING
NOPARALLEL;

CREATE INDEX IDX_CMPLTDSCOPE_ID_INSTANCE ON T_WF_COMPLETEDSCOPE
(ID_INSTANCE)
LOGGING
NOPARALLEL;


CREATE TABLE T_PAYMENT_INSTRUMENT
(
  ID_PAYMENT_INSTRUMENT   NVARCHAR2(36)         NOT NULL,
  ID_ACCT                 NUMBER(10),
  N_PAYMENT_METHOD_TYPE   INTEGER               NOT NULL,
  NM_TRUNCD_ACCT_NUM      NVARCHAR2(50)         NOT NULL,
  TX_HASH                 NVARCHAR2(255),
  ID_CREDITCARD_TYPE      NUMBER(10),
  N_ACCOUNT_TYPE          NUMBER(10),
  NM_EXP_DATE             NVARCHAR2(10),
  NM_EXP_DATE_FORMAT      NUMBER(10),
  NM_FIRST_NAME           NVARCHAR2(50)         NOT NULL,
  NM_MIDDLE_NAME          NVARCHAR2(50),
  NM_LAST_NAME            NVARCHAR2(50)         NOT NULL,
  NM_ADDRESS1             NVARCHAR2(255)        NOT NULL,
  NM_ADDRESS2             NVARCHAR2(255),
  NM_CITY                 NVARCHAR2(20)         NOT NULL,
  NM_STATE                NVARCHAR2(20),
  NM_ZIP                  NVARCHAR2(10),
  ID_COUNTRY              NUMBER(10)            NOT NULL,
  ID_PRIORITY             NUMBER(10),
  N_MAX_CHARGE_PER_CYCLE  NUMBER(18,6),
  DT_CREATED              DATE                  NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE UNIQUE INDEX PK_T_PAYMENT_INSTRUMENT ON T_PAYMENT_INSTRUMENT
(ID_PAYMENT_INSTRUMENT)
LOGGING
NOPARALLEL;

ALTER TABLE T_PAYMENT_INSTRUMENT
 ADD CONSTRAINT PK_T_PAYMENT_INSTRUMENT
 PRIMARY KEY
 (ID_PAYMENT_INSTRUMENT);

CREATE TABLE T_PENDING_PAYMENT_TRANS
(
  ID_INTERVAL            INTEGER                NOT NULL,
  ID_ACC                 INTEGER                NOT NULL,
  ID_PAYMENT_INSTRUMENT  NVARCHAR2(36)          NOT NULL,
  NM_INVOICE_NUM         NVARCHAR2(50)          NOT NULL,
  DT_INVOICE             DATE                   NOT NULL,
  NM_PO_NUMBER           NVARCHAR2(30),
  NM_DESCRIPTION         NVARCHAR2(100),
  NM_CURRENCY            NVARCHAR2(10)          NOT NULL,
  N_AMOUNT               NUMBER(18,6)           NOT NULL,
  ID_AUTHORIZATION       NVARCHAR2(36),
  B_CAPTURED             CHAR(1 BYTE)           NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE UNIQUE INDEX PK_T_PENDING_PAYMENT_TRANS ON T_PENDING_PAYMENT_TRANS
(ID_INTERVAL, ID_ACC, ID_PAYMENT_INSTRUMENT)
LOGGING
NOPARALLEL;

ALTER TABLE T_PENDING_PAYMENT_TRANS
 ADD CONSTRAINT PK_T_PENDING_PAYMENT_TRANS
 PRIMARY KEY
 (ID_INTERVAL, ID_ACC, ID_PAYMENT_INSTRUMENT);


CREATE TABLE T_FAILED_PAYMENT
(
  ID_INTERVAL            INTEGER                NOT NULL,
  ID_ACC                 INTEGER                NOT NULL,
  ID_PAYMENT_INSTRUMENT  NVARCHAR2(36)          NOT NULL,
  DT_ORIGINAL_TRANS      DATE                   NOT NULL,
  NM_INVOICE_NUM         NVARCHAR2(50)          NOT NULL,
  DT_INVOICE             DATE                   NOT NULL,
  NM_PO_NUMBER           NVARCHAR2(30),
  NM_DESCRIPTION         NVARCHAR2(100),
  NM_CURRENCY            NVARCHAR2(10)          NOT NULL,
  N_AMOUNT               NUMBER(18,6)           NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE UNIQUE INDEX PK_T_FAILED_PAYMENT ON T_FAILED_PAYMENT
(ID_INTERVAL, ID_ACC, ID_PAYMENT_INSTRUMENT)
LOGGING
NOPARALLEL;

ALTER TABLE T_FAILED_PAYMENT
 ADD CONSTRAINT PK_T_FAILED_PAYMENT
 PRIMARY KEY
 (ID_INTERVAL, ID_ACC, ID_PAYMENT_INSTRUMENT);


CREATE TABLE T_UI_EVENT_QUEUE
(
  ID_EVENT_QUEUE  NUMBER(10),
  ID_EVENT        NUMBER(10)                    NOT NULL,
  ID_ACC          NUMBER(10),
  DT_CRT          DATE                          NOT NULL,
  DT_VIEWED       DATE,
  B_DELETED       CHAR(1 BYTE)                  NOT NULL,
  B_BUBBLED       CHAR(1 BYTE)                  NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


ALTER TABLE T_UI_EVENT_QUEUE
 ADD PRIMARY KEY
 (ID_EVENT_QUEUE);

CREATE TABLE T_UI_EVENT
(
  ID_EVENT       NUMBER(10),
  TX_EVENT_TYPE  NVARCHAR2(50)                  NOT NULL,
  JSON_BLOB      NCLOB
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE T_UI_EVENT
 ADD PRIMARY KEY
 (ID_EVENT);

CREATE TABLE T_WF_INSTANCESTATE
(
  ID_INSTANCE    NVARCHAR2(36)                  NOT NULL,
  STATE          BLOB,
  N_STATUS       NUMBER(10),
  N_UNLOCKED     NUMBER(10),
  N_BLOCKED      NUMBER(10),
  TX_INFO        NCLOB,
  DT_MODIFIED    DATE                           NOT NULL,
  ID_OWNER       NVARCHAR2(36),
  DT_OWNEDUNTIL  DATE,
  DT_NEXTTIMER   DATE
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE UNIQUE INDEX IDX_INSTANCESTATE_ID_INSTANCE ON T_WF_INSTANCESTATE
(ID_INSTANCE)
LOGGING
NOPARALLEL;


CREATE TABLE T_WF_ACC_INST_MAP
(
  ID_ACC                NUMBER(10)              NOT NULL,
  WORKFLOW_TYPE         NVARCHAR2(250)          NOT NULL,
  ID_TYPE_INSTANCE      NVARCHAR2(36)           NOT NULL,
  ID_WORKFLOW_INSTANCE  NVARCHAR2(36)           NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE T_WF_ACC_INST_MAP
 ADD CONSTRAINT PK_T_WF_ACC_INST_MAP
 PRIMARY KEY
 (ID_ACC, WORKFLOW_TYPE, ID_TYPE_INSTANCE);

/*CREATE UNIQUE INDEX PK_T_WF_ACC_INST_MAP ON T_WF_ACC_INST_MAP
(ID_ACC, WORKFLOW_TYPE, ID_TYPE_INSTANCE)
LOGGING
NOPARALLEL;*/

CREATE TABLE T_PAYMENT_HISTORY
(
  ID_PAYMENT_TRANSACTION  VARCHAR2(40 BYTE)     NOT NULL,
  ID_ACCT                 NUMBER(10)            NOT NULL,
  DT_TRANSACTION          TIMESTAMP(6)          NOT NULL,
  N_PAYMENT_METHOD_TYPE   NUMBER(10)            NOT NULL,
  NM_TRUNCD_ACCT_NUM      NVARCHAR2(20)         NOT NULL,
  ID_CREDITCARD_TYPE      NUMBER(10),
  N_ACCOUNT_TYPE          NUMBER(10),
  NM_INVOICE_NUM          NVARCHAR2(50),
  DT_INVOICE_DATE         TIMESTAMP(6)          NOT NULL,
  NM_PO_NUMBER            NVARCHAR2(30),
  NM_DESCRIPTION          NVARCHAR2(100)        NOT NULL,
  N_CURRENCY              NVARCHAR2(10)         NOT NULL,
  N_AMOUNT                INTEGER               NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


ALTER TABLE T_PAYMENT_HISTORY
 ADD PRIMARY KEY
 (ID_PAYMENT_TRANSACTION);

CREATE TABLE T_PAYMENT_INSTRUMENT_XREF
(
  TEMP_ACC_ID  NUMBER(10)                       NOT NULL,
  NM_LOGIN     NVARCHAR2(255)                   NOT NULL,
  NM_SPACE     NVARCHAR2(40)                    NOT NULL,
  DT_CREATED   TIMESTAMP(6)                     NOT NULL
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE T_PAYMENT_INSTRUMENT_XREF
 ADD PRIMARY KEY
 (TEMP_ACC_ID, NM_LOGIN, NM_SPACE);


CREATE TABLE T_USER_CREDENTIALS_HISTORY
(
  NM_LOGIN     NVARCHAR2(255)                   NOT NULL,
  NM_SPACE     NVARCHAR2(40)                    NOT NULL,
  TX_PASSWORD  NVARCHAR2(1024)                  NOT NULL,
  TT_END       DATE
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


/*
*********** alter tables
*/

ALTER TABLE T_USER_CREDENTIALS
 ADD (DT_LAST_LOGOUT  DATE);

ALTER TABLE T_USER_CREDENTIALS
 ADD (DT_LAST_LOGIN  DATE);

ALTER TABLE T_RECUR
 ADD (N_UNIT_DISPLAY_NAME  NUMBER(10));

ALTER TABLE T_USER_CREDENTIALS
MODIFY(TX_PASSWORD NVARCHAR2(1024));


ALTER TABLE T_BASE_PROPS
MODIFY(NM_DESC NVARCHAR2(2000));


ALTER TABLE T_DESCRIPTION
MODIFY(TX_DESC NVARCHAR2(2000));


CREATE UNIQUE INDEX PK_T_USER_CREDENTIALS_HISTORY ON T_USER_CREDENTIALS_HISTORY
(NM_LOGIN, NM_SPACE, TT_END)
LOGGING
NOPARALLEL;

ALTER TABLE T_USER_CREDENTIALS_HISTORY
 ADD CONSTRAINT PK_T_USER_CREDENTIALS_HISTORY
 PRIMARY KEY
 (NM_LOGIN, NM_SPACE, TT_END);

ALTER TABLE T_BATCH
 DROP CONSTRAINT TX_STATUS_CHECK;
ALTER TABLE T_BATCH
 ADD CONSTRAINT TX_STATUS_CHECK
 CHECK (       UPPER(tx_status) = 'A' OR       UPPER(tx_status) = 'B' OR      UPPER(tx_status) = 'F' OR      UPPER(tx_status) = 'D' OR      UPPER(tx_status) = 'C');

ALTER TABLE T_USER_CREDENTIALS
 ADD (DT_EXPIRE  DATE);

ALTER TABLE T_USER_CREDENTIALS
 ADD (NUM_FAILURES_SINCE_LOGIN  NUMBER(10));

ALTER TABLE T_USER_CREDENTIALS
 ADD (DT_AUTO_RESET_FAILURES  DATE);

ALTER TABLE T_USER_CREDENTIALS
 ADD (B_ENABLED  NVARCHAR2(1));

ALTER TABLE T_RECEVENT_INST_AUDIT
 DROP CONSTRAINT CK1_T_RECEVENT_INST_AUDIT;
ALTER TABLE T_RECEVENT_INST_AUDIT
 ADD CONSTRAINT CK1_T_RECEVENT_INST_AUDIT
 CHECK (tx_action IN ('SubmitForExecution', 'SubmitForReversal','Acknowledge', 'Unacknowledge','Cancel', 'MarkAsSucceeded', 'MarkAsFailed','MarkAsNotYetRun'));

ALTER TABLE T_RECUR
 ADD (NM_UNIT_DISPLAY_NAME  NVARCHAR2(255));

ALTER TABLE T_RECUR
 ADD (N_UNIT_NAME  NUMBER(10));


/*
*********** Delete tables
*/

DROP TABLE T_PREAUTHORIZATIONLIST CASCADE CONSTRAINTS;

/*
*********** View
*/

create or replace force  view t_vw_base_props
as
select
  td_dispname.id_lang_code, bp.id_prop, bp.n_kind, bp.n_name, bp.n_desc,
  bp.nm_name as nm_name, td_desc.tx_desc as nm_desc, bp.b_approved, bp.b_archive,
  bp.n_display_name, td_dispname.tx_desc as nm_display_name
from t_base_props bp
  left join t_description td_dispname on td_dispname.id_desc = bp.n_display_name
  left join t_description td_desc on td_desc.id_desc = bp.n_desc and td_desc.id_lang_code = td_dispname.id_lang_code;
/

/*
*********** Sequence
*/                  

CREATE SEQUENCE SEQ_T_UI_EVENT
	INCREMENT BY 1
	START WITH 1
	MAXVALUE 999999999999999999999999999
	MINVALUE 1
	NOCYCLE
	CACHE 20
	NOORDER;

CREATE SEQUENCE SEQ_T_UI_EVENT_QUEUE
	INCREMENT BY 1
	START WITH 1
	MAXVALUE 999999999999999999999999999
	MINVALUE 1
	NOCYCLE
	CACHE 20
	NOORDER;

/*
*********** update t_current_id
*/

insert into t_current_id values (-2147483647, 'temp_acc_id');

UPDATE t_sys_upgrade
SET db_upgrade_status = 'C',
dt_end_db_upgrade = sysdate
WHERE upgrade_id = (SELECT MAX(upgrade_id) FROM t_sys_upgrade);

commit;
