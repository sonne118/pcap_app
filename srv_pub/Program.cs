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
    option.ListenAnyIP(5000, listenOptions =>
    {
        // Use HTTP/2 over cleartext (h2c). Do not enable HTTPS here.
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    // Only configure HTTPS if certificate exists
    var certPath = builder.Configuration["Certificate:Path"];
    var certPassword = builder.Configuration["Certificate:Password"];
    
    if (!string.IsNullOrEmpty(certPath))
    {
        // Resolve the full path relative to the content root
        var fullCertPath = Path.Combine(builder.Environment.ContentRootPath, certPath);

        if (File.Exists(fullCertPath))
        {
            option.ListenAnyIP(5001, listenOptions =>
            {
                listenOptions.UseHttps(fullCertPath, certPassword);
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        }
        else
        {
            Console.WriteLine($"WARNING: Certificate file not found at: {fullCertPath}. HTTPS endpoint will not be available.");
        }
    }
});

builder.Services.AddGrpc(option => {
   ///// option.Interceptors.Add<ServerLoggerInterceptor>();
});

builder.Services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();


//builder.Services.AddScoped<IOutboxInitializer, OutboxInitializer>();
builder.Services.AddKafkaPublish("");
builder.Services.AddOutbox();

// Add retry capability to MySQL connection
builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration["SqlConnStr"] ?? throw new ArgumentException(), 
    retryOnFailure: true, 
    maxRetryCount: 5);

builder.Services.AddAutoMapper(typeof(AppMappingProfile));


var app = builder.Build();

//using var scope = app.Services.CreateScope();
//var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//var outboxInitializer = scope.ServiceProvider.GetRequiredService<IOutboxInitializer>();
//await outboxInitializer.InitializeAsync(CancellationToken.None);

app.MapGrpcService<StreamingService>();

app.UseRouting();

app.Run();
