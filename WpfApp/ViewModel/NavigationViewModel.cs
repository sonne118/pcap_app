using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfapp.Utilities;
using System.Windows.Input;
using wpfapp.View;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;
using MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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


        //public object CurrentView
        //{
        //    get { return _currentView; }
        //    set { _currentView = value; OnPropertyChanged(); }
        //}
        public object CurrentView
        {
            get => _currentView;
            set
            {
                Set(ref _currentView, value);
                //{
                //if (_currentView != null)
                //    _currentView.IsSelected = true;
                //_currentView.Add(value);
                //}
            }
        }

        public IBackgroundJobs<Snapshot> BackgroundJobs
        {
            get { return _backgroundJobs; }
            set { _backgroundJobs = value; }
        }

        public IMapper Mapper
        {
            get { return _mapper; }
            set { _mapper = value; }
        }

        public IDevices Devices
        {
            get { return _devices; }
            set { _devices = value; }
        }

        public IPutDevice PutDevice
        {
            get { return _putDevice; }
            set { _putDevice = value; }
        }

        public IServiceScopeFactory ScopeFactory
        {
            get { return _scopeFactory; }
            set { _scopeFactory = value; }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DashboardCommand { get; set; }
        public ICommand FileCommand { get; set; }
        public ICommand TreeCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

       
        private void Home(object obj) => CurrentView = new HomeViewModel(BackgroundJobs, Devices, Mapper, ScopeFactory);  //, this

        private void Dashboard(object obj) => CurrentView = new DashboardViewModel();
        private void Tree(object obj) => CurrentView = new TreeViewModel();
        private void File(object obj) => CurrentView = new FileViewModel();
        private void Settings(object obj) => CurrentView = new SettitngsViewModel();

        public NavigationViewModel(
                       IBackgroundJobs<Snapshot> backgroundJobs,
                       IDevices devices,
                       IPutDevice putDevice,
                       IMapper mapper,
                       IServiceScopeFactory scopeFactory) : base(devices)
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

            //// Startup Page
            CurrentView = new HomeViewModel(BackgroundJobs, Devices, Mapper, ScopeFactory);
        }

        public override void SetDevice(string str)
        {
            ///////// using (var scope = _scopeFactory.CreateScope())
            {

                //var service = ServiceLocator.Current.GetInstance<IPutDevice>();
                //////var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                Int32.TryParse(str?.Substring(0, 1), out var dev);
                _putDevice.PutDevices(dev);
                //service.PutDevices(dev);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
