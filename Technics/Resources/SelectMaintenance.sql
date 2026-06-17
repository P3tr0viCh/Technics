SELECT
	maintenance.id, techid, techs.text AS techtext, mtid, mts.text AS mttext,
	datetime,
	mileagecommon, mileageaftermaintenance
FROM maintenance
LEFT JOIN techs ON maintenance.techid = techs.id
LEFT JOIN mts ON maintenance.mtid = mts.id