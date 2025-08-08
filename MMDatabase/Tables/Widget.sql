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