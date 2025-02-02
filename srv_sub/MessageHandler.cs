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
            var CreatedEvent = _serializer.Deserialize<SnapshotMessage>(message.Payload);
            _logger.LogInformation("Received dest_ip {Id} and {proto} ", CreatedEvent.dest_ip, CreatedEvent.proto);

            return Task.CompletedTask;
        }
    }
}