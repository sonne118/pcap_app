using System;
using AutoMapper;
using CoreModel.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public abstract class GridViewModel : ViewModelBase, IDisposable
    {
        protected readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        public ObservableCollection<StreamingData> _StreamingData { get; set; } = new ObservableCollection<StreamingData>();
        public ObservableCollection<string> Items { get; set; }       
        
        protected readonly  IBackgroundJobs<Snapshot> _backgroundJobs;
        protected readonly  IServiceScopeFactory _scopeFactory;        
        protected readonly  IMapper _mapper;
        private   readonly  DispatcherTimer _timer;

        protected StreamingData _selectedSnifferData;
        private string _selectedItem;

        public StreamingData SelectedSnifferData
        {
            get => _selectedSnifferData;
            set
            {
                Set(ref _selectedSnifferData, value);
                //_selectedSnifferData = value;
                //OnPropertyChanged("SelectedSnifferData");
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
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            if (device?.GetDevices() is IEnumerable<string> ls)
            {
                Items = new ObservableCollection<string>(ls);
            }         
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(100);
            _timer.Tick += ProcessQueue;
            _timer.IsEnabled = true;
        }
     

        private void ProcessQueue(object sender, EventArgs e)
        {
            while (_backgroundJobs.BackgroundTasks.TryDequeue(out var data))
            {
                var _snifferData = _mapper.Map<StreamingData>(data);
                _StreamingData.Add(_snifferData);
            }
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
      
        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= ProcessQueue;
        }
    }    
}