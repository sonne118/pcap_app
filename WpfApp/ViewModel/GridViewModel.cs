using AutoMapper;
using CoreModel.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Threading;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;

namespace MVVM
{
    public abstract class GridViewModel : ViewModelBase, IDisposable
    {
        private IDevices _devices;

        protected readonly DispatcherTimer _timer;

        protected readonly CancellationTokenSource _stoppingCts = new();
        public ObservableCollection<StreamingData> _StreamingData { get; set; } = new();
        public ObservableCollection<StreamingData> _SelectedData { get; set; } = new();

        private ObservableCollection<string> _items = new();

        protected readonly ISubject<Snapshot> _dataSubject = new Subject<Snapshot>();

        protected readonly CompositeDisposable _disposable = new();

        protected readonly BackgroundWorker _worker = new();

        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                if (_devices?.GetDevices() is IEnumerable<string> ls)
                {
                    _items.AddRange(ls);
                }
                Set(ref _items, value);
            }
        }

        private StreamingData _selectedSnifferData;
        public StreamingData SelectedSnifferData
        {
            get => _selectedSnifferData;
            set
            {
                Set(ref _selectedSnifferData, value);
            }
        }

        private StreamingData _selectedData;
        public StreamingData SelectedData
        {
            get => _selectedData;
            set
            {
                Set(ref _selectedData, value);
            }
        }

        private string _selectedItem;
        public string _SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                SetDevice(_selectedItem);
            }
        }

        private StreamingData _selectedRow;

        public StreamingData SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (Set(ref _selectedRow, value))
                {
                    if (_selectedRow != null)
                        _selectedRow.IsSelected = true;
                    _SelectedData.Add(value);
                }
            }
        }

        public GridViewModel(IDevices devices,
                            IMapper mapper,
                            IBackgroundJobs<Snapshot> backgroundJobs)
        {
           
            _devices = devices;
            Items = _items;

            _dataSubject
              .ObserveOn(SynchronizationContext.Current)
              .Subscribe(data =>
              {
                  if (backgroundJobs.BackgroundTasks.TryDequeue(out var value))
                  {
                      var _snifferData = mapper.Map<StreamingData>(value);
                      _StreamingData.Add(_snifferData);
                  }
              })
               .DisposeWith(_disposable);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(1000);
            _timer.Tick += OnProcessQueue;
            _timer.IsEnabled = true;
        }

        public abstract void OnProcessQueue(object sender, EventArgs e);
        public abstract void SetDevice(string str);
        public abstract void Dispose();
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