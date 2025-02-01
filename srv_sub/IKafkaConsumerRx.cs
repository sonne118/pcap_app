namespace srv_sub
{
    public interface IKafkaConsumerRx
    {
        public Task StartConsume(CancellationToken stoppingToken);
    }
}
