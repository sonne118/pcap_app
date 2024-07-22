using Microsoft.AspNetCore.SignalR;
using Server.Services.BackgroundJobs;
using Server.Model;
using System.Runtime.CompilerServices;

namespace Server.Services
{
    public class DataHub : Hub
    {
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        public DataHub(IBackgroundJobs<Snapshot> backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
        }
        public async IAsyncEnumerable<Snapshot> Stream([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var item in _backgroundJobs.BackgroundTaskSignalR)
            {
                yield return item;
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
