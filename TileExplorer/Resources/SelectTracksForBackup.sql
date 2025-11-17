SELECT tracks.text, datetimestart, eleascent, eledescent, equipments.text AS equipmenttext
FROM tracks
LEFT JOIN equipments ON equipmentid = equipments.id
ORDER BY datetimestart DESC;