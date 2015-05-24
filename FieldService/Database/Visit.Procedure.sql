USE FieldService
GO

IF OBJECT_ID('Visit') IS NOT NULL
	DROP PROCEDURE Visit
GO

CREATE PROCEDURE Visit
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
