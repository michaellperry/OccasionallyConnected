USE FieldService
GO

IF OBJECT_ID('FollowUpOrder') IS NOT NULL
	DROP PROCEDURE FollowUpOrder
GO

CREATE PROCEDURE FollowUpOrder
	@IncidentId int,
	@FollowUpOrderOrderHash varchar(50)
AS
	INSERT INTO AwaitingFollowUp
		(IncidentId, FollowUpOrderHash)
	VALUES
		(@IncidentId, @FollowUpOrderOrderHash)

GO
