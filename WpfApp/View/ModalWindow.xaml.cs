using System.Windows;

namespace wpfapp.View
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