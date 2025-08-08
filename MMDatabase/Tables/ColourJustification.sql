CREATE TABLE ColourJustification (
	ColourJustificationId INT NOT NULL IDENTITY (1, 1),
	Justification NVARCHAR(100) NOT NULL
	CONSTRAINT PK_ColourJustification PRIMARY KEY (ColourJustificationId)
)