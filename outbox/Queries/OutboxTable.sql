
CREATE TABLE dbo.Outbox(
    Id INT NOT NULL DEFAULT (UUID()),
    DateTimestamp DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    RawData TEXT NOT NULL,
    MessageType varchar(255) NOT NULL ,
    Topic VARCHAR(255) NOT NULL,
    PartitionBy VARCHAR(255) NULL,
    IsProcessed INT DEFAULT 0,
    IsSequential INT DEFAULT 0,
    Metadata TEXT NULL,
    ReservedAt DATETIME(6) NULL,
    ExpiredAt DATETIME(6) NULL,
    IsProcessing INT DEFAULT 0,
    PRIMARY KEY (Id)
);


