using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace wpfapp.ViewModel
{
    public class EventArgsConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {

            if (value is RoutedEventArgs routedEventArgs)
            {
                return (routedEventArgs, parameter);
            }
            return null;
            // Return a tuple with event args and window parameter } return null;
            //RoutedEventArgs? eventArgs = default;
            //if (value is RoutedEventArgs mouseEventArgs)
            //{
            //     eventArgs = mouseEventArgs;
            //}   
            ////// var additionalParam = parameter as MainWindow;
            //return (RoutedEventArgs: eventArgs, Parameter: parameter); //new Tuple<EventArgs,MainWindow>(eventArgs, additionalParam);
        }
    }
}
