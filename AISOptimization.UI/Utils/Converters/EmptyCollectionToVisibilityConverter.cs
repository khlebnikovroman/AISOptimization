using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace AISOptimization.Utils;

public class EmptyCollectionToVisibilityConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ICollection values)
        {
            if (values.Count==0)
            {
                return Visibility.Collapsed;
            }
        }

        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
