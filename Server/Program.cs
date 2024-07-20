using Microsoft.AspNetCore.Server.Kestrel.Core;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

string? certPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
string? certPass = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

builder.WebHost.ConfigureKestrel(option =>
{
    option.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(certPath, certPass);  ///@"ssl\\server.pfx"
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<StreamingService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();


