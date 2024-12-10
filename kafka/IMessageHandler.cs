namespace Kafka
{
    public interface IMessageHandler
    {
        Task HandleAsync(Message message, CancellationToken cancellationToken);
    }
}