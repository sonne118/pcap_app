using GrpcClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using wpfapp.Infrastructure.DependencyInjection;
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
                    services.AddApplicationServices(hostContext.Configuration); // Fix: Use hostContext.Configuration instead of undefined 'configuration'

                    services.AddGrpcServices(hostContext.Configuration);
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



