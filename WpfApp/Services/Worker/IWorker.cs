using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Services.Worker
{
    public interface IWorker
    {
        public Task ExecuteAsync(CancellationToken token = default);
        public Task StopAsync(CancellationToken token = default);
    }
}
