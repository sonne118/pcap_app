using CoreModel.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace wpfapp.ViewModel
{
    public abstract class HomeAbstract : ViewModelBase, IDisposable
    {
        protected readonly DispatcherTimer _timer;
        
        protected readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        public ObservableCollection<StreamingData> _StreamingData { get; set; } = new ObservableCollection<StreamingData>();
        public ObservableCollection<StreamingData> _SelectedData { get; set; } = new ObservableCollection<StreamingData>();

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

        private StreamingData _selectedRow;

        public StreamingData SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (Set(ref _selectedRow, value))
                {
                    if (_selectedRow != null)/// && Mouse.PrimaryDevice.RightButton is (MouseButtonState.Pressed| MouseButtonState.Released))
                        _selectedRow.IsSelected = true;
                        _SelectedData.Add(value);
                }
            }
        }

        public HomeAbstract()
        {         
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(100);
            _timer.Tick += OnProcessQueue;
            _timer.IsEnabled = true;
        }

        public abstract void OnProcessQueue(object sender, EventArgs e);
        public abstract void Dispose();
    }
}