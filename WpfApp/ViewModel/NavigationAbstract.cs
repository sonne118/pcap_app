using GalaSoft.MvvmLight;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public abstract class NavigationAbstract : ViewModelBase
    {
        private object _currentView;
        private IDevices? _devices;
        private readonly IServiceScopeFactory _scopeFactory;
        private string _selectedItem;

        protected ObservableCollection<string> _items = new ObservableCollection<string>();
        protected readonly BackgroundWorker _worker = new();

        public object CurrentView
        {
            get => _currentView;
            set
            {
                Set(ref _currentView, value);
            }
        }

        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                Set(ref _items, value);

                if (SetDeviceList() is IEnumerable<string> list)
                {
                    _items.AddRange(list);
                }
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

        public NavigationAbstract(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            Items = _items;
        }

        private IEnumerable<string> SetDeviceList()
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDevices>();
                if (service?.GetDevices() is IEnumerable<string> list)
                    return list;
            }
            return Enumerable.Empty<string>();
        }
        public abstract void SetDevice(string str);

    }
}
