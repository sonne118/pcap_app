
CREATE PROCEDURE `GetDataFromTempTable`(IN MaxLimit INT, IN ReservationSeconds INT)
BEGIN
    CREATE TEMPORARY TABLE TempOutbox AS
    SELECT * FROM Outbox 
    WHERE  IsProcessed = 0 AND IsSequential = 0 AND (IsProcessing = 0 OR (IsProcessing = 1 AND NOW() >= ExpiredAt));
 
        UPDATE dbo.Outbox as o
        JOIN TempOutbox as t ON o.Id = t.Id
        SET o.IsProcessing = 1, 
            o.ReservedAt = NOW(), 
            o.ExpiredAt = NOW() + INTERVAL ReservationSeconds SECOND;

    SELECT * FROM TempOutbox;
    DROP TEMPORARY TABLE TempOutbox;
END


