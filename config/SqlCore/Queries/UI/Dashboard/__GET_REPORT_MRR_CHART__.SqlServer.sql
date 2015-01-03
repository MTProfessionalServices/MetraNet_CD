DECLARE @dateFrom DATE, @dateTo DATE
SET @dateFrom = DATEADD(month, -13, GETUTCDATE());
SET @dateTo = GETUTCDATE();--DATEADD(month, 12, CONVERT(DATE, CONCAT(DATEPART(month, GETUTCDATE()),'-','01','-',DATEPART(year, GETUTCDATE())), 110));
SELECT 
      CONVERT(DATE, CAST(sm.[Month] AS VARCHAR) + '-' + '01' + '-' + CAST(sm.[Year] AS VARCHAR), 110) as [Date],
      sm.ReportingCurrency as CurrencyCode,
      SUM(sm.MrrPrimaryCurrency) as Amount,
      ISNULL(lc.tx_desc, sm.ReportingCurrency)  AS LocalizedCurrency
FROM SubscriptionsByMonth sm
LEFT OUTER JOIN t_enum_data c on LOWER(c.nm_enum_data) = CONCAT('global/systemcurrencies/systemcurrencies/', LOWER(sm.ReportingCurrency))
LEFT OUTER JOIN t_description lc on lc.id_desc = c.id_enum_data and lc.id_lang_code = %%ID_LANG_CODE%%
WHERE CONVERT(DATE, CAST(sm.[Month] AS VARCHAR) + '-' + '01' + '-' + CAST(sm.[Year] AS VARCHAR), 110) >= @dateFrom
	AND CONVERT(DATE, CAST(sm.[Month] AS VARCHAR) + '-' + '01' + '-' + CAST(sm.[Year] AS VARCHAR), 110) <= @dateTo
GROUP BY  CONVERT(DATE, CAST(sm.[Month] AS VARCHAR) + '-' + '01' + '-' + CAST(sm.[Year] AS VARCHAR), 110), sm.ReportingCurrency, lc.tx_desc
