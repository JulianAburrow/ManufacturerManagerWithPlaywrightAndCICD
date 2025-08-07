IF NOT EXISTS (SELECT 1 FROM Colour)
BEGIN
	INSERT INTO Colour
		( Name )
	VALUES
		( 'Red' ),
		( 'Green' ),
		( 'Blue' ),
		( 'Pink' )
END