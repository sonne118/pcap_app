using AutoMapper;
using CoreModel.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
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

        public ObservableCollection<SnifferData> _SnifferData { get; set; }
        public List<string> _ComboBox { get; set; }

        private SnifferData _selectedSnifferData;
        public SnifferData SelectedSnifferData
        {
            get { return _selectedSnifferData; }
            set
            {
                _selectedSnifferData = value;
                OnPropertyChanged("SelectedSnifferData");
            }
        }
        public GridViewModel(IBackgroundJobs<Snapshot> backgroundJobs,
                             IDevices device,
                             IMapper mapper,
                             IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            SetGrpcService = new RelayCommand<bool>(ExecuteGrpcService);
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            if (device?.GetDevices() is IEnumerable<string> ls)
            {
                _ComboBox = new List<string>(ls);
            }
            _SnifferData = new ObservableCollection<SnifferData>();
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
                var _snifferData = _mapper.Map<SnifferData>(data);
                _SnifferData.Add(_snifferData);
            }
        }

        void ExecuteGrpcService(bool isChecked)
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