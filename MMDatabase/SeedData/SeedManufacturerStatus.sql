IF NOT EXISTS (SELECT 1 FROM ManufacturerStatus)
	BEGIN
	INSERT INTO ManufacturerStatus
		( StatusName )
	VALUES
		( 'Active' ),
		( 'Inactive' )
END