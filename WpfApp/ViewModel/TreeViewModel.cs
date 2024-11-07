using MVVM;
using System.Diagnostics;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public class TreeViewModel //: GridViewModel
    {

        private readonly IDevices _devices;
        public TreeViewModel()
        {

            Debug.WriteLine("TheeViewModel");
        }
    }
}
