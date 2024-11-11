using GrpcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using wpfapp;
using wpfapp.IPC.Grpc;
using wpfapp.models;
using wpfapp.Services.Worker;
using wpfapp.Map;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using System.Windows.Navigation;
using wpfapp.ViewModel;

namespace wpfapp
{
    public partial class App : Application
    {
        private readonly static string? path;
        private readonly static string? basePath;
        private readonly string? certPath;
        private readonly string? certPass;
        static App()
        {
            path = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
            basePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        }

        public static IHost? AppHost { get; private set; }
        public App()
        {
            if (path is not null && Path.Exists(basePath))
            {
                certPath = Path.GetFullPath(path, basePath);
            }
            certPass = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IStreamData, StreamData>();
                    services.AddSingleton<IHostedService, StartService>();
                    services.AddHostedService(s => s.GetRequiredService<StartService>());
                    services.AddScoped<IDevices, Devices>();
                    services.AddTransient<IPutDevice, PutDevice>();
                    services.AddSingleton<IHostedGrpcService, GrpcService>();
                    services.AddGrpcClient<StreamingDates.StreamingDatesClient>(options =>
                    {
                        options.Address = new Uri("https://localhost:5001");
                    })
                     .ConfigurePrimaryHttpMessageHandler(() =>
                     {
                         var handler = new HttpClientHandler();
                         handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                         handler.ClientCertificates.Add(new X509Certificate2(certPath, certPass));
                         return handler;
                     });
                    services.AddSingleton<NavigationViewModel>();

                    services.AddAutoMapper(typeof(AppMappingProfile));
                })
                .Build();
        }

        //protected override async void OnStartup111(StartupEventArgs e)
        //{
        //    await AppHost!.StartAsync();
        //    var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        //    startupForm.Show();

        //    base.OnStartup(e);
        //}

        protected  async void OnExit(object sender, ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }

        protected async void OnStartup(object sender, StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);

        }
    }
}



