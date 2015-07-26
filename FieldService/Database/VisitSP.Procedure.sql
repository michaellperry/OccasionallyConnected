USE FieldService
GO

IF OBJECT_ID('VisitSP') IS NOT NULL
	DROP PROCEDURE VisitSP
GO

CREATE PROCEDURE VisitSP
	@IncidentId int,
	@VisitHash varchar(50),
	@DateOfVisit date,
	@FollowUpOrderHash varchar(50) NULL
AS
	DELETE FROM Reported
	WHERE Reported.IncidentId = @IncidentId

	INSERT INTO Scheduled
		(IncidentId, VisitHash, DateOfVisit)
	VALUES
		(@IncidentId, @VisitHash, @DateOfVisit)

	DELETE FROM AwaitingFollowUp
	WHERE AwaitingFollowUp.IncidentId = @IncidentId
	  AND AwaitingFollowUp.FollowUpOrderHash = @FollowUpOrderHash

GO
