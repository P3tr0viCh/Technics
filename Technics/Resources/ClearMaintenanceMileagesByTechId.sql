UPDATE
	maintenance
SET
	mileagecommon = null,
	mileageaftermaintenance = null
WHERE techid = @techid;