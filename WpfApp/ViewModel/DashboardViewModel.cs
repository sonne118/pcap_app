using System.Diagnostics;
using wpfapp.Infrastructure.ViewModels;

namespace wpfapp.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
        {
            Debug.WriteLine("DashboardViewModel initialized");
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
