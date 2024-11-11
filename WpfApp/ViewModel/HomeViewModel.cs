﻿using AutoMapper;
using CoreModel.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Windows.Input;
using wpfapp.IPC.Grpc;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;

namespace wpfapp.ViewModel
{
    public class HomeViewModel : HomeAbstract
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IMapper _mapper;
        public ClosingCommand _closingCommand;
        public DataGridDoubleClickCommand _dataGridDoubleClickCommand;

        private  ConcurrentQueue<Snapshot> _backgroundJobs;
        public ConcurrentQueue<Snapshot> BackgroundJobs
        {
            get { return _backgroundJobs; }
            set
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IBackgroundJobs<Snapshot>>();
                    if (service?.BackgroundTasks is ConcurrentQueue<Snapshot> dic)
                        _backgroundJobs = dic;
                    else _backgroundJobs = value;
                }
            }
        }

        public IMapper Mapper
        {
            get { return _mapper; }
            set
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IMapper> ();
                    if (service is Mapper mapper)
                        _mapper = mapper;
                    else _mapper = value;
                }
            }
        }

        public HomeViewModel(IServiceScopeFactory scopeFactory) 
        //MainWindow mainWindow) : base(device)
        {
            _scopeFactory = scopeFactory;
            // _closingCommand = new ClosingCommand(mainWindow);
            //_dataGridDoubleClickCommand = new DataGridDoubleClickCommand(mainWindow);
            OnSetGrpcService = new RelayCommand<bool>(OnExecuteGrpcService);
            OnStartStreamService = new RelayCommand(OnExecuteStartService);
            OnStopStreamService = new RelayCommand(OnExecuteStopService);
            BackgroundJobs = _backgroundJobs;
            Mapper = _mapper;
        }

        public RelayCommand<Boolean> OnSetGrpcService { get; private set; }
        public ICommand OnStartStreamService { get; private set; }
        public ICommand OnStopStreamService { get; private set; }
        
      //public ICommand OnClosingCommand { get { return _closingCommand.ExitCommand; } }
        
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

            while (BackgroundJobs.TryDequeue(out var data))
            {
                var _snifferData = Mapper.Map<StreamingData>(data);
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
