SELECT tracks.text, datetimestart,
	eleascent, eledescent,
	equipmentid,
	equipments.text AS equipmenttext
FROM tracks
LEFT JOIN equipments ON equipmentid = equipments.id
ORDER BY datetimestart DESC;