DECLARE @LAST_NOTIFY_FROM DateTime
DECLARE @MAX_SUB_END_DATE DateTime
DECLARE @days int

SET @MAX_SUB_END_DATE = DATEADD(DAY, DATEDIFF(day, 0, @CURRENT_RUN_DATE), @NOTIFICATION_DAYS) + '23:59:59'
SET @LAST_NOTIFY_FROM = DATEADD(D, @NOTIFICATION_DAYS, @LAST_RUN_DATE) 
SET @days = 90  -- TO DO  if the end date for the subscription was set in the last 90 days, then don’t warn about it

SELECT
  'SUB' varSubType,
  sub.id_acc varAccount,
  varLoginName = 
  CASE 
    WHEN tmap_for_partitioned_account.nm_login IS NULL THEN tmap_for_nonpartitioned_account.nm_login
    ELSE tmap_for_partitioned_account.nm_login
  END,
  sub.vt_end varEndDate,
  sub.id_po varPOID,
  tbp.nm_display_name varDisplayName,
  t_po.c_POPartitionId varPartitionID
FROM t_sub sub
  INNER JOIN t_vw_base_props tbp ON sub.id_po = tbp.id_prop AND tbp.id_lang_code = @LANGUAGE_CODE
  INNER JOIN t_po ON sub.id_po = t_po.id_po
  LEFT OUTER JOIN t_account_mapper partition_owner ON partition_owner.id_acc = t_po.c_POPartitionId and partition_owner.nm_space = 'mt' and partition_owner.id_acc != 1
  /*now we will get the nm_login for the subscriber account*/
  LEFT OUTER JOIN t_account_mapper tmap_for_nonpartitioned_account ON tmap_for_nonpartitioned_account.id_acc = sub.id_acc and tmap_for_nonpartitioned_account.nm_space = 'mt'
  LEFT OUTER JOIN t_account_mapper tmap_for_partitioned_account ON tmap_for_partitioned_account.id_acc = sub.id_acc and tmap_for_partitioned_account.nm_space = partition_owner.nm_login  
WHERE sub.vt_end <= @MAX_SUB_END_DATE 
  AND sub.vt_end >= @LAST_NOTIFY_FROM
  AND sub.id_group IS NULL  
UNION
SELECT sq.varSubType, sq.varAccount, sq.varLoginName,sq.varEndDate, sq.varPOID, sq.varDisplayName, sq.varPartitionID
FROM(
       SELECT
         'SUB' varSubType,
         sub.id_acc varAccount,
         varLoginName = 
         CASE 
          WHEN tmap_for_partitioned_account.nm_login IS NULL THEN tmap_for_nonpartitioned_account.nm_login
          ELSE tmap_for_partitioned_account.nm_login
         END,
         sub.vt_end varEndDate,
         sub.id_po varPOID,
         tbp.nm_display_name varDisplayName,
         t_po.c_POPartitionId varPartitionID
       FROM t_sub sub
         INNER JOIN t_po ON sub.id_po = t_po.id_po
         LEFT OUTER JOIN t_account_mapper partition_owner ON partition_owner.id_acc = t_po.c_POPartitionId and partition_owner.nm_space = 'mt' and partition_owner.id_acc != 1
         /*now we will get the nm_login for the subscriber account*/
         LEFT OUTER JOIN t_account_mapper tmap_for_nonpartitioned_account ON tmap_for_nonpartitioned_account.id_acc = sub.id_acc and tmap_for_nonpartitioned_account.nm_space = 'mt'
         LEFT OUTER JOIN t_account_mapper tmap_for_partitioned_account ON tmap_for_partitioned_account.id_acc = sub.id_acc and tmap_for_partitioned_account.nm_space = partition_owner.nm_login  
         INNER JOIN t_sub_history tsh ON sub.id_sub = tsh.id_sub AND sub.vt_end = tsh.vt_end
         INNER JOIN t_sub_history tsh2 ON sub.id_sub = tsh2.id_sub
         INNER JOIN t_vw_base_props tbp ON sub.id_po = tbp.id_prop AND tbp.id_lang_code = @LANGUAGE_CODE
       WHERE sub.vt_end <= @LAST_NOTIFY_FROM
         AND sub.vt_end > @CURRENT_RUN_DATE
         AND sub.id_group IS NULL 
         AND tsh.dt_crt <= @LAST_NOTIFY_FROM
         AND tsh2.dt_crt >= @LAST_NOTIFY_FROM
         AND CAST(tsh2.dt_crt AS DATE) <= CAST(tsh.dt_crt AS DATE)
         AND tsh2.vt_end > @MAX_SUB_END_DATE
         AND tsh2.vt_end <> tsh.vt_end) sq
GROUP BY sq.varSubType, sq.varAccount, sq.varLoginName, sq.varEndDate, sq.varPOID, sq.varDisplayName, sq.varPartitionID