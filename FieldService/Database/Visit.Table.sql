USE FieldService
GO

IF OBJECT_ID('Visit') IS NOT NULL
BEGIN
	EXEC sp_cdc_disable_table
		@source_schema='dbo',
		@source_name='Visit',
		@capture_instance='dbo_Visit'
	DROP TABLE Visit
END
GO

CREATE TABLE Visit(
	VisitId INT IDENTITY(1,1) NOT NULL,
	IncidentId INT NOT NULL,
	TechnicianId INT NOT NULL,
	StartTime DATETIME NOT NULL,
	EndTime DATETIME NOT NULL,
	CONSTRAINT PK_Visit PRIMARY KEY (VisitId),
	CONSTRAINT FK_Visit_Incident FOREIGN KEY (IncidentId) REFERENCES Incident,
	CONSTRAINT FK_Visit_Technician FOREIGN KEY (TechnicianId) REFERENCES Technician
)
GO

EXEC sp_cdc_enable_table
	@source_schema='dbo',
	@source_name='Visit',
	@role_name=NULL
