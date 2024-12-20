﻿using System.Collections.Immutable;

namespace outbox;

public interface IOutbox
{
    Task AddAsync<T>(T data, string topic, Func<T, string>? partitionBy, bool isSequential, Dictionary<string, string>? metadata, CancellationToken cancellationToken)
        where T : class;
    Task<ImmutableArray<OutboxRecord>> ReserveAsync(int top, TimeSpan reservationTimeout, CancellationToken cancellationToken);
    Task MarkAsProcessedAsync(ImmutableArray<OutboxRecord> data, CancellationToken cancellationToken);
    Task DeleteProcessedAsync(CancellationToken cancellationToken);
}