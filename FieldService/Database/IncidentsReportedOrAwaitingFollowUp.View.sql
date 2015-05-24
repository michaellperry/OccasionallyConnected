USE FieldService
GO

IF OBJECT_ID('IncidentsReportedOrAwaitingFollowUp') IS NOT NULL
	DROP VIEW IncidentsReportedOrAwaitingFollowUp
GO

CREATE VIEW IncidentsReportedOrAwaitingFollowUp
AS
	SELECT Incident.IncidentId
	FROM Incident
	JOIN Reported ON Incident.IncidentId = Reported.IncidentId

	UNION ALL

	SELECT Incident.IncidentId
	FROM Incident
	JOIN AwaitingFollowUp ON Incident.IncidentId = AwaitingFollowUp.IncidentId

GO