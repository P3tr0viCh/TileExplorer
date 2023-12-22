SELECT * FROM tiles
WHERE id IN (
	SELECT tileid FROM tracks_tiles
	WHERE trackid = :trackid
);