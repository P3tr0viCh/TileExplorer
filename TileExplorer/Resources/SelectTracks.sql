SELECT id, text, dt AS datetimestart, datetimefinish, 
	distance, equipmentid,
	SUM(CASE WHEN e = 0 THEN 1 ELSE 0 END) AS newtilescount
FROM (
	SELECT *,
		EXISTS(
			SELECT tileid, datetimestart
			FROM tracks 
				LEFT JOIN tracks_tiles ON tracks.id = tracks_tiles.trackid
			WHERE tileid = tid AND datetimestart < dt
		) AS e
	FROM (
		SELECT tracks.id AS id, text,
			datetimestart AS dt, datetimefinish,
			distance, equipmentid, tileid AS tid
		FROM tracks
			LEFT JOIN tracks_tiles ON tracks.id = tracks_tiles.trackid
		{0}
	)
)
GROUP BY id
ORDER BY dt;