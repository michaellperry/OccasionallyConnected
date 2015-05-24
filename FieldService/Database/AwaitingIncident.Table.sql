USE FieldService
GO

IF OBJECT_ID('AwaitingParts') IS NOT NULL
	DROP TABLE AwaitingParts
GO

CREATE TABLE AwaitingParts (
	IncidentId int NOT NULL,
	PartsOrderHash varchar(50)

	CONSTRAINT FK_AwaitingParts_Incident FOREIGN KEY (IncidentId)
		REFERENCES Incident (IncidentId)
)
GO
