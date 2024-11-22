SELECT tiles.id, x, y, COUNT(*) AS trackcount FROM tiles
LEFT JOIN tracks_tiles ON tiles.id = tileid
GROUP BY tiles.id
ORDER BY x, y;