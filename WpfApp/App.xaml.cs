using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVM;
using System.Windows;
using wpfapp.Services.Worker;
using WpfApp.Map;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;
using WpfApp.Services.Worker;

namespace WpfApp
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IStreamData, StreamData>();
                    services.AddSingleton<IHostDevice, StartService>();
                    //services.AddHostedService<StartService>(s =>s.GetRequiredService<StartService>());
                    services.AddSingleton<IDevices, Devices>();
                    services.AddSingleton<IPutDevice, PutDevice>();
                    services.AddAutoMapper(typeof(AppMappingProfile));
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }
    }
}



