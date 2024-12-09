using Microsoft.AspNetCore.Server.Kestrel.Core;
using Server.Map;
using Server.Model;
using Server.Services;
using Server.Services.BackgroundJobs;


var builder = WebApplication.CreateBuilder(args);
string? certPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
string? certPass = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");
var path = Path.Combine(builder.Environment.ContentRootPath, "ssl\\server.pfx");

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
        listenOptions.UseHttps(certPath, certPass);
        //listenOptions.UseHttps(path, "11111");  
       //listenOptions.ht
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    //option.ListenLocalhost(5000, listenOptions =>
    //{
    //    listenOptions.Protocols = HttpProtocols.Http2;
    //});
});

builder.Services.AddGrpc();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();

var app = builder.Build();

app.MapGrpcService<StreamingService>();

app.UseRouting();

//app.UseWebSockets(new Microsoft.AspNetCore.Builder.WebSocketOptions
//{
//    KeepAliveInterval = TimeSpan.FromSeconds(120),
//});

app.UseEndpoints(endpoints =>
{
    // endpoints.MapHub<DataHub>("/dataHub");
});

app.Run();


