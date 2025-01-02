using Microsoft.Extensions.Logging;

namespace srv_sub
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
            //var type = Type.GetType(message.Payload);

            //if (type == typeof(SnapshotMessage))
            {
                var CreatedEvent = _serializer.Deserialize<SnapshotMessage>(message.Payload);
                _logger.LogInformation("Received dest_ip created event with ID {Id}", CreatedEvent.dest_ip);
            }

            return Task.CompletedTask;
        }
    }
}