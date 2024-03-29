SELECT (CASE WHEN year = 32000 THEN NULL ELSE year END) AS year, count, distancesum, durationsum
FROM
(SELECT 
    CAST(STRFTIME('%Y', datetimestart) AS INTEGER) AS year,
    COUNT(*) AS count,
    SUM(distance) / 1000.0 AS distancesum, 
	SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestart)) as durationsum
FROM
    tracks
GROUP BY year
	UNION
SELECT 
    32000 AS year,
    COUNT(*) AS count,
    SUM(distance) / 1000.0 AS distancesum,
	SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestart)) as durationsum
FROM
    tracks
ORDER BY year
);