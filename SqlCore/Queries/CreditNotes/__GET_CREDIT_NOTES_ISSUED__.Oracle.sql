SELECT
	CN.c_CreditNoteID as 'ID',
	CONCAT(TEMPLATE.c_CreditNotePrefix, CAST(CN.c_CreditNoteID as varchar)) as 'CreditNoteIdentifier',
	CONCAT(SUBSCRIBER.c_FirstName, ' ', SUBSCRIBER.c_LastName) as 'AccountName',
	CONCAT(CREATOR.c_FirstName, ' ', CREATOR.c_LastName) as 'CreateBy',
	CN.c_CreationDate as 'CreateDate',
	AMOUNTS.TotalAmount as 'Amount',
	AMOUNTS.TotalTaxAmount as 'Tax',
	INTERNAL.c_Currency as 'Currency'
FROM t_be_cor_cre_creditnote CN
INNER JOIN (SELECT
							CN.c_CreditNote_Id,
							SUM(ISNULL(USAGE.amount,0)) + SUM(ISNULL(ADJUSTMENTS.AdjustmentAmount,0)) as 'TotalAmount',
							SUM(ISNULL(USAGE.tax_federal, 0)) + SUM(ISNULL(USAGE.tax_state, 0)) + SUM(ISNULL(USAGE.tax_county, 0)) + SUM(ISNULL(USAGE.tax_local, 0)) + SUM(ISNULL(USAGE.tax_other, 0)) +
							SUM(ISNULL(ADJUSTMENTS.aj_tax_federal, 0)) + SUM(ISNULL(ADJUSTMENTS.aj_tax_state, 0)) + SUM(ISNULL(ADJUSTMENTS.aj_tax_county, 0)) + SUM(ISNULL(ADJUSTMENTS.aj_tax_local, 0)) + SUM(ISNULL(ADJUSTMENTS.aj_tax_other, 0)) as 'TotalTaxAmount'
						FROM t_be_cor_cre_creditnote CN 
						INNER JOIN t_be_cor_cre_creditnoteitem CNI ON CNI.c_CreditNote_Id = CN.c_CreditNote_Id
						LEFT JOIN t_acc_usage USAGE ON USAGE.id_sess = CNI.c_SessionID
						LEFT JOIN t_adjustment_transaction ADJUSTMENTS ON ADJUSTMENTS.id_adj_trx = CNI.c_AdjustmentTransactionID
						GROUP BY CN.c_CreditNote_Id) AS AMOUNTS ON cn.c_CreditNote_Id = AMOUNTS.c_CreditNote_Id
INNER JOIN t_be_cor_cre_creditnotetmpl TEMPLATE ON TEMPLATE.c_CreditNoteTmpl_Id = CN.c_CreditNoteTmpl_Id
LEFT JOIN t_av_Contact SUBSCRIBER ON SUBSCRIBER.id_acc = CN.c_AccountID 
LEFT JOIN t_av_Contact CREATOR ON CREATOR.id_acc = CN.c_CreatorID
LEFT JOIN t_av_Internal INTERNAL ON INTERNAL.id_acc = CN.c_AccountID
WHERE CN.c_AccountID = %%ACCOUNTID%% OR %%ACCOUNTID%% IS NULL

