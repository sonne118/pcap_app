using srv_sub;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);


builder.Services.AddSingleton<ISerializer, Serializer>();
builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
builder.Services.AddSingleton<IHostedService>(sp =>
   new KafkaConsumer(sp.GetRequiredService<IConfiguration>(),
       sp.GetRequiredService<ILogger<KafkaConsumer>>(),
       sp.GetRequiredService<ISerializer>(),
       sp.GetRequiredService<IEnumerable<IMessageHandler>>(),
       sp.GetRequiredService<IMessageHandler>(),
       ""));

var app = builder.Build();

app.Run();

