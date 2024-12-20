using System.ComponentModel;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace wpfapp.ViewModel
{
    public class CustomParameter :DependencyObject
    {    
        public EventArgs EventArgs1
        {
            get { return (EventArgs)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("EventArgs1", typeof(EventArgs), typeof(CustomParameter));


        public Window MainWindow1
        {
            get { return (Window)GetValue(AdditionalParameterProperty); }
            set => SetValue(AdditionalParameterProperty, value);
        }

        public static readonly DependencyProperty AdditionalParameterProperty =
            DependencyProperty.Register("MainWindow1", typeof(Window), typeof(CustomParameter), new PropertyMetadata(default(Window)));


    }

    public class CustomEventArgs : CancelEventArgs
    {
        public string CustomProperty { get; set; }

        public CustomEventArgs(string customProperty)
        {
            CustomProperty = customProperty;
        }
        public CustomEventArgs()
        {
            
        }

    }
}
