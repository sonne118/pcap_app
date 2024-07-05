using AutoMapper;
using CoreModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;
using wpfapp.Services.Worker;

namespace MVVM
{
    public class GridViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly DispatcherTimer _timer;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        public ObservableCollection<SnifferData> _SnifferData { get; set; }
        public List<string> _comboBox { get; set; }

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
                             IMapper mapper)
        {
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            if (device?.GetDevices() is IEnumerable<string> ls)
            {
                _comboBox = new List<string>(ls);
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