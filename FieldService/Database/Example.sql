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

