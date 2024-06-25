using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        [DllImport("sniffer_packages.dll")]
        extern static void fnCPPDLL(int flag);

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        static Worker()
        {
            ThreadPool.QueueUserWorkItem((_) => fnCPPDLL(1));
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

                    pool.Return(func);// first Task in poll
                    pool.Return(func);// second Task in poll 
                    pool.Return(func); //third Task in poll for any case ... improving scalability
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
