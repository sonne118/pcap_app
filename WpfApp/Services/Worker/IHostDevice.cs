namespace wpfapp.Services.Worker
{
    public interface IHostDevice
    {
        public Task StopAsync(CancellationToken cancellationToken);
        public Task StartAsync(CancellationToken cancellationToken);
        public Task SetUpDevice(int device, CancellationToken cancellationToken);
    }
}
