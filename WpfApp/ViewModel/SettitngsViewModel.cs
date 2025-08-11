using System.ComponentModel;
using System.Diagnostics;
using wpfapp.Infrastructure.ViewModels;

namespace wpfapp.ViewModel
{
    public class SettitngsViewModel : BaseViewModel
    {
        public SettitngsViewModel()
        {
            Debug.WriteLine("SettitngsViewModel");
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
