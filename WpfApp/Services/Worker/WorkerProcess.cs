using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp.Services.Worker
{
    public class WorkerProcess : BackgroundService
    {
        [DllImport("sniffer_packages.dll")]
        extern static void fnCPPDLL();
        readonly private int timeout;
        readonly private string path;
        public WorkerProcess()
        {
            timeout = 10000;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ThreadPool.QueueUserWorkItem((_) => fnCPPDLL());
                await Task.Delay(timeout, stoppingToken);
            }
        }
    }

}
