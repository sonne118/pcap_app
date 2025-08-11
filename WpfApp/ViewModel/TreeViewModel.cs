using System.ComponentModel;
using System.Diagnostics;
using wpfapp.Infrastructure.ViewModels;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public class TreeViewModel : BaseViewModel
    {
        private readonly IDevices _devices;
        public TreeViewModel()
        {
            Debug.WriteLine("TheeViewModel");
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
