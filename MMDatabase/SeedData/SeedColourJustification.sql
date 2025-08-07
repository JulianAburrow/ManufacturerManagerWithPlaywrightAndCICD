IF NOT EXISTS (SELECT 1 FROM ColourJustification)
BEGIN
	INSERT INTO ColourJustification
		( Justification )
	VALUES
		( 'Customer request' ),
		( 'Just felt like it '),
		( 'Pink is the new Black' ),
		( 'It''s all the rage, darling!' )
END