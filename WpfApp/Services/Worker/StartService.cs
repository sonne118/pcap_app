using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using wpfapp.Services.Worker;

namespace WpfApp.Services.Worker
{
    public class StartService : IHostDevice
    {

        [DllImport("sniffer_packages.dll")]
        extern static void fnCPPDLL(int dev);

        private static EventWaitHandle _eventWaitHandle;
        private static Thread _workerThread;
        static StartService()
        {
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, @"Global\sniffer");
        }

        private static void StartThread(int dev)
        {
            _workerThread = new Thread(() => fnCPPDLL(dev));
            _workerThread.Start();
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
            StartThread(device);
            return Task.CompletedTask;
        }

    }
}
