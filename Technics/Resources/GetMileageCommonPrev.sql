SELECT mileagecommon
FROM mileages
WHERE techid = :techid AND datetime < :datetime
ORDER BY datetime DESC
LIMIT 1;