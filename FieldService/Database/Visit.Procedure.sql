USE FieldService
GO

IF OBJECT_ID('Visit') IS NOT NULL
	DROP PROCEDURE Visit
GO

CREATE PROCEDURE Visit
	@IncidentId int,
	@VisitHash varchar(50),
	@DateOfVisit date
AS
	INSERT INTO Scheduled
		(IncidentId, VisitHash, DateOfVisit)
	VALUES
		(@IncidentId, @VisitHash, @DateOfVisit)

GO
