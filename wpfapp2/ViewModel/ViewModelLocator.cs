using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace wpfapp.ViewModel
{
    public class ViewModelLocator
    {
        public NavigationViewModel NavigationViewModel => App.AppHost.Services.GetRequiredService<NavigationViewModel>();
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => App.AppHost.Services.GetRequiredService<NavigationViewModel>());
        }

        public NavigationViewModel Main
        {
            get { return SimpleIoc.Default.GetInstance<NavigationViewModel>(); }
        }
    }
}


