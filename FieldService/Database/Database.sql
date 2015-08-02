use [master]
go

if exists( select top 1 0 from sys.databases where name = 'FieldService' )
begin
	alter database FieldService set single_user with rollback immediate
end
go

if exists( select top 1 0 from sys.databases where name = 'FieldService' )
begin
	drop database FieldService
end
go

create database FieldService
go

use FieldService
go

exec sp_cdc_enable_db
go
