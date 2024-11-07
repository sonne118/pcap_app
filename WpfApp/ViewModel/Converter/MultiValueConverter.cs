using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace wpfapp.ViewModel
{
    internal class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (values[0] as RoutedEventArgs, values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
