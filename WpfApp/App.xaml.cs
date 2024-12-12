using GrpcClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using wpfapp.IPC.Grpc;
using wpfapp.Map;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
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
            basePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        }

        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile("appsettings.json", optional: true);
                 })
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
                         var serviceProvider = services.BuildServiceProvider();
                         var _configuration = serviceProvider.GetRequiredService<IConfiguration>();
                         var path = _configuration["Certificate:Path"];
                         var certPassword = _configuration["Certificate:Password"];
                         var certPath = Path.GetFullPath(path, basePath);
                         var handler = new HttpClientHandler();
                         handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                         handler.ClientCertificates.Add(new X509Certificate2(certPath, certPassword));
                         return handler;
                     });
                    services.AddSingleton<NavigationViewModel>();

                    services.AddAutoMapper(typeof(AppMappingProfile));
                })
                .Build();
        }

        protected async void OnExit(object sender, ExitEventArgs e)
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



