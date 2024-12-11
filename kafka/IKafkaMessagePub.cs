using System.Collections.Immutable;

namespace kafka;

public interface IKafkaMessagePub
{
    Task SendAsync(ImmutableArray<Message> messages, CancellationToken cancellationToken);
}