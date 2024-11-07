using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MVVM;
using System.Windows.Input;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using wpfapp.Utilities;

namespace wpfapp.ViewModel
{
    public class NavigationViewModel : GridViewModel
    {
        private object _currentView;
        private IBackgroundJobs<Snapshot>? _backgroundJobs;
        private IMapper? _mapper;
        private IDevices? _devices;
        private IServiceScopeFactory? _scopeFactory;
        private IPutDevice _putDevice;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                Set(ref _currentView, value);
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DashboardCommand { get; set; }
        public ICommand FileCommand { get; set; }
        public ICommand TreeCommand { get; set; }
        public ICommand SettingsCommand { get; set; }


        private void Home(object obj) => CurrentView = new HomeViewModel(_backgroundJobs,
                                                                         _devices,
                                                                         _mapper,
                                                                         _scopeFactory);  //, this
        private void Dashboard(object obj) => CurrentView = new DashboardViewModel();
        private void Tree(object obj) => CurrentView = new TreeViewModel();
        private void File(object obj) => CurrentView = new FileViewModel();
        private void Settings(object obj) => CurrentView = new SettitngsViewModel();

        public NavigationViewModel(
                       IBackgroundJobs<Snapshot> backgroundJobs,
                       IDevices devices,
                       IPutDevice putDevice,
                       IMapper mapper,
                       IServiceScopeFactory scopeFactory) : base(devices, mapper, backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
            _devices = devices;
            _putDevice = putDevice;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
            HomeCommand = new RelayCommand(Home);
            DashboardCommand = new RelayCommand(Dashboard);
            FileCommand = new RelayCommand(File);
            TreeCommand = new RelayCommand(Tree);
            SettingsCommand = new RelayCommand(Settings);

            // Startup Page
            CurrentView = new HomeViewModel(_backgroundJobs, _devices, _mapper, _scopeFactory);
        }

        public override void SetDevice(string str)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                Int32.TryParse(str?.Substring(0, 1), out var dev);

                _worker.DoWork += (s, e) =>
                {
                    service.PutDevices(dev);
                };
                _worker.RunWorkerAsync();
            }
        }

        public override void OnProcessQueue(object sender, EventArgs e) { }

        public override void Dispose() { }

    }
}
