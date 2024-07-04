using System.Collections.Concurrent;
using WpfApp.Model;

namespace WpfApp.Services.BackgroundJob
{
    public class BackgroundJobs : IBackgroundJobs<Snapshot>
    {
        public ConcurrentQueue<Snapshot> BackgroundTasks { get; set; } = new();
    }
}
