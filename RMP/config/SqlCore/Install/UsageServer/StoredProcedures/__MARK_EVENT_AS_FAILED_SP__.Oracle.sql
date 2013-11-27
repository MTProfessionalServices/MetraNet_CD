
create or replace 
procedure MarkEventAsFailed(
  p_dt_now in date,   
  p_id_instance in int,   
  p_id_acc in int,   
  p_tx_detail in nvarchar2,   
  p_tx_machine varchar2,   
  p_status out int) 
as
  v_id_run int;
  tcount int;
begin
  p_status := -99;

  update t_recevent_inst
  set tx_status = 'Failed'
  where id_instance = p_id_instance
   and tx_status = 'Succeeded';

  if sql%rowcount = 1 then
    
    getcurrentid('receventrun', v_id_run);
    
    insert into t_recevent_run(id_run, id_instance, tx_type, id_reversed_run, 
      tx_machine, dt_start, dt_end, tx_status, tx_detail)
    values (v_id_run, p_id_instance, 'Execute', null, 
      p_tx_machine, p_dt_now, p_dt_now, 'Failed', 'Manually changed status to Failed');

    insert into t_recevent_inst_audit(id_audit, id_instance, id_acc, tx_action, 
      b_ignore_deps, dt_effective, tx_detail, dt_crt)
    values (seq_t_recevent_inst_audit.nextval, p_id_instance, p_id_acc, 'MarkAsFailed', 
      null, null, p_tx_detail, p_dt_now);

    p_status := 0;
    commit;
    return;
  end if;

  begin

    select count(1) into tcount
    from t_recevent_inst
    where id_instance = p_id_instance;

    if tcount = 0 then
      p_status := -1;
      rollback;
      return;
    end if;

  end;
  p_status := -2;
  rollback;
  return;

end MarkEventAsFailed;
  