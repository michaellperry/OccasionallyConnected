print 'Creating database'
:r Database.sql

print 'Creating tables'
:r Home.Table.sql
:r Incident.Table.sql
:r Technician.Table.sql
:r Visit.Table.sql
:r Reported.Table.sql
:r Scheduled.Table.sql
:r AwaitingFollowUp.Table.sql
:r AwaitingParts.Table.sql

print 'Creating views'
:r IncidentsScheduled.View.sql
:r IncidentsReportedOrAwaitingFollowUp.View.sql
:r IncidentsAwaitingParts.View.sql

print 'Creating procedures'
:r IncidentSP.Procedure.sql
:r VisitSP.Procedure.sql
:r FollowUpOrder.Procedure.sql
:r PartsOrder.Procedure.sql
:r PartsReceived.Procedure.sql
:r Outcome.Procedure.sql
