USE FieldService
GO

IF OBJECT_ID('PartsOrder') IS NOT NULL
	DROP PROCEDURE PartsOrder
GO

CREATE PROCEDURE PartsOrder
	@IncidentId int,
	@PartsOrderHash varchar(50),
	@ExpectedDeliveryDate date
AS
	INSERT INTO AwaitingParts
		(IncidentId, PartsOrderHash, ExpectedDeliveryDate)
	VALUES
		(@IncidentId, @PartsOrderHash, @ExpectedDeliveryDate)

GO
