IF NOT EXISTS (SELECT 1 FROM WidgetStatus)
	BEGIN
	INSERT INTO WidgetStatus
		( StatusName )
	VALUES
		( 'Active' ),
		( 'Inactive' )
END