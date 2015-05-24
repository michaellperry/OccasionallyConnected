USE FieldService
GO

IF OBJECT_ID('PartsReceived') IS NOT NULL
	DROP PROCEDURE PartsReceived
GO

CREATE PROCEDURE PartsReceived
	@IncidentId int,
	@PartsOrderHash varchar(50)
AS
	DELETE FROM AwaitingParts
	WHERE IncidentId = @IncidentId
	  AND PartsOrderHash = @PartsOrderHash

GO
