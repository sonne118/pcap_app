
CREATE PROCEDURE `dbo.GetDataFromTempTable`(IN MaxLimit INT, IN ReservationSeconds INT)
BEGIN
    /*
      Concurrency-safe reservation using row locks to avoid deadlocks:
      1) Select candidate Ids with FOR UPDATE SKIP LOCKED and LIMIT.
      2) Mark those rows as processing and set reservation/expiry.
      3) Return the selected rows.
    */

    CREATE TEMPORARY TABLE TempOutboxIds (Id CHAR(36) PRIMARY KEY);

    /* Lock only the chosen rows and skip ones locked by other workers */
    INSERT INTO TempOutboxIds (Id)
    SELECT o.Id
    FROM dbo.Outbox o
    WHERE o.IsProcessed = 0
      AND o.IsSequential = 0
      AND (o.IsProcessing = 0 OR (o.IsProcessing = 1 AND NOW() >= o.ExpiredAt))
    ORDER BY o.DateTimestamp
    LIMIT MaxLimit
    FOR UPDATE SKIP LOCKED;

    /* Reserve selected rows */
    UPDATE dbo.Outbox o
    JOIN TempOutboxIds t ON o.Id = t.Id
    SET o.IsProcessing = 1,
        o.ReservedAt = NOW(),
        o.ExpiredAt = NOW() + INTERVAL ReservationSeconds SECOND;

    /* Return full rows for processing */
    SELECT o.*
    FROM dbo.Outbox o
    JOIN TempOutboxIds t ON o.Id = t.Id;

    DROP TEMPORARY TABLE TempOutboxIds;
END


