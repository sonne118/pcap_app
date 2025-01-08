using srv_sub;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Services.AddSingleton<IKafkaConsumerRx_old, KafkaConsumerRx_old>();

builder.Services.AddSingleton<IHostedService>(sp =>
             new KafkaConsumerEx(
                 sp.GetRequiredService<IKafkaConsumerRx_old>(),
                 sp.GetRequiredService<IConfiguration>(),
                 sp.GetRequiredService<ILogger<KafkaConsumerRx_old>>(),
                 "SnapshotTopic"));
var app = builder.Build();

app.Run();

