﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace kafka;

public sealed class KafkaConsumer(
    IConfiguration configuration,
    ILogger<KafkaConsumer> logger,
    ISerializerer serializer,
    IEnumerable<IMessageHandler> messageHandlers,
    string topic) : BackgroundService
{
    private ConsumerConfig _consumerConfig = default!;
    private readonly int _maxConsumeBatchSize = 100;
    private readonly string _topic = "SnapshotTopic";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["KafkaServer"],
           // SecurityProtocol = SecurityProtocol.SaslSsl,
            //SaslMechanism = SaslMechanism.Plain,
            //SaslUsername = configuration["SaslUsername"],
            //SaslPassword = configuration["SaslPassword"],
            GroupId = configuration["ConsumerGroup"],
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false
        };

        for (var i = 0; i < 1; i++)
        {
            Task.Factory.StartNew(() => StartConsumer(stoppingToken),
                stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        return Task.CompletedTask;
    }

    private async Task StartConsumer(CancellationToken stoppingToken)
    {
        using (var consumer = new ConsumerBuilder<int, string>(_consumerConfig).Build())
        {
            consumer.Subscribe("SnapshotTopic");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        Console.WriteLine($"Consumed message '{consumeResult.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                        //if (!TryConsume(consumeResult, stoppingToken))
                        //{
                        //    await _retryQueueProducer.RetryAsync(consumeResult);
                        //}
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            finally
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                consumer.Close();
            }


            //using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
            //consumer.Subscribe(_topic);

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        var payloads = consumer.ConsumeBatch(TimeSpan.FromMinutes(1), _maxConsumeBatchSize, stoppingToken);

            //        if (payloads.Count == 0)
            //            continue;

            //        foreach (var payload in payloads)
            //        {
            //            var message = serializer.Deserialize<Message>(payload.Message.Value);

            //            foreach (var handler in messageHandlers)
            //            {
            //                await handler.HandleAsync(message, stoppingToken);
            //            }
            //        }

            //        consumer.Commit();
            //    }
            //    catch (OperationCanceledException)
            //    {
            //        break;
            //    }
            //    catch (ConsumeException e)
            //    {
            //        if (e.Error.IsFatal)
            //        {
            //            // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
            //            logger.LogCritical(e, "Fatal error consuming message");
            //            break;
            //        }
            //        else
            //        {
            //            logger.LogError(e, "Error consuming message");
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        logger.LogError(e, "Error consuming message");
            //    }
        }
    }
}