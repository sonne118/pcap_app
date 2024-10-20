using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

//string? certsPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
//string? password = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

//var config = builder.Configuration.GetSection("Kestrel:Certificates:Development");
//string certsPath = config.GetSection("CertsPath").Value ?? string.Empty;
//string password = config.GetSection("Password").Value ?? string.Empty;

//var path = Path.Combine(builder.Environment.ContentRootPath, "ssl\\server.pfx");

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.WebHost.ConfigureKestrel(option =>
{
    option.ListenAnyIP(5001, listenOptions =>
    {
        //listenOptions.UseHttps(
        //    adapterOptions =>
        //    {
        //        adapterOptions.ServerCertificate = new X509Certificate2("/https/server.pfx", "11111");
        //    });

        listenOptions.UseHttps("/https/server.pfx", "11111");
        listenOptions.Protocols = HttpProtocols.Http2;
             
    });
});


var app = builder.Build();

app.MapReverseProxy();

app.Run();
