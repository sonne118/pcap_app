using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using wpfapp.Services.BackgroundJobs;
using wpfapp.models;

namespace wpfapp.Services.BackgroundJob
{
    public class BackgroundJobs : IBackgroundJobs<Snapshot>
    {
        public ConcurrentQueue<Snapshot> BackgroundTasks { get; set; } = new();
        public AsyncConcurrencyQueue<Snapshot> BackgroundTaskGrpc { get; set; } = new();

        public BackgroundJobs()
        {
            //Task.Run(() => {
            //    for (int i = 0; i < 100000; i++)
            //    {
            //        BackgroundTaskGrpc.Enqueue(i);
            //        Thread.Sleep(5000);
            //    }
            //});
        }      
    }
}
