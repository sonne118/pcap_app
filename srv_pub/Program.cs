using kafka;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using outbox;
using Server.Map;
using Server.Model;
using Server.Services;
using Server.Services.BackgroundJobs;
using srv_pub.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        //builder.WithOrigins("http://localhost:3000", "http://localhost:5000")
        builder.WithOrigins("*")
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

builder.WebHost.ConfigureKestrel(option =>
{
    option.ListenAnyIP(5001, listenOptions =>
    {
        var certPath = builder.Configuration["Certificate:Path"];
        var certPassword = builder.Configuration["Certificate:Password"];
        listenOptions.UseHttps(certPath, certPassword);
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();

//builder.Services.AddSignalR();
 builder.Services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();

builder.Services.AddKafkaPublish("");
builder.Services.AddOutbox();

builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration["SqlConnStr"] ?? throw new ArgumentException());



builder.Services.AddAutoMapper(typeof(AppMappingProfile));

//builder.Services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();

var app = builder.Build();

app.MapGrpcService<StreamingService>();

app.UseRouting();

app.Run();


