using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace wpfapp.Infrastructure.Services
{
    public interface INavigationService
    {
        INotifyPropertyChanged CurrentViewModel { get; }
        void NavigateTo<T>() where T : class, INotifyPropertyChanged;
        void NavigateTo(Type viewModelType);
        event EventHandler<INotifyPropertyChanged> NavigationChanged;
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private INotifyPropertyChanged _currentViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INotifyPropertyChanged CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                NavigationChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<INotifyPropertyChanged> NavigationChanged;

        public void NavigateTo<T>() where T : class, INotifyPropertyChanged
        {
            var viewModel = _serviceProvider.GetRequiredService<T>();
            CurrentViewModel = viewModel;
        }

        public void NavigateTo(Type viewModelType)
        {
            if (!typeof(INotifyPropertyChanged).IsAssignableFrom(viewModelType))
                throw new ArgumentException("Type must implement INotifyPropertyChanged", nameof(viewModelType));

            var viewModel = (INotifyPropertyChanged)_serviceProvider.GetRequiredService(viewModelType);
            CurrentViewModel = viewModel;
        }
    }
}
