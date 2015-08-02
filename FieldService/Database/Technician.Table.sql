USE FieldService
GO

IF OBJECT_ID('Technician') IS NOT NULL
BEGIN
	EXEC sp_cdc_disable_table
		@source_schema='dbo',
		@source_name='Technician',
		@capture_instance='dbo_Technician'
	DROP TABLE Technician
END
GO

CREATE TABLE Technician(
	TechnicianId INT IDENTITY(1,1) NOT NULL,
	CONSTRAINT PK_Technician PRIMARY KEY (TechnicianId)
)
GO
