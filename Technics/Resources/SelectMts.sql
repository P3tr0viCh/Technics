SELECT mts.id, folderid, folders.text AS foldertext, mts.text, description
FROM mts
LEFT JOIN folders ON mts.folderid = folders.id;