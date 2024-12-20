namespace WpfApp.Services.Worker
{
    public interface IWorker
    {
        public Task ExecuteAsync(CancellationToken token = default);
        public Task StopAsync(CancellationToken token = default);
    }
}
