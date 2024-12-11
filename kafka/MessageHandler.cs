using Serialization;
using Microsoft.Extensions.Logging;

namespace kafka
{
    internal sealed class MessageHandler : IMessageHandler
    {
        private readonly ILogger<MessageHandler> _logger;
        private readonly ISerializer _serializer;

        public MessageHandler(ILogger<MessageHandler> logger, ISerializer serializer)
        {
            _logger = logger;
            _serializer = serializer;
        }

        public Task HandleAsync(Message message, CancellationToken cancellationToken)
        {
            var type = Type.GetType(message.PayloadType);

            if (type == typeof(Snapshot))
            {
                var CreatedEvent = _serializer.Deserialize<Snapshot>(message.Payload);
                _logger.LogInformation("Received dest_ip created event with ID {Id}", CreatedEvent.dest_ip);
            }

            return Task.CompletedTask;
        }
    }
}