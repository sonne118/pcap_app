using AutoMapper;
using CoreModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;
using WpfApp.Services.Reader;


namespace MVVM
{
    public class GridViewModel : INotifyPropertyChanged
    {
        private readonly IMapper _mapper;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private SnifferData selectedSnifferData;
        private DispatcherTimer _timer;
        public ObservableCollection<SnifferData> _SnifferData { get; set; }

        public SnifferData SelectedSnifferData
        {
            get { return selectedSnifferData; }
            set
            {
                selectedSnifferData = value;
                OnPropertyChanged("SelectedSnifferData");
            }
        }
        public GridViewModel(IBackgroundJobs<Snapshot> backgroundJobs, IMapper mapper)
        {
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            _SnifferData = new ObservableCollection<SnifferData>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(100);
            _timer.Tick += ProcessQueue;
            _timer.IsEnabled = true;
        }

        private void ProcessQueue(object sender, EventArgs e)
        {
            while (_backgroundJobs.BackgroundTasks.TryDequeue(out var data))
            {
                var _snifferData = _mapper.Map<SnifferData>(data);
                _SnifferData.Add(_snifferData);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
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