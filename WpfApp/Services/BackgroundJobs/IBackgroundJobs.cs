using System.Collections.Concurrent;

namespace WpfApp.Services.BackgroundJob
{
    public interface IBackgroundJobs<T>
    {
        ConcurrentQueue<T> BackgroundTasks { get; set; }
    }
}
