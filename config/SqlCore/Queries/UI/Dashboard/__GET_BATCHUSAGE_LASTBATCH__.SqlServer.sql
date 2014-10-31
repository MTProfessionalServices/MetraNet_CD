DECLARE @currentBatch DATETIME
DECLARE @currentBatchID INT
DECLARE @previousBatch DATETIME

SET @currentBatchID = (
        SELECT TOP 1 id_batch
        FROM   t_batch
        WHERE  (n_completed + n_failed + n_dismissed) > 0
        ORDER BY
               dt_crt DESC
    )

SET @currentBatch = (
        SELECT TOP 1 dt_crt
        FROM   t_batch
        WHERE  id_batch = @currentBatchID
    )

SET @previousBatch = (
        SELECT TOP 1 dt_crt
        FROM   t_batch
        WHERE  dt_crt < @currentBatch
               AND (n_completed + n_failed + n_dismissed) > 0
        ORDER BY
               dt_crt DESC
    )

SELECT @currentBatchID AS BATCHID,
       DATENAME(MM, @currentBatch) + ' ' + CAST(DAY(@currentBatch) AS VARCHAR(2))
		 AS [DATE],
       REPLACE(
           REPLACE(RIGHT(CONVERT(VARCHAR, @currentBatch), 7), 'AM', ' AM'),
           'PM', ' PM'
       ) AS [TIME],
       DATEDIFF(MINUTE, @previousBatch, @currentBatch) AS [TIME DIFF]
       WHERE @currentBatchID IS NOT NULL