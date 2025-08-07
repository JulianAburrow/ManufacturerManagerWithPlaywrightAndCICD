CREATE TABLE Manufacturer (
	ManufacturerId INT NOT NULL IDENTITY (1, 1),
	Name NVARCHAR(100) NOT NULL,
	StatusId INT NOT NULL,
	CONSTRAINT PK_Manufacturer PRIMARY KEY (ManufacturerId),
	CONSTRAINT FK_Manufacturer_ManufacturerStatus FOREIGN KEY (StatusId)
		REFERENCES ManufacturerStatus (StatusId)
)