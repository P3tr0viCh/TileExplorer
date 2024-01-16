CREATE TABLE IF NOT EXISTS tracks_points (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	trackid INTEGER, num INTEGER,
	lat REAL NOT NULL, lng REAL NOT NULL,
	datetime TEXT, ele REAL, distance REAL,
	FOREIGN KEY (trackid) REFERENCES tracks (id)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);