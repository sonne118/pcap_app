using CoreModel.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using wpfapp.Services.Worker;

namespace MVVM
{
    public abstract class GridViewModel : ViewModelBase, IDisposable
    {
        private IDevices _devices;
        protected readonly DispatcherTimer _timer;
        protected readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        
        public ObservableCollection<StreamingData> _StreamingData { get; set; } = new ObservableCollection<StreamingData>();
        public ObservableCollection<StreamingData> _SelectedData { get; set; } = new ObservableCollection<StreamingData>();

        private ObservableCollection<string> _items = new ObservableCollection<string>();
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

        public GridViewModel(IDevices device)
        {
            _devices = device;
             Items = _items;           
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(100);
            _timer.Tick += OnProcessQueue;
            _timer.IsEnabled = true;
        }

        public abstract void OnProcessQueue(object sender, EventArgs e);
        public abstract void SetDevice(string str);      
        public abstract void Dispose();
    }
}