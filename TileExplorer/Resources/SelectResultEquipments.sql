SELECT text, count, distancesum FROM
(
	SELECT equipmentid, COUNT(*) AS count, SUM(distance) / 1000 AS distancesum, CASE WHEN equipmentid = 0 THEN 2 ELSE 1 END AS type
	FROM tracks GROUP BY equipmentid
)
LEFT JOIN equipments
ON equipments.id = equipmentid
ORDER BY type, text;