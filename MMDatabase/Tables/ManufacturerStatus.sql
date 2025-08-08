CREATE TABLE ManufacturerStatus (
	StatusId INT NOT NULL IDENTITY (1, 1),
	StatusName NVARCHAR (20) NOT NULL,
	CONSTRAINT PK_ManufacturerStatus PRIMARY KEY (StatusId)
)