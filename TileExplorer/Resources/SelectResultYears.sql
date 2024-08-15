SELECT (CASE WHEN year = 32000 THEN NULL ELSE year END) AS year, count,
    distancesum, durationsum,
    distancestep0, distancestep1, distancestep2
FROM
(SELECT 
    CAST(STRFTIME('%Y', datetimestart) AS INTEGER) AS year,
    COUNT(*) AS count,
    SUM(distance) / 1000.0 AS distancesum, 
	SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestart)) AS durationsum,
	SUM(IIF(distance < 50000, 1, 0)) AS distancestep0,
	SUM(IIF(distance >= 50000 AND distance < 100000, 1, 0)) AS distancestep1,
	SUM(IIF(distance >= 100000, 1, 0)) AS distancestep2
FROM
    tracks
GROUP BY year
	UNION
SELECT 
    32000 AS year,
    COUNT(*) AS count,
    SUM(distance) / 1000.0 AS distancesum,
	SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestart)) AS durationsum,
	SUM(IIF(distance < 50000, 1, 0)) AS distancestep0,
	SUM(IIF(distance >= 50000 AND distance < 100000, 1, 0)) AS distancestep1,
	SUM(IIF(distance >= 100000, 1, 0)) AS distancestep2
FROM
    tracks
ORDER BY year
);