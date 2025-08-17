using srv_sub;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);


builder.Services.AddSingleton<ISerializer, Serializer>();
builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
builder.Services.AddSingleton<ILogger<KafkaConsumerRx>, Logger<KafkaConsumerRx>>();
builder.Services.AddSingleton<ISerializer, Serializer>();
builder.Services.AddSingleton<ExTcpClient>();


builder.Services.AddSingleton<IHostedService, KafkaConsumerRx>();
builder.Services.AddHostedService(s => s.GetRequiredService<KafkaConsumerRx>());
//builder.Services.AddSingleton<IKafkaConsumerRx,KafkaConsumerRx>();

//builder.Services.AddSingleton<IHostedService>(sp =>
//             new KafkaConsumerEx(
//                 sp.GetRequiredService<IKafkaConsumerRx>()
//            ));


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5005); // HTTP
    // Uncomment and configure below for HTTPS
    // options.ListenAnyIP(5001, listenOptions =>
    // {
    //     listenOptions.UseHttps("path/to/cert.pfx", "certPassword");
    // });
});
var app = builder.Build();

app.Run();



