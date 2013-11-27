select seq_t_sys_upgrade.nextval from dual;

INSERT INTO t_sys_upgrade
(upgrade_id,target_db_version, dt_start_db_upgrade, db_upgrade_status)
VALUES
(seq_t_sys_upgrade.nextval,'6.0.4', sysdate, 'R');

commit;

ALTER TABLE T_BATCH
 ADD (n_dismissed  NUMBER(10) DEFAULT 0 NOT NULL);

ALTER TABLE T_BATCH ADD (
  CONSTRAINT UK_7_T_BATCH
 CHECK (n_dismissed >= 0));


UPDATE t_sys_upgrade
SET db_upgrade_status = 'C',
dt_end_db_upgrade = sysdate
WHERE upgrade_id = (SELECT MAX(upgrade_id) FROM t_sys_upgrade);

commit;
