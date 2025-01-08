namespace srv_sub
{
    public interface IKafkaConsumerRx_old
    {
       public Task StartConsume(CancellationToken stoppingToken);
    }
}
