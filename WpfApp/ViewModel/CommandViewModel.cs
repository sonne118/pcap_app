using AutoMapper;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reactive.Disposables;
using System.Windows.Input;
using wpfapp.IPC.Grpc;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public class CommandViewModel : GridViewModel
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;

        public ClosingCommand _closingCommand;
        public DataGridDoubleClickCommand _dataGridDoubleClickCommand;

        public CommandViewModel(IBackgroundJobs<Snapshot> backgroundJobs,
                                IDevices device,
                                IMapper mapper,
                                IServiceScopeFactory scopeFactory,
                                MainWindow mainWindow) : base(device, mapper, backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
            _scopeFactory = scopeFactory;
            _closingCommand = new ClosingCommand(mainWindow);
            _dataGridDoubleClickCommand = new DataGridDoubleClickCommand(mainWindow);
            OnSetGrpcService = new RelayCommand<bool>(OnExecuteGrpcService);
            OnStartStreamService = new RelayCommand(OnExecuteStartService);
            OnStopStreamService = new RelayCommand(OnExecuteStopService);

        }

        public RelayCommand<Boolean> OnSetGrpcService { get; private set; }
        public ICommand OnStartStreamService { get; private set; }
        public ICommand OnStopStreamService { get; private set; }
        public ICommand OnClosingCommand { get { return _closingCommand.ExitCommand; } }
        public ICommand OnDataGridDoubleClickCommand { get { return _dataGridDoubleClickCommand.ShowCommand; } }


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

        public override void SetDevice(string str)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                Int32.TryParse(str?.Substring(0, 1), out var dev);

                _worker.DoWork += (s, e) =>
                {
                    service.PutDevices(dev);
                };
                _worker.RunWorkerAsync();
            }
        }

        public override void OnProcessQueue(object sender, EventArgs e)
        {
            if (_backgroundJobs.BackgroundTasks.TryPeek(out var data))
            {
                _dataSubject.OnNext(data);
            }
        }

        public override void Dispose()
        {
            _disposable.Dispose();
            _timer.Stop();
            _timer.Tick -= OnProcessQueue;
        }
    }

    public static class DisposableExtensions
    {
        public static T DisposeWith<T>(this T disposable, CompositeDisposable container)
            where T : IDisposable
        {
            container.Add(disposable);
            return disposable;
        }
    }
}

