SELECT id, techid, datetime
FROM mileages
WHERE datetime > :datetime
ORDER BY datetime DESC;