using Server.Model;

namespace Server.Services.BackgroundJobs
{
    public class BackgroundJobs : IBackgroundJobs<Snapshot>
    {
        public AsyncConcurrencyQueue<Snapshot> BackgroundTaskSignalR { get; set; } = new();

    }
}
