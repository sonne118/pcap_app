
DELIMITER //

CREATE PROCEDURE CheckAndUpdateOutbox1(IN MaxLimit INT, IN ReservationSeconds INT)
BEGIN
    DECLARE table_exists INT DEFAULT 0;

    SELECT COUNT(*)
    INTO table_exists
    FROM dbo.Outbox
    WHERE IsProcessed = 0 AND IsSequential = 0 AND (IsProcessing = 0 OR (IsProcessing = 1 AND NOW() >= ExpiredAt));

    IF table_exists > 0 THEN
        CREATE TEMPORARY TABLE forReservation AS
        SELECT * FROM Outbox
        WHERE IsProcessed = 0 AND IsSequential = 0 AND (IsProcessing = 0 OR (IsProcessing = 1 AND NOW() >= ExpiredAt))
        LIMIT MaxLimit;

        UPDATE dbo.Outbox
        JOIN forReservation ON Outbox.Id = forReservation.Id
        SET dbo.Outbox.IsProcessing = 1, 
            dbo.Outbox.ReservedAt = NOW(), 
            dbo.Outbox.ExpiredAt = NOW() + INTERVAL ReservationSeconds SECOND;

        DROP TEMPORARY TABLE forReservation;
    END IF;
END //

DELIMITER ;
CALL CheckAndUpdateOutbox1(10, 60);