using AutoMapper;
using CoreModel.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using wpfapp.IPC.Grpc;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public class GridViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly IMapper _mapper;
        private readonly DispatcherTimer _timer;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        public RelayCommand<Boolean> SetGrpcService { get; private set; }
        public ICommand StartStreamService { get; private set; }
        public ICommand StopStreamService { get; private set; }
        public ObservableCollection<StreamingData> _StreamingData { get; set; }
        public ObservableCollection<string> Items { get; set; }
        private StreamingData _selectedSnifferData;
        private string _selectedItem;

        public StreamingData SelectedSnifferData
        {
            get => _selectedSnifferData;
            set
            {
                _selectedSnifferData = value;
                OnPropertyChanged("SelectedSnifferData");
            }
        }

        public string _SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                SetDevice(_selectedItem);
            }
        }

        public GridViewModel(IBackgroundJobs<Snapshot> backgroundJobs,
                             IDevices device,
                             IMapper mapper,
                             IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            SetGrpcService = new RelayCommand<bool>(OnExecuteGrpcService);
            StartStreamService = new RelayCommand(OnExecuteStartService);
            StopStreamService = new RelayCommand(OnExecuteStopService);
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            if (device?.GetDevices() is IEnumerable<string> ls)
            {
                Items = new ObservableCollection<string>(ls);
            }
            _StreamingData = new ObservableCollection<StreamingData>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(100);
            _timer.Tick += ProcessQueue;
            _timer.IsEnabled = true;
        }
        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= ProcessQueue;
        }

        private void ProcessQueue(object sender, EventArgs e)
        {
            while (_backgroundJobs.BackgroundTasks.TryDequeue(out var data))
            {
                var _snifferData = _mapper.Map<StreamingData>(data);
                _StreamingData.Add(_snifferData);
            }
        }

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

        private bool Set<T>(ref T field, T value, bool forceNotify = false, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value))
            {
                if (forceNotify) OnPropertyChanged(PropertyName);
                return false;
            }
            field = value;
            OnPropertyChanged(PropertyName);

            return true;
        }

        private void SetDevice(string str)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                Int32.TryParse(str?.Substring(0, 1), out var dev);
                service.PutDevices(dev);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }

    public static class LinqExtensions
    {
        public static ICollection<Data> AddRange(this ICollection<Data> source, IEnumerable<PcapStruct> addSource)
        {
            foreach (var item in addSource)
            {
                source.Add(new Data
                {
                    Id = item.id,
                    Source_ip = item.source_ip,
                    Dest_ip = item.dest_ip,
                    Mac_source = item.mac_source,
                    Mac_destin = item.mac_destin,
                    User_agent = item.user_agent
                });
            }
            return source;
        }
    }

}