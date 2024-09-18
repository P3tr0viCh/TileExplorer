CREATE TRIGGER equipments_ad
AFTER DELETE ON equipments
BEGIN
	UPDATE tracks SET equipmentid=0 WHERE equipmentid=OLD.id;
END;