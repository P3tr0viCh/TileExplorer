CREATE TABLE IF NOT EXISTS tracks (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	text TEXT, datetimestart TEXT, datetimefinish TEXT,
	distance REAL, eleascent REAL, equipmentid INTEGER
);