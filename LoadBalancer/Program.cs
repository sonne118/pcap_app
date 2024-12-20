using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

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
