using System.ComponentModel;
using System.Diagnostics;
using wpfapp.Infrastructure.ViewModels;

namespace wpfapp.ViewModel
{
    public class FileViewModel : BaseViewModel
    {
        public FileViewModel()
        {
            Debug.WriteLine("FileViewModel");
        }

        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing);
        }
    }
}
