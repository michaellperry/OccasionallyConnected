USE FieldService
GO

insert into Home
	(Address)
	values ('221B Baker Street')

declare @HomeId int = @@IDENTITY

insert into Incident
	(HomeId)
	values (@HomeId)

declare @IncidentId int = @@IDENTITY

insert into Technician
	default values

declare @TechnicianId int = @@IDENTITY

insert into Visit
	(IncidentId, TechnicianId)
	values (@IncidentId, @TechnicianId)


select *
from Home h
join Incident i on i.HomeId = h.HomeId
join Visit v on v.IncidentId = i.IncidentId
join Technician t on t.TechnicianId = v.TechnicianId

select *
from cdc.dbo_Home_CT

select *
from cdc.dbo_Incident_CT

select *
from cdc.dbo_Technician_CT

select *
from cdc.dbo_Visit_CT


update Home
set Address = '4214 Main Street'
where HomeId = 1


DECLARE @from_lsn binary(10), @to_lsn binary(10);
SET @from_lsn = sys.fn_cdc_get_min_lsn('dbo_Home');
--SET @from_lsn = sys.fn_cdc_increment_lsn(0x00000035000000F60001);
SET @to_lsn = sys.fn_cdc_get_max_lsn();
SELECT * from cdc.fn_cdc_get_all_changes_dbo_Home( @from_lsn, @to_lsn, 'all update old' );
