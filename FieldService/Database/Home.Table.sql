USE FieldService
GO

IF OBJECT_ID('Home') IS NOT NULL
BEGIN
	EXEC sp_cdc_disable_table
		@source_schema='dbo',
		@source_name='Home',
		@capture_instance='dbo_Home'
	DROP TABLE Home
END
GO

CREATE TABLE Home(
	HomeId INT IDENTITY(1,1) NOT NULL,
	Address VARCHAR(100) NOT NULL,
	CONSTRAINT PK_Home PRIMARY KEY (HomeId)
)
GO
