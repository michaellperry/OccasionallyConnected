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

	IF NOT EXISTS (SELECT * FROM Scheduled
		WHERE IncidentId = @IncidentId
		  AND VisitHash = @VisitHash
		  AND DateOfVisit = @DateOfVisit)
	BEGIN
		INSERT INTO Scheduled
			(IncidentId, VisitHash, DateOfVisit)
		VALUES
			(@IncidentId, @VisitHash, @DateOfVisit)
	END

	DELETE FROM AwaitingFollowUp
	WHERE AwaitingFollowUp.IncidentId = @IncidentId
	  AND AwaitingFollowUp.FollowUpOrderHash = @FollowUpOrderHash

GO
