SELECT * FROM tags
WHERE id IN (
	SELECT tagid FROM tracks_tags
	WHERE trackid = :trackid
)
ORDER BY text;