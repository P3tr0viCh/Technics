SELECT IFNULL(SUM(mileage), 0.0)
FROM mileages
WHERE techid = :techid AND datetime <= :datetime;