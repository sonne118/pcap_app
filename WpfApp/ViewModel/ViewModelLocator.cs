using AutoMapper;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommonServiceLocator;

namespace wpfapp.ViewModel
{
    public class ViewModelLocator
    {
        //private readonly IUnityContainer _container;
        //public NavigationViewModel NavigationViewModel => WpfApp.App.AppHost.Services.GetRequiredService<NavigationViewModel>();
        public NavigationViewModel NavigationViewModel => wpfapp.App.AppHost.Services.GetRequiredService<NavigationViewModel>();
        //public Home Home => menu_wpf.App.AppHost.Services.GetRequiredService<Home>();

        // public IBackgroundJobs<Snapshot> _backgroundJobs; //=> WpfApp.App.AppHost.Services.GetRequiredService<IBackgroundJobs<Snapshot>>();
        public ViewModelLocator()
        {

            //var mapperConfig = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<AppMappingProfile>();
            //});

            //IMapper mapper = mapperConfig.CreateMapper();
            // var serviceProvider = ServiceProviderFactory.CreateServiceProvider();

            //var factory = WpfApp.App.AppHost.Services;//.CreateScope// GetRequiredService<IServiceScopeFactory>();
            //using (var scope = factory.CreateScope())
            //{
            //   var backgroundJobs = scope.ServiceProvider.GetRequiredService<IStreamData>();
            //    //service.DoYourMagic();
            //}



            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //SimpleIoc.Default.Register<IBackgroundJobs<Snapshot>, BackgroundJobs>();
            //SimpleIoc.Default.Register<IStreamData, StreamData>();
            //SimpleIoc.Default.Register<IDevices, Devices>();
            //SimpleIoc.Default.Register<IPutDevice, PutDevice>();

            //SimpleIoc.Default.Register(() => mapper);
            //SimpleIoc.Default.Register(()=> serviceProvider);

            //SimpleIoc.Default.Register<IPutDevice, PutDevice>();

            //  SimpleIoc.Default.Register<NavigationViewModel>();
            //SimpleIoc.Default.Register(()=> WpfApp.App.AppHost.Services.GetRequiredService<NavigationViewModel>());


            //SimpleIoc.Default.Register(() => WpfApp.App.AppHost.Services.GetRequiredService<IBackgroundJobs<Snapshot>>());
            //var _backgroundJobs = WpfApp.App.AppHost.Services.GetRequiredService<BackgroundJobs>();

            /////SimpleIoc.Default.Register<System.Windows.Window>(() => System.Windows.Application.Current.MainWindow as MainWindow);
            SimpleIoc.Default.Register(() => wpfapp.App.AppHost.Services.GetRequiredService<NavigationViewModel>());

            //SimpleIoc.Default.Register(() => menu_wpf.App.AppHost.Services.GetRequiredService<Home>());
            //SimpleIoc.Default.Register(() => new NavigationViewModel(
            //SimpleIoc.Default.GetInstance<IBackgroundJobs<Snapshot>>(),
            //SimpleIoc.Default.GetInstance<IDevices>(),
            //SimpleIoc.Default.GetInstance<IPutDevice>()
            //));



            //SimpleIoc.Default.Register(() => new Worker(
            //SimpleIoc.Default.GetInstance<IStreamData>(),
            //SimpleIoc.Default.GetInstance<IBackgroundJobs<Snapshot>>(),
            //SimpleIoc.Default.GetInstance<IServiceScopeFactory>()));
        }


        public NavigationViewModel Main
        {
            get { return SimpleIoc.Default.GetInstance<NavigationViewModel>(); }
        }
        //mapper using automapper
        //var mapperConfig = new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile<AppMappingProfile>();
        //});

        //IMapper mapper = mapperConfig.CreateMapper();

        ////ServiceScopeFactory using microsoft dependency
        // var serviceProvider = ServiceProviderFactory.CreateServiceProvider();

        //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        //SimpleIoc.Default.Register<IBackgroundJobs<Snapshot>, BackgroundJobs>();
        //SimpleIoc.Default.Register<IStreamData, StreamData>();
        //SimpleIoc.Default.Register<IDevices, Devices>();
        //SimpleIoc.Default.Register(() => mapper);
        //SimpleIoc.Default.Register(()=> serviceProvider);

        //SimpleIoc.Default.Register<IPutDevice, PutDevice>();


        //SimpleIoc.Default.Register<IStreamData, StreamData>();
        //SimpleIoc.Default.Register<IHostedService, StartService>();

        //SimpleIoc.Default.Register(() => new Worker(
        //SimpleIoc.Default.GetInstance<IStreamData>(),
        //SimpleIoc.Default.GetInstance<IBackgroundJobs<Snapshot>>(),
        //SimpleIoc.Default.GetInstance<IServiceScopeFactory>()));




        ////SimpleIoc.Default.GetInstance<IWorker>();
        //SimpleIoc.Default.GetInstance<IStreamData>();
        //SimpleIoc.Default.GetInstance<IHostedService>();





        //SimpleIoc.Default.Register(()=> serviceProvider.GetRequiredService<IServiceScopeFactory>());
        ////SimpleIoc.Default.Register(() => serviceProvider.GetRequiredService<IPutDevice,PutDevice>());

        //SimpleIoc.Default.Register(() => new NavigationViewModel(
        //SimpleIoc.Default.GetInstance<IBackgroundJobs<Snapshot>>(),
        //SimpleIoc.Default.GetInstance<IDevices>(),
        //SimpleIoc.Default.GetInstance<IPutDevice>()
        //));
        //    //SimpleIoc.Default.GetInstance <IMapper>(),
        //    //SimpleIoc.Default.GetInstance <IServiceScopeFactory>()
        //    ));


        ///public NavigationViewModel NavigationViewModel => ServiceLocator.Current.GetInstance<NavigationViewModel>();
        //public Worker Worker => ServiceLocator.Current.GetInstance<Worker>();
    }

    //public static class ServiceProviderFactory
    //{
    //    public static IServiceProvider CreateServiceProvider()
    //    {
    //        var serviceCollection = new ServiceCollection();
    //        //serviceCollection.AddScoped<IPutDevice>();
    //        return serviceCollection.BuildServiceProvider();
    //    }
    //}
}


