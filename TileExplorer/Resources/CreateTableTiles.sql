CREATE TABLE tiles (
	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	x INTEGER NOT NULL, y INTEGER NOT NULL,
	UNIQUE(x, y)
);