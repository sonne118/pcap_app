using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

string? certPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
string? certPass = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.WebHost.ConfigureKestrel(option =>
{
    option.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(certPath, certPass);
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

app.MapReverseProxy();

app.Run();
