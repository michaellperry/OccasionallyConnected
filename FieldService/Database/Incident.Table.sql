IF OBJECT_ID('Incident') IS NOT NULL
	DROP TABLE Incident
GO

CREATE TABLE Incident (
	IncidentId int IDENTITY(1,1) NOT NULL,
	CONSTRAINT PK_Incident PRIMARY KEY (IncidentId)
)
GO
