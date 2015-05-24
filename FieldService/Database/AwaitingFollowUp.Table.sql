USE FieldService
GO

IF OBJECT_ID('AwaitingFollowUp') IS NOT NULL
	DROP TABLE AwaitingFollowUp
GO

CREATE TABLE AwaitingFollowUp (
	AwaitingFollowUpId int IDENTITY(1, 1) NOT NULL,
	IncidentId int NOT NULL INDEX IX_Scheduled_Incident,
	FollowUpOrderHash varchar(50) NOT NULL,

	CONSTRAINT PK_AwaitingFollowUp PRIMARY KEY (AwaitingFollowUpId),
	CONSTRAINT FK_AwaitingFollowUp_Incident FOREIGN KEY (IncidentId)
		REFERENCES Incident (IncidentId)
)
GO
