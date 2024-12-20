using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Pipes;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;

namespace wpfapp.Services.Worker
{
    public class Worker : BackgroundService
    {
        private readonly int timeout = 10000;
        private readonly ILogger<Worker> _logger;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger,
                      IStreamData streamData,
                      IBackgroundJobs<Snapshot> backgroundJobs,
                      IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _backgroundJobs = backgroundJobs;
            _scopeFactory = scopeFactory;
            streamData.GetStream(3);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Func<BinaryReader, Task<Snapshot>> result; Snapshot res;

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
                        result = pool.Get();
                        try
                        {
                            res = await result(stream);
                            _backgroundJobs.BackgroundTasks.Enqueue(res);
                            _backgroundJobs.BackgroundTaskGrpc.Enqueue(res);
                        }
                        finally
                        {
                            pool.Return(result);
                        }
                    }
                }
                pipe.Close();
                await Task.Delay(timeout, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await ExecuteAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            using (var scope = _scopeFactory.CreateScope())
            {
                var service_j = scope.ServiceProvider.GetRequiredService<IBackgroundJobs<Snapshot>>();
                service_j.CleanBackgroundTask();
            }
        }
    }
}
