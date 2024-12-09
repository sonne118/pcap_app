namespace Server.Services.BackgroundJobs
{
    public interface IBackgroundJobs<T>
    {
        AsyncConcurrencyQueue<T> BackgroundTaskSignalR { get; set; }
    }
}
