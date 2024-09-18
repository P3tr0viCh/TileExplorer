CREATE TABLE markers (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    lat REAL NOT NULL,
    lng REAL NOT NULL,
    text TEXT,
    istextvisible INTEGER,
    offsetx INTEGER,
    offsety INTEGER
);