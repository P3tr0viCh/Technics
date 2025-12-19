SELECT IFNULL(SUM(mileage), 0.0)
FROM mileages
WHERE techid = :techid AND datetime >= :datetimeinstall AND datetime < :datetimeremove;