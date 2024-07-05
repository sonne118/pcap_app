using System.Threading;
using System.Threading.Tasks;
using wpfapp.Services.Worker;

namespace WpfApp.Services.Worker
{
    public class StartService : IHostDevice
    {

        private static EventWaitHandle _eventWaitHandle;     
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

        public Task SetUpDevice(int device, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
