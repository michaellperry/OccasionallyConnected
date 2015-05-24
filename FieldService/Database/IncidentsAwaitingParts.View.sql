USE FieldService
GO

IF OBJECT_ID('IncidentsAwaitingParts') IS NOT NULL
	DROP VIEW IncidentsAwaitingParts
GO

CREATE VIEW IncidentsAwaitingParts
AS
	SELECT Incident.IncidentId, AwaitingParts.ExpectedDeliveryDate
	FROM Incident
	JOIN AwaitingParts ON Incident.IncidentId = AwaitingParts.IncidentId

GO