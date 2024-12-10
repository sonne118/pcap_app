using System.Collections.Immutable;

namespace Kafka.Internal;

public interface IKafkaMessagePub
{
    Task SendAsync(ImmutableArray<Message> messages, CancellationToken cancellationToken);
}