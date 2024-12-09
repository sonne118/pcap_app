using System.Collections.Concurrent;
using wpfapp.Services.BackgroundJobs;
using WpfApp.Model;

namespace wpfapp.Services.BackgroundJob
{
    public interface IBackgroundJobs<T>
    {
        ConcurrentQueue<T> BackgroundTasks { get; set; }
        AsyncConcurrencyQueue<T> BackgroundTaskGrpc {  get; set; }
        public void CleanBackgroundTask() {

            //BackgroundTasks.Clear();
        }
    }
}
