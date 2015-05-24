USE FieldService
GO

IF OBJECT_ID('Reported') IS NOT NULL
	DROP TABLE Reported
GO

CREATE TABLE Reported (
	ReportedId int IDENTITY(1, 1) NOT NULL,
	IncidentId int NOT NULL INDEX IX_Scheduled_Incident,

	CONSTRAINT PK_Reported PRIMARY KEY (ReportedId),
	CONSTRAINT FK_Reported_Incident FOREIGN KEY (IncidentId)
		REFERENCES Incident (IncidentId)
)
GO
