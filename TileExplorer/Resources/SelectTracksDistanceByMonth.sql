SELECT
	CAST(STRFTIME('%Y', datetimestart) AS INTEGER) AS year,
	CAST(STRFTIME('%m', datetimestart) AS INTEGER) AS month,
	CAST(STRFTIME('%d', datetimestart) AS INTEGER) AS day,
	SUM(distance) AS distance
FROM tracks
WHERE year = :year AND month = :month 
GROUP by year, month, day
ORDER BY year, month, day;