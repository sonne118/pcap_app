using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace WpfApp.Services.Worker
{
    public class Worker : BackgroundService
    {
        readonly private int timeout;
        readonly private string path;
        private readonly ILogger<Worker> _logger;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        public static CancellationToken stoppingToken;

        public Worker(ILogger<Worker> logger, IStreamData streamData, IBackgroundJobs<Snapshot> backgroundJobs)
        {
            _logger = logger;
            _backgroundJobs = backgroundJobs;
            streamData.GetStream(3);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            BinaryReader value = default; Snapshot res = default;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await using var pipe = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut);
                using (BinaryReader stream = new BinaryReader(pipe))
                {
                    if (!pipe.IsConnected)
                    {
                        await pipe.ConnectAsync(stoppingToken).ConfigureAwait(false);
                        pipe.ReadMode = PipeTransmissionMode.Byte;
                    }

                    Func<BinaryReader, Task<Snapshot>> func = (value) => Task.Run(() => ReadClass.ReadMessage(value));

                    var pool = new ObjectPool<BinaryReader, Task<Snapshot>>(() =>
                    {
                        return func;
                    });

                    pool.Return(func);// first Task in poll
                    pool.Return(func);// second Task in poll 
                    pool.Return(func); //third Task in poll for any case ... improving scalability
                    while (pipe.IsConnected)
                    {
                        var result = pool.Get();
                        try
                        {
                            res = await result(stream);
                            _backgroundJobs.BackgroundTasks.Enqueue(res);
                        }
                        finally
                        {
                            pool.Return(result);
                        }
                    }
                }
                pipe.Close();
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
