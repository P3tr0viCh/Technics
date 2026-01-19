CREATE TABLE mileages (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	techid INTEGER,
	datetime TEXT,
	mileage REAL,
	mileagecommon REAL,
	mileagetype INTEGER,
	description TEXT,
	FOREIGN KEY (techid) REFERENCES techs (id)
	ON DELETE SET NULL
	ON UPDATE CASCADE
);