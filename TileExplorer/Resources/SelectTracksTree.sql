SELECT CAST(STRFTIME('%Y', datetimestart) AS INTEGER) AS year, CAST(STRFTIME('%m', datetimestart) AS INTEGER) AS month
FROM tracks
GROUP by year, month;