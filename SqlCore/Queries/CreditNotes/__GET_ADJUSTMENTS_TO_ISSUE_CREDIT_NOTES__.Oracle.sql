SELECT 
	ALLADJUSTMENTS.AdjustmentID,
	ALLADJUSTMENTS.AdjustmentType,
	(case(ALLADJUSTMENTS.AdjustmentType) when 0 then 'Line Item' when 1 then 'Miscellaneous' end) AS AdjustmentTypeAsString,
	( -1 * ALLADJUSTMENTS.Amount) AS Amount,
	ALLADJUSTMENTS.Currency,
	ALLADJUSTMENTS.CreatedDate,
	ALLADJUSTMENTS.AdjustmentDescription,
  (TEMPLATE.c_CreditNotePrefix || CN.c_CreditNoteID) AS CreditNoteIdentifier
FROM
(
	SELECT 
		1 AS AdjustmentType,
		(NVL(USAGE.amount,0) + NVL(USAGE.tax_federal, 0) + NVL(USAGE.tax_state, 0) + NVL(USAGE.tax_county, 0) + NVL(USAGE.tax_local, 0) + NVL(USAGE.tax_other, 0)) AS Amount,
		USAGE.am_currency AS Currency,
		AC.c_CreditTime AS CreatedDate,
		AC.c_InvoiceComment AS AdjustmentDescription,
		AC.id_sess AS AdjustmentID
	FROM 
		t_pv_AccountCredit AC
		INNER JOIN t_acc_usage USAGE ON USAGE.id_sess = AC.id_sess
	WHERE 
		(AC.c_SubscriberAccountID = :AccountId OR USAGE.id_acc = @AccountId) AND AC.c_CreditTime BETWEEN :FromDate AND :ToDate
	UNION ALL
	SELECT 
		0 AS AdjustmentType,
		(NVL(ADJUSTMENTS.AdjustmentAmount,0) + NVL(ADJUSTMENTS.aj_tax_federal, 0) + NVL(ADJUSTMENTS.aj_tax_state, 0) + NVL(ADJUSTMENTS.aj_tax_county, 0) + NVL(ADJUSTMENTS.aj_tax_local, 0) + NVL(ADJUSTMENTS.aj_tax_other, 0)) AS Amount,
		ADJUSTMENTS.am_currency AS Currency,
		ADJUSTMENTS.dt_crt AS CreatedDate,
		ADJUSTMENTS.tx_desc AS AdjustmentDescription,
		ADJUSTMENTS.id_adj_trx AS AdjustmentID
	FROM 
		t_adjustment_transaction ADJUSTMENTS
	WHERE 
		ADJUSTMENTS.id_acc_payer = :AccountId AND ADJUSTMENTS.dt_crt BETWEEN :FromDate AND :ToDate
) ALLADJUSTMENTS 
LEFT JOIN t_be_cor_cre_creditnoteitem CNI ON  (CNI.c_SessionID = ALLADJUSTMENTS.AdjustmentID OR CNI.c_AdjustmentTransactionID = ALLADJUSTMENTS.AdjustmentID)
LEFT JOIN t_be_cor_cre_creditnote CN ON CN.c_CreditNote_Id = CNI.c_CreditNote_Id 
LEFT JOIN t_be_cor_cre_creditnotetmpl TEMPLATE ON TEMPLATE.c_CreditNoteTmpl_Id = CN.c_CreditNoteTmpl_Id