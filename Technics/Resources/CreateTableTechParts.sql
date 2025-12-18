CREATE TABLE techparts (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	techid INTEGER,
	partid INTEGER,
	datetimeinstall TEXT,
	datetimeremove TEXT
);