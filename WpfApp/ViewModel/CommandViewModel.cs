using System;
using AutoMapper;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows.Input;
using wpfapp.IPC.Grpc;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public class CommandViewModel : GridViewModel
    {
        public ClosingCommand closingCommand;

        public DataGridDoubleClickCommand dataGridDoubleClickCommand;
        public ICommand OnClosingCommand { get { return closingCommand.ExitCommand; } }
        public ICommand OnDataGridDoubleClickCommand { get { return dataGridDoubleClickCommand.ShowCommand; } }

        public CommandViewModel(IBackgroundJobs<Snapshot> backgroundJobs,
                                IDevices device,
                                IMapper mapper,
                                IServiceScopeFactory scopeFactory,
                                MainWindow mainWindow) : base(backgroundJobs, device, mapper, scopeFactory )
        {
            SetGrpcService = new RelayCommand<bool>(OnExecuteGrpcService);
            StartStreamService = new RelayCommand(OnExecuteStartService);
            StopStreamService = new RelayCommand(OnExecuteStopService);
            closingCommand = new ClosingCommand(mainWindow);
            dataGridDoubleClickCommand = new DataGridDoubleClickCommand(mainWindow);
        }

        public RelayCommand<Boolean> SetGrpcService { get; private set; }
        public ICommand StartStreamService { get; private set; }
        public ICommand StopStreamService { get; private set; }


        private void OnExecuteGrpcService(bool isChecked)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedGrpcService>();
                if (!isChecked)
                {
                    service.StartAsync(_stoppingCts.Token);
                }
                else
                {
                    service.StopAsync(_stoppingCts.Token);
                }
            }
        }
        private void OnExecuteStartService()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StartAsync(_stoppingCts.Token);
            }
        }

        private void OnExecuteStopService()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StopAsync(_stoppingCts.Token);
            }
        }
    }
}
