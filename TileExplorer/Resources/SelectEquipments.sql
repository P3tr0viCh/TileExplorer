SELECT * FROM equipments
LEFT JOIN (
	SELECT equipmentid, COUNT(*) AS count, SUM(distance) / 1000 AS distancesum
	FROM tracks GROUP BY equipmentid
) 
WHERE equipments.id = equipmentid
ORDER BY text;