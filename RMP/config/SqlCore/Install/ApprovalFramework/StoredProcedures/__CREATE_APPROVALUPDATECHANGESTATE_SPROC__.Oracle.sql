
	  
CREATE OR REPLACE
PROCEDURE ApprovalUpdateChangeState(
    idApproval           INT,
    newState              VARCHAR,
    expectedPreviousState VARCHAR,
    changeModificationDate DATE,    
    status OUT INT)
AS
  currentState nvarchar2(50);
BEGIN
  SELECT c_CurrentState INTO currentState FROM t_approvals
    WHERE id_approval = idApproval;
	
  IF (currentState IS NULL) THEN
    BEGIN
      status := -1;
      /*Invalid change id */
      RETURN;
    END;
  END IF;
  
  IF ((expectedPreviousState IS NOT NULL) AND (currentState <> expectedPreviousState)) THEN
    BEGIN
      status := -2;
      /* State has changed since caller last read the change */
      RETURN;
    END;
  END IF;
  /* Verify the state change is a valid transition */
  status := -3;
  /* Assume invalid state change and update for valid state changes */
  IF (currentState = 'Pending') THEN
    status := CASE newState
    WHEN 'ApprovedWaitingToBeApplied' THEN 0
    WHEN 'Dismissed' THEN 0
    ELSE -3
    END;
  END IF;
  IF (currentState = 'Applied') THEN
    status := -3;
  END IF;
  IF (currentState = 'Dismissed') THEN
    status := -3;
  END IF;
  IF (currentState = 'ApprovedWaitingToBeApplied') THEN
    status := CASE newState
    WHEN 'Dismissed' THEN 0
    WHEN 'FailedToApply' THEN 0
    WHEN 'Applied' THEN 0
    ELSE -3
    END;
  END IF;
  IF (currentState = 'FailedToApply') THEN
    status := CASE newState
    WHEN 'ApprovedWaitingToBeApplied' THEN 0
    WHEN 'Dismissed' THEN 0
    WHEN 'Pending' THEN 0
      /* Details updated on change that failed to apply */
    WHEN 'FailedToApply' THEN 0
      /* Allow the state change to remain the same */
    ELSE -3
    END;
  END IF;
  IF (status = 0) THEN
    UPDATE t_approvals
    SET c_CurrentState = newState, c_ChangeLastModifiedDate = changeModificationDate WHERE t_approvals.id_approval = idApproval;
  END IF;
END;
	 