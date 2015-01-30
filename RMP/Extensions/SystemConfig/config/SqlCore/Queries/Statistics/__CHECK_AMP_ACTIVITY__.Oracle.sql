SELECT  
 		dbo.GenGuid() "ID", /* dummy filed as identifier for GridLayout*/	
		COALESCE(partition_name, N'Non-Partitioned') "PARTITION",
        TRAIL.DECISION_UNIQUE_ID,
        TRAIL.ID_USAGE_INTERVAL,
        TRAIL.END_DATE,
        TRAIL.ID_ACC,
        TRAIL.INTERVAL_START,
        TRAIL.INTERVAL_END,
        TRAIL.PARENT_DECISION_UNIQUE_ID,
        --DECISION_TYPE,
        INFO.TIER_COLUMN_GROUP AS DECISION_TYPE,
        TRAIL.INTERVALS_REMAINING,
        TRAIL.START_DATE,
        TRAIL.FINALIZATION_DATE,
        TRAIL.FINALIZED,
        TRAIL.EXPIRATION_DATE,
        TRAIL.EXPIRED,
        TRAIL.AUDIT_ONLY,
        TRAIL.TIER_START,
        TRAIL.TIER_END,
        TRAIL.QUALIFIED_TOTAL,
        TRAIL.QUALIFIED_EVENTS,
        TRAIL.QUALIFIED_UNITS,
        TRAIL.QUALIFIED_AMOUNT,
        TRAIL.TOTAL_GENERATED_EVENTS,
        TRAIL.TOTAL_GENERATED_AMOUNT,
        TRAIL.TOTAL_RATED_EVENTS,
        TRAIL.TOTAL_RATED_UNITS,
        TRAIL.TOTAL_RATED_AMOUNT_DELTA,
        TRAIL.TOTAL_RATED_AMOUNT_AFTER,
        TRAIL.TOTAL_DISCOUNTED_EVENTS,
        TRAIL.TOTAL_DISCOUNTED_UNITS,
        TRAIL.TOTAL_DISCOUNTED_AMOUNT,
        TRAIL.TOTAL_DISCOUNT_AMOUNT,
        TRAIL.PRE_QUALIFIED_TOTAL,
        TRAIL.PRE_QUALIFIED_EVENTS,
        TRAIL.PRE_QUALIFIED_UNITS,
        TRAIL.PRE_QUALIFIED_AMOUNT,
        TRAIL.PRE_TOTAL_GENERATED_EVENTS,
        TRAIL.PRE_TOTAL_GENERATED_AMOUNT,
        TRAIL.PRE_TOTAL_RATED_EVENTS,
        TRAIL.PRE_TOTAL_RATED_UNITS,
        TRAIL.PRE_TOTAL_RATED_AMOUNT_DELTA,
        TRAIL.PRE_TOTAL_RATED_AMOUNT_AFTER,
        TRAIL.PRE_TOTAL_DISCOUNTED_EVENTS,
        TRAIL.PRE_TOTAL_DISCOUNTED_UNITS,
        TRAIL.PRE_TOTAL_DISCOUNTED_AMOUNT,
        TRAIL.PRE_TOTAL_DISCOUNT_AMOUNT,
        TRAIL.TT_END
FROM agg_decision_audit_trail TRAIL
inner join agg_decision_info INFO on INFO.decision_unique_id = TRAIL.decision_unique_id 
left outer join vw_bus_partition_accounts bpt on bpt.id_acc = INFO.id_acc
where id_usage_interval = %%ID_INTERVAL%%
