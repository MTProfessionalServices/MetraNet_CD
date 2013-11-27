
	   CREATE TABLE dm_t_acc_usage_summ (
		id_sess bigint NOT NULL,
		id_acc int NOT NULL,
		id_payee int NOT NULL,
 		top_lvl_1 int NULL, top_lvl_1_desc varchar(200) NULL, 
 		top_lvl_2 int NULL, top_lvl_2_desc varchar(200) NULL, 
 		top_lvl_3 int NULL, top_lvl_3_desc varchar(200) NULL, 
 		top_lvl_4 int NULL, top_lvl_4_desc varchar(200) NULL, 
 		top_lvl_5 int NULL, top_lvl_5_desc varchar(200) NULL, 
 		top_lvl_6 int NULL, top_lvl_6_desc varchar(200) NULL,
 		top_lvl_7 int NULL, top_lvl_7_desc varchar(200) NULL,
 		top_lvl_8 int NULL, top_lvl_8_desc varchar(200) NULL,
 		top_lvl_9 int NULL, top_lvl_9_desc varchar(200) NULL,
 		top_lvl_10 int NULL, top_lvl_10_desc varchar(200) NULL,
		id_view int NOT NULL,
		id_usage_interval int NOT NULL,
		call_count int NULL,
		amount numeric(22,10) NOT NULL,
		am_currency nvarchar(3) NOT NULL,
		dt_crt datetime NULL,
		tax_federal numeric(22,10) NULL,
		tax_state numeric(22,10) NULL,
		tax_county numeric(22,10) NULL,
		tax_local numeric(22,10) NULL,
		tax_other numeric(22,10) NULL,
		view_desc nvarchar (255) NULL)
	     