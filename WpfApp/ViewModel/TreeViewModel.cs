using System.Diagnostics;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public class TreeViewModel
    {

        private readonly IDevices _devices;
        public TreeViewModel()
        {
            Debug.WriteLine("TheeViewModel");
        }
    }
}
