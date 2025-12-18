SELECT techparts.id, techid, techs.text AS techtext, partid, parts.text AS parttext, datetimeinstall, datetimeremove
FROM techparts
LEFT JOIN techs ON techparts.techid = techs.id
LEFT JOIN parts ON techparts.partid = parts.id
{0}
ORDER BY datetimeinstall DESC;