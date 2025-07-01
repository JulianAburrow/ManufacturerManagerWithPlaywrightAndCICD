-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------
-------------------------------------------DO NOT------------------------------------------------------
----------------------------------UNDER ANY CIRCUMSTANCES----------------------------------------------
--------------------------------------RUN THIS SCRIPT--------------------------------------------------
-----------------------------------------ON A LIVE-----------------------------------------------------
-------------------------------------SERVER AS YOU WILL------------------------------------------------
---------------------------------------DELETE ALL THE--------------------------------------------------
---------------------------------DATA IN THE DATABASE !!!!!--------------------------------------------
-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------

USE Master
GO

IF EXISTS (SELECT * FROM sys.databases WHERE NAME = 'ManufacturerManagerWithMudBlazor')
	ALTER DATABASE [ManufacturerManagerWithMudBlazor] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
IF EXISTS (SELECT * FROM sys.databases WHERE NAME = 'ManufacturerManagerWithMudBlazor')
	DROP DATABASE ManufacturerManagerWithMudBlazor
GO

CREATE DATABASE ManufacturerManagerWithMudBlazor
GO

USE ManufacturerManagerWithMudBlazor
GO

CREATE TABLE ManufacturerStatus (
	StatusId INT NOT NULL IDENTITY (1, 1),
	StatusName NVARCHAR(20),
	CONSTRAINT PK_ManufacturerStatus PRIMARY KEY (StatusId)
)
GO

INSERT INTO ManufacturerStatus
	( StatusName )
VALUES
	( 'Active' ),
	( 'Inactive' )
GO

CREATE TABLE Manufacturer (
	ManufacturerId INT NOT NULL IDENTITY (1, 1),
	Name NVARCHAR(100) NOT NULL,
	StatusId INT NOT NULL,
	CONSTRAINT PK_Manufacturer PRIMARY KEY (ManufacturerId),
	CONSTRAINT FK_Manufacturer_ManufacturerStatus FOREIGN KEY (StatusId)
		REFERENCES ManufacturerStatus (StatusId)
)
GO

CREATE TABLE ColourJustification (
	ColourJustificationId INT NOT NULL IDENTITY (1, 1),
	Justification NVARCHAR(100) NOT NULL
	CONSTRAINT PK_ColourJustification PRIMARY KEY (ColourJustificationId)
)
GO

INSERT INTO ColourJustification
	( Justification )
VALUES
	( 'Customer request' ),
	( 'Just felt like it '),
	( 'Pink is the new Black' ),
	( 'It''s all the rage, darling!' )
GO

CREATE TABLE WidgetStatus (
	StatusId INT NOT NULL IDENTITY (1, 1),
	StatusName NVARCHAR(20),
	CONSTRAINT PK_WidgetStatus PRIMARY KEY (StatusId)
)
GO

INSERT INTO WidgetStatus
	( StatusName )
VALUES
	( 'Active' ),
	( 'Inactive' )
GO

CREATE TABLE Colour (
	ColourId INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(20) NOT NULL,
	CONSTRAINT PK_Colour PRIMARY KEY (ColourId)
)
GO

INSERT INTO Colour
	( Name )
VALUES
	( 'Red' ),
	( 'Green' ),
	( 'Blue' ),
	( 'Pink' )
GO

CREATE TABLE Widget (
	WidgetId INT NOT NULL IDENTITY (1, 1),
	Name NVARCHAR(100) NOT NULL,
	ManufacturerId INT NOT NULL,
	ColourId INT NULL,
	ColourJustificationId INT NULL,
	StatusId INT NOT NULL,
	CostPrice DECIMAL(18,2) NOT NULL,
	RetailPrice DECIMAL(18,2) NOT NULL,
	StockLevel INT NOT NULL,
	WidgetImage VARBINARY(MAX) NULL,
	CONSTRAINT PK_Widget PRIMARY KEY (WidgetId),
	CONSTRAINT FK_Widget_Manufacturer FOREIGN KEY (ManufacturerId)
		REFERENCES Manufacturer (ManufacturerId),
	CONSTRAINT FK_Widget_WidgetStatus FOREIGN KEY (StatusId)
		REFERENCES WidgetStatus (StatusId),
	CONSTRAINT FK_Widget_Colour FOREIGN KEY (ColourId)
		REFERENCES Colour (ColourId),
	CONSTRAINT FK_Widget_ColourJustification FOREIGN KEY (ColourJustificationId)
		REFERENCES ColourJustification (ColourJustificationId)
	)
GO