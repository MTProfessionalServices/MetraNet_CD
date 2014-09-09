SELECT * 
FROM (
  SELECT 
    ROW_NUMBER() OVER (ORDER BY SUM(NVL(ss.MRRPrimaryCurrency, 0.0))-SUM(NVL(prev.MRRPrimaryCurrency, 0.0)) DESC) AS ordernum, 
		NVL(props.nm_display_name, po.ProductOfferingName) AS productname,
		po.ProductOfferingId AS productcode,
	ss.Month, 
	SUM(NVL(ss.MRRPrimaryCurrency, 0.0)) AS MRR, 
	SUM(NVL(prev.MRRPrimaryCurrency, 0.0)) AS MRRPrevious, 
	SUM(NVL(ss.MRRPrimaryCurrency, 0.0))-SUM(NVL(prev.MRRPrimaryCurrency, 0.0)) AS MRRChange
  FROM SubscriptionSummary ss
  INNER JOIN ProductOffering po 
   ON po.ProductOfferingId = ss.ProductOfferingId 
   AND ss.InstanceId = po.InstanceId
  LEFT JOIN t_vw_base_props props on props.id_prop = po.ProductOfferingId AND props.id_lang_code = %%ID_LANG%%
  LEFT JOIN SubscriptionSummary prev 
   ON ss.InstanceId = prev.InstanceId 
   AND ss.ProductOfferingId = prev.ProductOfferingId 
   AND prev.Month = TO_NUMBER (TO_CHAR (ADD_MONTHS (SYSDATE, -2),'mm'))  
   AND prev.Year = TO_NUMBER (TO_CHAR (ADD_MONTHS (SYSDATE, -2),'yyyy'))
  WHERE ss.Month = TO_NUMBER (TO_CHAR (ADD_MONTHS (SYSDATE, -1),'mm')) 
   AND ss.Year = TO_NUMBER (TO_CHAR (ADD_MONTHS (SYSDATE, -1),'yyyy'))  
  GROUP BY ss.InstanceId, ss.Month, po.ProductOfferingId, po.ProductOfferingName, props.nm_display_name
  HAVING SUM(NVL(ss.MRRPrimaryCurrency, 0.0))-SUM(NVL(prev.MRRPrimaryCurrency, 0.0)) > 0 ) a
WHERE a.ordernum <=10 



