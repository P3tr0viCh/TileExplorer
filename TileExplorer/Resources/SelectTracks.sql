SELECT id, text, dt AS datetimestart, datetimefinish, distance,
	equipmentid, equipmenttext, equipmentbrand, equipmentmodel,
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
		SELECT tracks.id AS id, tracks.text AS text,
			datetimestart AS dt, datetimefinish,
			distance, tileid AS tid,
			equipmentid,
			equipments.text AS equipmenttext,
			equipments.brand AS equipmentbrand,
			equipments.model AS equipmentmodel
		FROM tracks
			LEFT JOIN tracks_tiles ON tracks.id = tracks_tiles.trackid
			LEFT JOIN equipments ON equipmentid = equipments.id
		{0}
	)
)
GROUP BY id
ORDER BY {1};