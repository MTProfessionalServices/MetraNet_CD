
CREATE TABLE t_recevent_run_details
(
  id_detail INT IDENTITY(1000,1) NOT NULL,
  id_run INT NOT NULL,
  tx_type CHAR(7) NOT NULL,         -- type of detail: debug, info, warning
  tx_detail nvarchar(4000) NOT NULL, -- details generated by adapter
  dt_crt DATETIME NOT NULL,
  CONSTRAINT PK1_t_recevent_run_details PRIMARY KEY (id_detail),
  CONSTRAINT FK1_t_recevent_run_details FOREIGN KEY (id_run) REFERENCES t_recevent_run (id_run),
  CONSTRAINT CK1_t_recevent_run_details CHECK (tx_type IN ('Debug', 'Info', 'Warning'))
)
			 