SELECT CAST(STRFTIME('%Y', datetimestart) AS INTEGER) AS year
FROM tracks
GROUP by year
ORDER BY year;