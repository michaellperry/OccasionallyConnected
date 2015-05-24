USE FieldService
GO

IF OBJECT_ID('AwaitingParts') IS NOT NULL
	DROP TABLE AwaitingParts
GO

CREATE TABLE AwaitingParts (
	AwaitingPartsId int IDENTITY(1, 1) NOT NULL,
	IncidentId int NOT NULL INDEX IX_AwaitingParts_Indident,
	PartsOrderHash varchar(50) NOT NULL,
	ExpectedDeliveryDate date NOT NULL,

	CONSTRAINT PK_AwaitingParts PRIMARY KEY (AwaitingPartsId),
	CONSTRAINT FK_AwaitingParts_Incident FOREIGN KEY (IncidentId)
		REFERENCES Incident (IncidentId)
)
GO
