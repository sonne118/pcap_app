using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Text;

namespace kafka;

public class KafkaMessagePub : IKafkaMessagePub       
{
    private readonly ISerializer _serializer;
    private readonly ILogger<KafkaMessagePub> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly string _defaultKey = Guid.NewGuid().ToString();
    private readonly ITopicRepository _topicRepository;
    private readonly IConfiguration _configuration;
    private ProducerConfig _producerConfig;

    public KafkaMessagePub(IConfiguration configuration,
                           ISerializer serializer,
                           ILogger<KafkaMessagePub> logger,
                           ITopicRepository topicRepository)
    {
        _configuration = configuration;
        _topicRepository = topicRepository;
        _serializer = serializer;
        _logger = logger;
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = _configuration["KafkaServer"],

            //SecurityProtocol = SecurityProtocol.SaslSsl,
            //SaslMechanism = SaslMechanism.Plain,

            //SaslUsername = configuration["SaslUsername"],
            //SaslPassword = configuration["SaslPassword"],

            LingerMs = 200,
            BatchSize = 10 * 1024,
            MessageTimeoutMs = 10000,

            // Enable receiving delivery reports
            EnableDeliveryReports = true,

            // Receive acknowledgement from all sync replicas
            //Acks = Acks.All,

            // Number of times to retry before giving up
            MessageSendMaxRetries = 3,
            // Duration to retry before next attempt
            RetryBackoffMs = 1000,

            // Set to true if you don't want to reorder messages on retry
            //EnableIdempotence = true
        };

        _producer = new ProducerBuilder<string, string>(_producerConfig)
            .Build();
        //BeginProduction();
    }


    private async Task BeginProduction()
    {
        // Try and create the primary topic (for first time requests)
        await _topicRepository.TryCreateTopic(_configuration["Topic"]);

        // Try and create the delay topic (for failed requests)
        await _topicRepository.TryCreateTopic(_configuration["RetryTopic"]);

        // Create the deadletter topic (for failed retry requests)
        await _topicRepository.TryCreateTopic(_configuration["DeadletterTopic"]);
    }

    public  Task SendAsync(ImmutableArray<Message> messages, CancellationToken cancellationToken)
    {
        foreach (var message in messages)
        {
            try
            {
                var kafkaPayload = _serializer.Serialize(message);

                _producer.Produce(
                    message.Topic,
                    new Message<string, string> { Key = message.Key ?? _defaultKey, Value = kafkaPayload, Headers = PrepareHeaders(message.Metadata, message.Created, message.PayloadType) }, report =>
                    {
                        if (report.Status != PersistenceStatus.Persisted)
                            //throw new Exception($"Failed to send message to Kafka, Id: {message.id}, Topic: {report.Error.Code}");
                            _logger.LogError("Failed kafka message producing with Key {Key}, Error: {error}", report.Message.Key, report.Error.Code);
                    });

                 _logger.LogInformation("Message sent to Kafka, Id: {Id}, Topic: {Topic}", message.Key, message.Topic);
            }
            catch (ProduceException<Null, string> ex)
            {
                throw new Exception($"Failed to send message to Kafka, Id: {message.Key}, Topic: {message.Topic}", ex);
            }
        }

        _producer.Flush(cancellationToken);
        return Task.CompletedTask;

    }

    private Headers PrepareHeaders(Dictionary<string, string>? metadata, DateTimeOffset messageTimestamp, string messageType)
    {
        var headers = new Headers
        {
            { "timestamp", BitConverter.GetBytes(messageTimestamp.ToUnixTimeMilliseconds()) }, { "type", Encoding.UTF8.GetBytes(messageType) }
        };

        if (metadata is null)
            return headers;

        foreach (var (key, value) in metadata)
            headers.Add(key, Encoding.UTF8.GetBytes(value));

        return headers;
    }
}