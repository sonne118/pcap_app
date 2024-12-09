using System.Windows;
using System.Windows.Controls;
using wpfapp.Utilities;

namespace wpfapp.View
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void CheckBox_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            MessageBox.Show("CheckBox right-clicked!");
            //CustomedCheckBox checkBox = sender as CustomedCheckBox;
            //if (checkBox != null)
            //{
            //   // checkBox.IsChecked = !checkBox.IsChecked;
            //}
        }
    }
}
