SELECT mileages.id, techid, techs.text AS techtext, datetime, mileage, mileagecommon, description
FROM mileages
LEFT JOIN techs ON mileages.techid = techs.id