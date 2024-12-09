using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wpfapp.Utilities
{
    public class CustomedCheckBox : CheckBox
    {
        private bool? _isChecked;

        public new bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value; OnIsCheckedChanged();
                    SetValue(IsCheckedProperty, value);
                }
            }
        }
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            this.IsChecked = !this.IsChecked;
            e.Handled = true;
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        protected void OnIsCheckedChanged()
        {

            if (IsChecked == true)
            { RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent)); }
            else if (IsChecked == false)
            { RaiseEvent(new RoutedEventArgs(CheckBox.UncheckedEvent)); }
        }

        protected override void OnClick()
        {
            base.OnClick();
        }
    }
}


