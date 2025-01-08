
namespace srv_sub
{
    public class KafkaConsumerEx : BackgroundService
    {
        private readonly IKafkaConsumerRx_old? _kafkaConsumerRx;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumerRx_old> _logger;
        private readonly string _topic;
        public KafkaConsumerEx(IKafkaConsumerRx_old kafkaConsumerRx,
                               IConfiguration configuration,
                               ILogger<KafkaConsumerRx_old> logger,
                               string topic
            )
        {
            _kafkaConsumerRx = kafkaConsumerRx;
            _configuration = configuration;
            _logger = logger;
            _topic = topic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _kafkaConsumerRx.StartConsume(stoppingToken);
            await Task.Delay(1000);
        }
    }
}
