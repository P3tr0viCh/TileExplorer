CREATE TABLE tracks_tiles (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	trackid INTEGER, tileid INTEGER,
	FOREIGN KEY (trackid) REFERENCES tracks (id)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);