using System;
using CoreModel.Model;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;
using wpfapp.Services.Worker;

namespace MVVM
{
    public abstract class GridViewModel : ViewModelBase, IDisposable
    {
        private IDevices _device;
        protected readonly DispatcherTimer _timer;
        protected readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        
        public ObservableCollection<StreamingData> _StreamingData { get; set; } = new ObservableCollection<StreamingData>();        
        private ObservableCollection<string> _items = new ObservableCollection<string>();
        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                if (_device?.GetDevices() is IEnumerable<string> ls)
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

        public GridViewModel(IDevices device)
        {
            _device = device;
             Items = _items;           
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(100);
            _timer.Tick += ProcessQueue;
            _timer.IsEnabled = true;
        }

        public abstract void ProcessQueue(object sender, EventArgs e);
        public abstract void SetDevice(string str);
        public abstract void OnPropertyChanged([CallerMemberName] string prop = "");
        public abstract void Dispose();
    }
}