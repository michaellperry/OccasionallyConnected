USE FieldService
GO

IF OBJECT_ID('IncidentsScheduled') IS NOT NULL
	DROP VIEW IncidentsScheduled
GO

CREATE VIEW IncidentsScheduled
AS
	SELECT Incident.IncidentId, Scheduled.DateOfVisit
	FROM Incident
	JOIN Scheduled ON Incident.IncidentId = Scheduled.IncidentId

GO