using System.Collections.Concurrent;
using wpfapp.models;
using wpfapp.Services.BackgroundJobs;

namespace wpfapp.Services.BackgroundJob
{
    public class BackgroundJobs : IBackgroundJobs<Snapshot>
    {
        public ConcurrentQueue<Snapshot> BackgroundTasks { get; set; } = new();
        public AsyncConcurrencyQueue<Snapshot> BackgroundTaskGrpc { get; set; } = new();

    }
}
