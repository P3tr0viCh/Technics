SELECT mileages.id, techid, techs.text AS techtext, datetime, mileage, description
FROM mileages
LEFT JOIN techs ON mileages.techid = techs.id
{0}
ORDER BY datetime DESC;