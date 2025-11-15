use subscription order by 1
select * from reftable where TenentID=5 and REFSUBTYPE='PRODTYPE'  AND REFTYPE='PRODTYPE'  order by 1
---select * into temp_REFTABLE from REFTABLE where TenentID=1 and REFSUBTYPE='Attachment'  AND REFTYPE='CRM';update temp_REFTABLE set TenentID=15 where TenentID=1;INSERT INTO REFTABLE SELECT * FROM temp_REFTABLE;drop table temp_REFTABLE;
use xamfopak_ERPMF
select * from tblproduct where  prodTypeRefId=99954 

