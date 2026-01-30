SELECT parts.id, folderid, folders.text AS foldertext, parts.text, description
FROM parts
LEFT JOIN folders ON parts.folderid = folders.id;