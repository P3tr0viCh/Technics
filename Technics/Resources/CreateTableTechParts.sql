CREATE TABLE techparts (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	techid INTEGER,
	partid INTEGER,
	datetimeinstall TEXT,
	datetimeremove TEXT,
	mileage REAL,
	mileagecommon REAL,
	FOREIGN KEY (techid) REFERENCES techs (id)
	ON DELETE SET NULL
	ON UPDATE CASCADE,
	FOREIGN KEY (partid) REFERENCES parts (id)
	ON DELETE SET NULL
	ON UPDATE CASCADE
);