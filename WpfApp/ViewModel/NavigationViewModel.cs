using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using wpfapp.IPC.Grpc;
using wpfapp.Infrastructure.ViewModels;
using wpfapp.Infrastructure.Services;

namespace wpfapp.ViewModel
{
    public class NavigationViewModel : NavigationAbstract
    {
        private readonly INavigationService _navigationService;
        private IServiceScopeFactory? _scopeFactory;
        private readonly IPutDevice _putDevice;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        private INotifyPropertyChanged _currentView;
        public INotifyPropertyChanged CurrentView
        {
            get => _currentView;
            private set => SetProperty(ref _currentView, value);
        }

        // Commands
        public ICommand HomeCommand { get; }
        public ICommand DashboardCommand { get; }
        public ICommand FileCommand { get; }
        public ICommand TreeCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand OnClosingCommand { get; }
        public ICommand OnSetGrpcServiceCommand { get; }
        public ICommand OnStartStreamServiceCommand { get; }
        public ICommand OnStopStreamServiceCommand { get; }

        public NavigationViewModel(
            INavigationService navigationService,
            IServiceScopeFactory scopeFactory) : base(scopeFactory)
        // IPutDevice putDevice,
        // IBackgroundJobs<Snapshot> backgroundJobs) : base(scopeFactory)
        {
            _navigationService = navigationService;
            _scopeFactory = scopeFactory;
           // _putDevice = putDevice;
           // _backgroundJobs = backgroundJobs;

            // Initialize commands
            HomeCommand = new RelayCommand(_ => NavigateToHome());
            DashboardCommand = new RelayCommand(_ => NavigateToDashboard());
            FileCommand = new RelayCommand(_ => NavigateToFile());
            TreeCommand = new RelayCommand(_ => NavigateToTree());
            SettingsCommand = new RelayCommand(_ => NavigateToSettings());
            OnClosingCommand = new RelayCommand(OnClosing);
            OnSetGrpcServiceCommand = new RelayCommand<bool>(OnSetGrpcService);
            OnStartStreamServiceCommand = new AsyncRelayCommand(OnStartStreamServiceAsync);
            OnStopStreamServiceCommand = new AsyncRelayCommand(OnStopStreamServiceAsync);

            // Subscribe to navigation changes
            _navigationService.NavigationChanged += OnNavigationChanged;

            // Set initial view
            NavigateToHome();
        }

        private void NavigateToHome() => _navigationService.NavigateTo<HomeViewModel>();
        private void NavigateToDashboard() => _navigationService.NavigateTo<DashboardViewModel>();
        private void NavigateToFile() => _navigationService.NavigateTo<FileViewModel>();
        private void NavigateToTree() => _navigationService.NavigateTo<TreeViewModel>();
        private void NavigateToSettings() => _navigationService.NavigateTo<SettitngsViewModel>();

        private void OnNavigationChanged(object sender, INotifyPropertyChanged viewModel)
        {
            CurrentView = viewModel;
        }

        private void OnClosing(object parameter)
        {
            _stoppingCts.Cancel();
            Application.Current.Shutdown();
        }

        private void OnSetGrpcService(bool isChecked)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IHostedGrpcService>();
                    if (!isChecked)
                    {
                        _ = service.StartAsync(_stoppingCts.Token);
                    }
                    else
                    {
                        _ = service.StopAsync(_stoppingCts.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error managing gRPC service: {ex.Message}");
            }
        }

        private async Task OnStartStreamServiceAsync(object parameter)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                    service.StartAsync(_stoppingCts.Token);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting stream service: {ex.Message}");
            }
        }

        private async Task OnStopStreamServiceAsync(object parameter)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                    service.StopAsync(_stoppingCts.Token);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping stream service: {ex.Message}");
            }
        }

        public override void SetDevice(string str)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                    Int32.TryParse(str?.Substring(0, 1), out var dev);
                    _worker.DoWork += (s, e) =>
                    {
                        service.PutDeviceAsync(dev);
                    };
                    _worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting device: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _navigationService.NavigationChanged -= OnNavigationChanged;
                _stoppingCts?.Cancel();
                _stoppingCts?.Dispose();
            }
            // base.Dispose(disposing);
        }
    }
}
// The navigation issue may stem from the following potential problems in the NavigationViewModel class:

// 1. Ensure that the INavigationService is properly initialized and assigned.
// 2. Check if the NavigationChanged event is being triggered correctly.
// 3. Verify that the NavigateTo methods are correctly implemented in the INavigationService.
// 4. Ensure that the CurrentView property is being set correctly and is bound to the UI.
// 5. Make sure that the views (HomeViewModel, DashboardViewModel, etc.) are correctly implemented and registered.

//private readonly INavigationService _navigationService; // Ensure this is assigned in the constructor
//// Example of how to assign it if uncommented
//_navigationService = navigationService; // Uncomment this line in the constructor

//// Ensure that the CurrentView property is implemented and used correctly
//public INotifyPropertyChanged CurrentView
//{
//    get => _currentView;
//    private set => SetProperty(ref _currentView, value); // Ensure SetProperty is defined
//}

//// Check if the NavigateTo methods are correctly implemented in the INavigationService
//private void NavigateToHome() => _navigationService.NavigateTo<HomeViewModel>();
//private void NavigateToDashboard() => _navigationService.NavigateTo<DashboardViewModel>();
//private void NavigateToFile() => _navigationService.NavigateTo<FileViewModel>();
//private void NavigateToTree() => _navigationService.NavigateTo<TreeViewModel>();
//private void NavigateToSettings() => _navigationService.NavigateTo<SettitngsViewModel>();
