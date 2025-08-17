using Confluent.Kafka;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;
using System.IO;

namespace srv_sub
{
    public delegate Task MessageReceivedHandler(object sender, string message);

    public sealed class KafkaConsumerRx : BackgroundService, IDisposable
    {
        private readonly int _maxConsumeBatchSize = 100;
        public readonly string _topic = "SnapshotTopic";
        private readonly ConsumerConfig _consumerConfig;
        private readonly ISerializer _serializer;
        private readonly IMessageHandler _handler;
        private readonly ILogger<KafkaConsumerRx> _logger;
        private readonly ExTcpClient _tcpClient;
        private readonly IConsumer<string, string> _consumer;
        private readonly bool _checkApiKey;

        public KafkaConsumerRx(IConfiguration configuration,
                             ILogger<KafkaConsumerRx> logger,
                             ISerializer serializer,
                             IMessageHandler handler,
                             ExTcpClient tcpClient)
        {
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["KafkaServer"],
                GroupId = configuration["ConsumerGroup"],
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
            };

            _serializer = serializer;
            _handler = handler;
            _logger = logger;
            _tcpClient = tcpClient;
            _consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
            _checkApiKey = CheckApiKey();
        }

        private bool CheckApiKey()
        {
            return _tcpClient.Connect("localhost", 8000).GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var observable = StartKafkaObservable(stoppingToken);

            observable.Subscribe(
                async message =>
                {
                    Console.WriteLine($"Topic: {message.Topic}");

                    var processed = await ProcessMessageWithTcpClient(message, stoppingToken);

                    if (processed)
                    {
                        _consumer.Commit();
                    }
                },
                ex => Console.WriteLine($"Error: {ex.Message}"),
                () => Console.WriteLine("Completed.")
            );

            await Task.Delay(1000);
        }

        public IObservable<ConsumeResult<string, string>> StartKafkaObservable(CancellationToken stoppingToken)
        {
            return Observable.Create<ConsumeResult<string, string>>(observer =>
            {
                _consumer.Subscribe(_topic);

                var task = Task.Run(async () =>
                {
                    try
                    {
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                var consumeResults = _consumer.ConsumeBatch(out var consumeResult, TimeSpan.FromMinutes(1), _maxConsumeBatchSize, stoppingToken);

                                if (consumeResults.Count == 0)
                                    continue;

                                foreach (var payload in consumeResults)
                                {
                                    var message = _serializer.Deserialize<Message>(payload.Message.Value);
                                    await _handler.HandleAsync(message, stoppingToken);
                                }

                                observer.OnNext(consumeResults.Last());
                            }
                            catch (ConsumeException e)
                            {
                                if (e.Error.IsFatal)
                                {
                                    _logger.LogCritical(e, "Fatal error consuming message");
                                    observer.OnError(e);
                                    break;
                                }
                                else
                                {
                                    _logger.LogError(e, "Error consuming message");
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, "Error consuming message");
                                observer.OnError(e);
                            }
                        }

                        observer.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                    }
                    finally
                    {
                        _consumer.Close();
                    }
                }, stoppingToken);

                return () =>
                {
                    _consumer.Close();
                };
            });
        }

        private async Task<bool> ProcessMessageWithTcpClient(ConsumeResult<string, string> message, CancellationToken stoppingToken)
        {
            try
            {
                if (_checkApiKey)
                {
                    var json = JObject.Parse(message.Message.Value);
                    var payload = json["Payload"]?.ToString();

                    if (string.IsNullOrWhiteSpace(payload))
                    {
                        _logger.LogWarning("Received message with empty Payload. Skipping.");
                        return false;
                    }

                    if (stoppingToken.IsCancellationRequested)
                    {
                        return false;
                    }
                    var stream = _tcpClient?.Client?.GetStream();


                    var messageBytes = Encoding.UTF8.GetBytes(payload + "\n");
                    

                    if (stream == null || !stream.CanWrite)
                    {
                                _logger.LogError("TCP client is not connected and reconnection failed.");
                                return false;
                    }
                    int bytesRead = 0;
                    string response = string.Empty;
                    var buffer = new byte[1024];
                   
                    try
                    {
                        await stream.WriteAsync(messageBytes, 0, messageBytes.Length, stoppingToken);
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
                         response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }
                    catch (OperationCanceledException)
                    {
                        return GetTrueOrFalse(response);
                    }
                    catch (IOException ioEx)
                    {
                        _logger.LogWarning(ioEx, "IO error while writing to TCP stream. Will attempt one reconnect and retry.");
                        return GetTrueOrFalse(response);
                    }
                    catch (ObjectDisposedException odEx)
                    {
                        _logger.LogWarning(odEx, "TCP stream disposed. Will attempt one reconnect and retry.");
                        return GetTrueOrFalse(response);
                    }
                    catch (SocketException sockEx)
                    {
                        _logger.LogWarning(sockEx, "Socket error while writing. Will attempt one reconnect and retry.");
                        return GetTrueOrFalse(response);
                    }

                   return GetTrueOrFalse(response);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message with TCP client");
                return false;
            }
        }

        private bool GetTrueOrFalse( string response)
        {
            if (response.Contains("Ok"))
            {
                Console.WriteLine($"Server response: {response}");
                return true;
            }
            else
            {
                Console.WriteLine($"Server response: {response}");
                return false;
            }
        }


        void IDisposable.Dispose()
        {
            _tcpClient.Disconnect();
        }
    }
}





