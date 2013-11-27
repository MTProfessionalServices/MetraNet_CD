
	begin
	if object_id('%%%NETMETERSTAGE_PREFIX%%%summ_delta_i_adj_usagedetail') is null
  exec ('create table %%%NETMETERSTAGE_PREFIX%%%summ_delta_i_adj_usagedetail 
				(
				id_sess bigint
				,id_usage_interval int
				,am_currency nvarchar(3)
				,id_acc int
				,c_status varchar(10)
				,CompoundPrebillAdjAmt numeric(38,6)
				,CompoundPostbillAdjAmt numeric(38,6)
				,CompoundPrebillAdjedAmt numeric(38,6)
				,CompoundPostbillAdjedAmt numeric(38,6)
				,AtomicPrebillAdjAmt numeric(38,6)
				,AtomicPrebillAdjedAmt numeric(38,6)
				,AtomicPostbillAdjAmt numeric(38,6)
				,AtomicPostbillAdjedAmt numeric(38,6)
				,CompoundPrebillFedTaxAdjAmt numeric(38,6)
				,CompoundPrebillStateTaxAdjAmt numeric(38,6)
				,CompoundPrebillCntyTaxAdjAmt numeric(38,6)
				,CompoundPrebillLocalTaxAdjAmt numeric(38,6)
				,CompoundPrebillOtherTaxAdjAmt numeric(38,6)
				,CompoundPrebillTotalTaxAdjAmt numeric(38,6)
				,CompoundPostbillFedTaxAdjAmt numeric(38,6)
				,CompoundPostbillStateTaxAdjAmt numeric(38,6)
				,CompoundPostbillCntyTaxAdjAmt numeric(38,6)
				,CompoundPostbillLocalTaxAdjAmt numeric(38,6)
				,CompoundPostbillOtherTaxAdjAmt numeric(38,6)
				,CompoundPostbillTotalTaxAdjAmt numeric(38,6)
				,AtomicPrebillFedTaxAdjAmt numeric(38,6)
				,AtomicPrebillStateTaxAdjAmt numeric(38,6)
				,AtomicPrebillCntyTaxAdjAmt numeric(38,6)
				,AtomicPrebillLocalTaxAdjAmt numeric(38,6)
				,AtomicPrebillOtherTaxAdjAmt numeric(38,6)
				,AtomicPrebillTotalTaxAdjAmt numeric(38,6)
				,AtomicPostbillFedTaxAdjAmt numeric(38,6)
				,AtomicPostbillStateTaxAdjAmt numeric(38,6)
				,AtomicPostbillCntyTaxAdjAmt numeric(38,6)
				,AtomicPostbillLocalTaxAdjAmt numeric(38,6)
				,AtomicPostbillOtherTaxAdjAmt numeric(38,6)
				,AtomicPostbillTotalTaxAdjAmt numeric(38,6)
				,IsPrebillTransaction char(1)
				,IsAdjusted char(1)
				,IsPrebillAdjusted char(1)
				,IsPostBillAdjusted char(1)
				,CanAdjust char(1)
				,CanRebill char(1)
				,CanManageAdjustments char(1)
				,PrebillAdjustmentID int
				,PostbillAdjustmentID int
				,IsIntervalSoftClosed char(1),
				NumTransactions int)
				')
				
	if object_id('%%%NETMETERSTAGE_PREFIX%%%summ_delta_d_adj_usagedetail') is null
  exec	('create table %%%NETMETERSTAGE_PREFIX%%%summ_delta_d_adj_usagedetail 
				(
				id_sess bigint
				,id_usage_interval int
				,am_currency nvarchar(3)
				,id_acc int
				,c_status varchar(10)
				,CompoundPrebillAdjAmt numeric(38,6)
				,CompoundPostbillAdjAmt numeric(38,6)
				,CompoundPrebillAdjedAmt numeric(38,6)
				,CompoundPostbillAdjedAmt numeric(38,6)
				,AtomicPrebillAdjAmt numeric(38,6)
				,AtomicPrebillAdjedAmt numeric(38,6)
				,AtomicPostbillAdjAmt numeric(38,6)
				,AtomicPostbillAdjedAmt numeric(38,6)
				,CompoundPrebillFedTaxAdjAmt numeric(38,6)
				,CompoundPrebillStateTaxAdjAmt numeric(38,6)
				,CompoundPrebillCntyTaxAdjAmt numeric(38,6)
				,CompoundPrebillLocalTaxAdjAmt numeric(38,6)
				,CompoundPrebillOtherTaxAdjAmt numeric(38,6)
				,CompoundPrebillTotalTaxAdjAmt numeric(38,6)
				,CompoundPostbillFedTaxAdjAmt numeric(38,6)
				,CompoundPostbillStateTaxAdjAmt numeric(38,6)
				,CompoundPostbillCntyTaxAdjAmt numeric(38,6)
				,CompoundPostbillLocalTaxAdjAmt numeric(38,6)
				,CompoundPostbillOtherTaxAdjAmt numeric(38,6)
				,CompoundPostbillTotalTaxAdjAmt numeric(38,6)
				,AtomicPrebillFedTaxAdjAmt numeric(38,6)
				,AtomicPrebillStateTaxAdjAmt numeric(38,6)
				,AtomicPrebillCntyTaxAdjAmt numeric(38,6)
				,AtomicPrebillLocalTaxAdjAmt numeric(38,6)
				,AtomicPrebillOtherTaxAdjAmt numeric(38,6)
				,AtomicPrebillTotalTaxAdjAmt numeric(38,6)
				,AtomicPostbillFedTaxAdjAmt numeric(38,6)
				,AtomicPostbillStateTaxAdjAmt numeric(38,6)
				,AtomicPostbillCntyTaxAdjAmt numeric(38,6)
				,AtomicPostbillLocalTaxAdjAmt numeric(38,6)
				,AtomicPostbillOtherTaxAdjAmt numeric(38,6)
				,AtomicPostbillTotalTaxAdjAmt numeric(38,6)
				,IsPrebillTransaction char(1)
				,IsAdjusted char(1)
				,IsPrebillAdjusted char(1)
				,IsPostBillAdjusted char(1)
				,CanAdjust char(1)
				,CanRebill char(1)
				,CanManageAdjustments char(1)
				,PrebillAdjustmentID int
				,PostbillAdjustmentID int
				,IsIntervalSoftClosed char(1),
				NumTransactions int)
				');
		    end
	