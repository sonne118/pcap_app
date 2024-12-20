namespace wpfapp.IPC.Grpc
{
    public interface IHostedGrpcService
    {
        public Task StartAsync(CancellationToken token = default);
        public Task StopAsync(CancellationToken token = default);
    }
}
