USE FieldService
GO

IF OBJECT_ID('Incident') IS NOT NULL
BEGIN
	EXEC sp_cdc_disable_table
		@source_schema='dbo',
		@source_name='Incident',
		@capture_instance='dbo_Incident'
	DROP TABLE Incident
END
GO

CREATE TABLE Incident (
	IncidentId INT IDENTITY(1,1) NOT NULL,
	HomeId INT NOT NULL,
	Description VARCHAR(50),
	CONSTRAINT PK_Incident PRIMARY KEY (IncidentId),
	CONSTRAINT FK_Incident_Home FOREIGN KEY (IncidentId) REFERENCES Incident
)
GO

EXEC sp_cdc_enable_table
	@source_schema='dbo',
	@source_name='Incident',
	@role_name=NULL
