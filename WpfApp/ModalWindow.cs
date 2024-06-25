using System.Windows;

namespace MVVM
{

    public partial class ModalWindow : Window
    {
        public ModalWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }
    }
}
