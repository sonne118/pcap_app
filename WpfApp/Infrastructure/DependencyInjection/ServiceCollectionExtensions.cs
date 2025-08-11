using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using GrpcClient;
using wpfapp.Infrastructure.Services;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using wpfapp.ViewModel;
using wpfapp.IPC.Grpc;
using wpfapp.Map;
using wpfapp.models;

namespace wpfapp.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register ViewModels
            services.AddTransient<NavigationViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<TreeViewModel>();
            services.AddTransient<FileViewModel>();
            services.AddTransient<SettitngsViewModel>();

            // Register Services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBackgroundJobs<Snapshot>, BackgroundJobs>();
            services.AddSingleton<IStreamData, StreamData>();
            services.AddScoped<IDevices, Devices>();
            services.AddTransient<IPutDevice, PutDevice>();
            services.AddSingleton<IHostedGrpcService, GrpcService>();

            // Register Workers
            services.AddHostedService<Worker>();
            services.AddSingleton<IHostedService, StartService>();
            services.AddHostedService(s => s.GetRequiredService<StartService>());

            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            var basePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

            services.AddGrpcClient<StreamingDates.StreamingDatesClient>(options =>
            {
                options.Address = new Uri("https://localhost:5001");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var path = configuration["Certificate:Path"];
                var certPassword = configuration["Certificate:Password"];
                var certPath = Path.GetFullPath(path, basePath);
                
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                handler.ClientCertificates.Add(new X509Certificate2(certPath, certPassword));
                
                return handler;
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AppMappingProfile));
            return services;
        }
    }
}
