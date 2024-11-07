using AutoMapper;
using CoreModel.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVM;
using System.Diagnostics;
using System.Windows.Input;
using wpfapp.IPC.Grpc;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public class HomeViewModel : GridViewModel
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private readonly IMapper _mapper;
        private readonly IDevices _devices;

        public ClosingCommand _closingCommand;
        public DataGridDoubleClickCommand _dataGridDoubleClickCommand;

        public HomeViewModel(IBackgroundJobs<Snapshot> backgroundJobs,
                                IDevices device,
                                IMapper mapper,
                                IServiceScopeFactory scopeFactory) : base(device)
        //MainWindow mainWindow) : base(device)
        {
            Debug.WriteLine("HomeModelView");
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            _scopeFactory = scopeFactory;
            // _closingCommand = new ClosingCommand(mainWindow);
            //_dataGridDoubleClickCommand = new DataGridDoubleClickCommand(mainWindow);
            OnSetGrpcService = new RelayCommand<bool>(OnExecuteGrpcService);
            OnStartStreamService = new RelayCommand(OnExecuteStartService);
            OnStopStreamService = new RelayCommand(OnExecuteStopService);
            _devices = device;
        }

        public RelayCommand<Boolean> OnSetGrpcService { get; private set; }
        public ICommand OnStartStreamService { get; private set; }
        public ICommand OnStopStreamService { get; private set; }
        // public ICommand OnClosingCommand { get { return _closingCommand.ExitCommand; } }
        //public ICommand OnDataGridDoubleClickCommand { get { return _dataGridDoubleClickCommand.ShowCommand; } }


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

        public override void OnProcessQueue(object sender, EventArgs e)
        {

            while (_backgroundJobs.BackgroundTasks.TryDequeue(out var data))
            {
                var _snifferData = _mapper.Map<StreamingData>(data);
                _StreamingData.Add(_snifferData);
            }
        }

        public override void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= OnProcessQueue;
        }
    }
}

