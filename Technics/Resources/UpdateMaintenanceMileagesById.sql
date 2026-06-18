UPDATE
	maintenance
SET
	mileagecommon = @mileagecommon,
	mileageaftermaintenance = @mileageaftermaintenance
WHERE id = @id;