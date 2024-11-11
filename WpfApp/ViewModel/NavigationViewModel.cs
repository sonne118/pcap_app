using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using wpfapp.Utilities;

namespace wpfapp.ViewModel
{
    public class NavigationViewModel : NavigationAbstract
    {
        private IMapper? _mapper;
        private IPutDevice _putDevice;
        private IServiceScopeFactory? _scopeFactory;
        private IBackgroundJobs<Snapshot>? _backgroundJobs;

        public ICommand HomeCommand { get; set; }
        public ICommand DashboardCommand { get; set; }
        public ICommand FileCommand { get; set; }
        public ICommand TreeCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = Singleton<HomeViewModel>.Instance(_scopeFactory); //this
        private void Dashboard(object obj) => CurrentView = Singleton<DashboardViewModel>.Instance();
        private void Tree(object obj) => CurrentView = Singleton<TreeViewModel>.Instance();
        private void File(object obj) => CurrentView = Singleton<FileViewModel>.Instance();
        private void Settings(object obj) => CurrentView = Singleton<SettitngsViewModel>.Instance();

        public NavigationViewModel(IServiceScopeFactory scopeFactory)
                                  : base(scopeFactory)
        {
            _scopeFactory = scopeFactory;
            HomeCommand = new RelayCommand(Home);
            DashboardCommand = new RelayCommand(Dashboard);
            FileCommand = new RelayCommand(File);
            TreeCommand = new RelayCommand(Tree);
            SettingsCommand = new RelayCommand(Settings);

            // startup page
            CurrentView = Singleton<HomeViewModel>.Instance(_scopeFactory);
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
    }
}
