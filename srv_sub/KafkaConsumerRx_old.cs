
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Reactive.Linq;
using System.Text;

namespace srv_sub;

public sealed class KafkaConsumerRx_old : IKafkaConsumerRx_old
{

    private readonly IConfiguration? _configuration;
    ILogger<KafkaConsumerRx_old>? _logger;
    private readonly int _maxConsumeBatchSize = 100;
    public readonly string _topic = "SnapshotTopic";
    public Consumer<string, string> _consumer;
    private IDisposable _registration;
    private CancellationTokenSource cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));


    public KafkaConsumerRx_old(IConfiguration configuration,
                               ILogger<KafkaConsumerRx_old> logger
                              )
    {
        _configuration = configuration;
        _logger = logger;
    }


    public async Task StartConsume(CancellationToken cancellationToken)
    {
        var BootstrapServers = _configuration["KafkaServer"];
        var GroupId = _configuration["ConsumerGroup"];

        var config = new Dictionary<string, object>
            {
            { "bootstrap.servers", BootstrapServers },
            { "group.id", !string.IsNullOrEmpty(GroupId) ? GroupId : Guid.NewGuid().ToString() },
            { "socket.blocking.max.ms", 1 },
                    { "enable.auto.commit", false },
                    { "fetch.wait.max.ms", 5 },
                    { "fetch.error.backoff.ms", 5 },
                    { "fetch.message.max.bytes", 10240 },
                    { "queued.min.messages", 1000 },
                    {
                        "default.topic.config",
                        new Dictionary<string, object>
                            {
                                { "auto.offset.reset", "beginning" }
                            }
                    }
                };

        _consumer = new Consumer<string, string>(config, new StringDeserializer(Encoding.UTF8), new StringDeserializer(Encoding.UTF8));

        _consumer.OnLog += OnLog;
        _consumer.OnError += OnError;

        var observable = Consume(cancellationToken);

        var subscription = observable
                  .Buffer(1)
                  .Subscribe(
                      messages =>
                      {
                          foreach (var message in messages)
                          {
                              Console.WriteLine($" '{message.Value}'");
                              _consumer.CommitAsync(message);
                          }

                          CommitAsync(messages[messages.Count - 1]).GetAwaiter().GetResult();

                      });

        var taskCompletionSource = new TaskCompletionSource<object>();
        _registration = cancellationToken.Register(
            () =>
            {
                subscription.Dispose();
                taskCompletionSource.SetResult(null);
            });

        await taskCompletionSource.Task;
    }

    public async Task CommitAsync(Message<string, string> message) => await _consumer.CommitAsync(message);

    public IObservable<Message<string, string>> Consume(CancellationToken cancellationToken)
    {
        var observable = Observable.FromEventPattern<Message<string, string>>(
        x =>
        {
            _consumer.OnMessage += x;
            _consumer.Subscribe(_topic);
        },
                             x =>
                             {
                                 _consumer.Unsubscribe();
                                 _consumer.OnMessage -= x;
                             })
                         .Select(x => x.EventArgs);

        Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {

                        _consumer.Poll(TimeSpan.FromMilliseconds(1000));
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default)
            .ConfigureAwait(false);

        return observable;
    }

    private void OnLog(object sender, LogMessage logMessage)
           => _logger.LogInformation(
               "Consuming from Kafka. Client: '{client}', syslog level: '{logLevel}', message: '{logMessage}'.",
               logMessage.Name,
               logMessage.Level,
               logMessage.Message);

    private void OnError(object sender, Error error)
        => _logger.LogInformation("Consumer error: {error}. No action required.", error);

    private void OnConsumeError(object sender, Message message)
    {
        _logger.LogError(
            "Error consuming from Kafka. Topic/partition/offset: '{topic}/{partition}/{offset}'. Error: '{error}'.",
            message.Topic,
            message.Key,
            message.Payload,
            message.Created);
        throw new KafkaException(new Error(ErrorCode.BrokerNotAvailable));
    }

    public void Dispose()
    {
        if (_consumer != null)
        {
            _consumer.OnLog -= OnLog;
            _consumer.OnError -= OnError;
            _registration.Dispose();
            _consumer.Dispose();
        }
    }
}