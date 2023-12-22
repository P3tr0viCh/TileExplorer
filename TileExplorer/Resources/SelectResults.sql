SELECT 
    CAST(STRFTIME('%Y', datetimestart) AS INTEGER) AS year,
    COUNT(*) AS count,
    SUM(distance) / 1000.0 AS distancesum
FROM
    tracks
GROUP BY year
ORDER BY year;