using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp.Services.Worker
{
    public class StartService : IHostedService
    {
        static EventWaitHandle _eventWaitHandle;
        static StartService()
        {
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, @"Global\sniffer");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventWaitHandle.Reset();
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventWaitHandle.Set();
            return Task.CompletedTask;
        }

    }
}
