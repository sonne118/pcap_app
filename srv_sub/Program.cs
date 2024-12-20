using kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Services.AddKafkaSubscribe("");

var app = builder.Build();

app.Run();

