USE FieldService
GO

IF OBJECT_ID('Outcome') IS NOT NULL
	DROP PROCEDURE Outcome
GO

CREATE PROCEDURE Outcome
	@IncidentId int,
	@VisitHash varchar(50)
AS
	DELETE FROM Scheduled
	WHERE IncidentId = @IncidentId
	  AND VisitHash = @VisitHash

GO
