namespace outbox;

public interface IOutboxInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}