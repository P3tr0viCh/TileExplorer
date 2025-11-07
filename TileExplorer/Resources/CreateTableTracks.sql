CREATE TABLE tracks (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	text TEXT, datetimestart TEXT, datetimefinish TEXT,
	duration INTEGER, durationinmove INTEGER,
	distance REAL, eleascent REAL, eledescent REAL,
	equipmentid INTEGER
);