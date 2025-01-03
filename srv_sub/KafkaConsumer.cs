﻿using Confluent.Kafka;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Reactive.Linq;

namespace srv_sub;

public sealed class KafkaConsumer(
    IConfiguration configuration,
    ILogger<KafkaConsumer> logger,
    ISerializer serializer,
    IEnumerable<IMessageHandler> messageHandlers,
    IMessageHandler handler,
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
       
         var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
        //{

            consumer.Subscribe(_topic);
            while (!stoppingToken.IsCancellationRequested)
            //var observable = Observable.Create<ConsumeResult<string, string>>(observer =>
            {

                ConsumeResult<string, string> consumeResult = default;
                while (!stoppingToken.IsCancellationRequested)
                {
                  // var consumeResult = consumer.Consume(); 

                    try
                    {
                        var payloads = consumer.ConsumeBatch(TimeSpan.FromMinutes(1) , _maxConsumeBatchSize, stoppingToken);

                        if (payloads.Count == 0)
                            continue;

                        foreach (var payload in payloads)
                        {
                            var message = serializer.Deserialize<Message>(payload.Message.Value);
                            handler.HandleAsync(message, stoppingToken);
                        }

                        consumer.Commit();
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ConsumeException e)
                    {
                        if (e.Error.IsFatal)
                        {
                            logger.LogCritical(e, "Fatal error consuming message");
                            break;
                        }
                        else
                        {
                            logger.LogError(e, "Error consuming message");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error consuming message");
                    }

                    //observer.OnNext(consumeResult);
                }
               // return () => { };
            }

        //observable.Subscribe(message => {
        //    Console.WriteLine("Received message:"); 
        //    Console.WriteLine($"Topic: {message.Topic}");
        //    //Console.WriteLine($"Payload: {message.Payload}");


        //     });
        }
    
}