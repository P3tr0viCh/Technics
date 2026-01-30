UPDATE
	techparts
SET
	mileage = @mileage,
	mileagecommon = @mileagecommon
WHERE id = @id;