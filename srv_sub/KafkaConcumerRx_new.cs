//using Confluent.Kafka;
//using System.Reactive.Linq;

//namespace srv_sub;

//public sealed class KafkaConsumer(
//    IConfiguration configuration,
//    ILogger<KafkaConsumerRx_old> logger,
//    ISerializer serializer,
//    IEnumerable<IMessageHandler> messageHandlers,
//    IMessageHandler handler,
//    string topic) : BackgroundService
//{
//    public ConsumerConfig _consumerConfig = default!;
//    private readonly int _maxConsumeBatchSize = 100;
//    public readonly string _topic = "SnapshotTopic";
//    private IEnumerable<string> _topics;
//    public Consumer<string, string> consumer;
//    private CancellationTokenSource cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));



//    protected override Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        var BootstrapServers = configuration["KafkaServer"];
//        var GroupId = configuration["ConsumerGroup"];

//        var AutoOffsetReset = AutoOffsetReset.Latest;
//        var EnableAutoCommit = false;

//        var _consumerConfig = new ConsumerConfig
//        {
//            BootstrapServers = configuration["KafkaServer"],
//            GroupId = configuration["ConsumerGroup"],
//            AutoOffsetReset = AutoOffsetReset.Latest,
//            EnableAutoCommit = false,
//        };
//        consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

//        var observable = Start(stoppingToken);

//        observable.Subscribe(
//            consumeResult =>
//            {

//                var message = serializer.Deserialize<Message>(consumeResult.Value);
//                handler.HandleAsync(message, stoppingToken);

//            },
//            error => Console.WriteLine($"Error: {error.Message}"),
//            () => Console.WriteLine("Completed")
//        );

//        return Task.CompletedTask;
//    }


//    public IObservable<Message<string, string>> Start(CancellationToken stoppingToken)
//    {
//        consumer.Subscribe(_topic);
//        return Observable.Create<Message<string, string>>(observer =>
//         {
//             //ConsumeResult<string, string> consumeResult = default;

//             while (true)
//             {
//                 try
//                 {
//                     //var payloads = consumer.ConsumeBatch(out consumeResult, TimeSpan.FromMinutes(1), _maxConsumeBatchSize, stoppingToken);

//                     var message1 = consumer.Consume();

//                     observer.OnNext(message1.Message);


//                     consumer.Commit();
//                 }
//                 catch (OperationCanceledException)
//                 {
//                     break;
//                 }
//                 catch (ConsumeException e)
//                 {
//                     if (e.Error.IsFatal)
//                     {
//                         logger.LogCritical(e, "Fatal error consuming message");
//                         break;
//                     }
//                     else
//                     {
//                         logger.LogError(e, "Error consuming message");
//                         observer.OnError(e);
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     logger.LogError(e, "Error consuming message");
//                 }
//             }
//             //return () => { };
//             return () => { consumer.Unsubscribe(); consumer.Close(); };
//         });
//    }
//}