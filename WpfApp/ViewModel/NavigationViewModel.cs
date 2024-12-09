using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows.Input;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using wpfapp.Utilities;
///using GalaSoft.MvvmLight.CommandWpf;
using  wpfapp.IPC.Grpc; 

namespace wpfapp.ViewModel
{
    public class NavigationViewModel : NavigationAbstract
    {
        private IMapper? _mapper;
        private IPutDevice _putDevice;
        private IServiceScopeFactory? _scopeFactory;
        private IBackgroundJobs<Snapshot>? _backgroundJobs;
        protected readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        public ClosingCommand _closingCommand;
        //public DataGridDoubleClickCommand DataGridDoubleClickCommand;

        public ICommand HomeCommand { get; set; }
        public ICommand DashboardCommand { get; set; }
        public ICommand FileCommand { get; set; }
        public ICommand TreeCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand OnClosingCommand { get { return _closingCommand.ExitCommand; } }
        //public ICommand OnDataGridDoubleClickCommand { get { return DataGridDoubleClickCommand.ShowCommand; } }

         public GalaSoft.MvvmLight.CommandWpf.RelayCommand<Boolean> OnSetGrpcService { get; private set; }
         public ICommand OnStartStreamService { get; private set; }
         public ICommand OnStopStreamService { get; private set; }


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
            OnSetGrpcService = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<bool>(OnExecuteGrpcService);
            OnStartStreamService = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteStartService);
            OnStopStreamService = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteStopService);

            _closingCommand = new ClosingCommand();
            

            // startup page
            CurrentView = Singleton<HomeViewModel>.Instance(_scopeFactory);
        }

        private void OnExecuteGrpcService(bool isChecked)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedGrpcService>();
                if (!isChecked)
                {
                    service.StartAsync(_stoppingCts.Token);
                }
                else
                {
                    service.StopAsync(_stoppingCts.Token);
                }
            }
        }
        private void OnExecuteStartService()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StartAsync(_stoppingCts.Token);
            }
        }
        private void OnExecuteStopService()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StopAsync(_stoppingCts.Token);
            }
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
