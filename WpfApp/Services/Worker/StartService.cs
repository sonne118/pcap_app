using Microsoft.Extensions.Hosting;

namespace wpfapp.Services.Worker
{
    public class StartService : IHostedService
    {
        readonly private int timeout = 1;
        private static EventWaitHandle _eventWaitHandle;
        static StartService()
        {
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, @"Global\sniffer");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _eventWaitHandle.Reset();
            await Task.Delay(timeout, cancellationToken);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _eventWaitHandle.Set();
            await Task.Delay(timeout, cancellationToken);
        }

        public async Task SetUpDevice(int device, CancellationToken cancellationToken)
        {
            await Task.Delay(timeout, cancellationToken);
        }

    }
}
