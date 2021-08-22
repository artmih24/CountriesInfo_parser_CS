use master
go

alter database Countries_DB set SINGLE_USER WITH ROLLBACK IMMEDIATE;
go

drop database Countries_DB
go