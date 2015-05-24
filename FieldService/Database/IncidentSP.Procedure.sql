USE FieldService
GO

IF OBJECT_ID('IncidentSP') IS NOT NULL
	DROP PROCEDURE IncidentSP
GO

CREATE PROCEDURE IncidentSP
	@IncidentId int
AS
	INSERT INTO Reported
		(IncidentId)
	VALUES
		(@IncidentId)

GO
