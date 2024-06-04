using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Reflection.Metadata.Ecma335;

namespace worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
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
                    pipe.Connect(100000);
                    pipe.ReadMode = PipeTransmissionMode.Byte;
                  
                    Func<BinaryReader,Task<Snapshot>> func = (value) => Task.Run(() => ReadClass.ReadMessage(value));

                    var pool = new ObjectPool<BinaryReader,Task<Snapshot>>(() =>
                    {
                        return func;                        
                    });

                    pool.Return(func);// first object in poll
                    pool.Return(func);// second object in poll in any case
                    while (pipe.IsConnected)
                    {
                        var result = pool.Get();
                        try
                        {
                            res =  await result(stream);
                            Console.WriteLine(res);
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