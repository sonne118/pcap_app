using System.IO.Pipes;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);        

                await using var pipe = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut);
                using (BinaryReader stream = new BinaryReader(pipe))
                {
                    pipe.Connect(100000);
                    pipe.ReadMode = PipeTransmissionMode.Byte;                   

                    while (pipe.IsConnected)
                    {
                        var result = await Task.Run(() =>
                        {
                            return ReadClass.ReadMessage(stream);
                        });
                        Console.WriteLine(result);
                    }
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }

}