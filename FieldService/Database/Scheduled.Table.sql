USE FieldService
GO

IF OBJECT_ID('Scheduled') IS NOT NULL
	DROP TABLE Scheduled
GO

CREATE TABLE Scheduled (
	ScheduledId int IDENTITY(1, 1) NOT NULL,
	IncidentId int NOT NULL INDEX IX_Scheduled_Incident,
	VisitHash varchar(50) NOT NULL,
	DateOfVisit date NOT NULL,

	CONSTRAINT PK_Scheduled PRIMARY KEY (ScheduledId),
	CONSTRAINT FK_Scheduled_Incident FOREIGN KEY (IncidentId)
		REFERENCES Incident (IncidentId)
)
GO
