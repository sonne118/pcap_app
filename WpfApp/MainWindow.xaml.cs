using System.Windows;
using WpfApp.Services.Helpers;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel(new AccumulateData());
        }
    }
}