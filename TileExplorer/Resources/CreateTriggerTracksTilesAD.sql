CREATE TRIGGER tracks_tiles_ad
AFTER DELETE ON tracks_tiles
WHEN
	(SELECT COUNT(*) FROM tracks_tiles WHERE tileid=OLD.tileid) = 0
BEGIN
	DELETE FROM tiles WHERE id=OLD.tileid;
END;