using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WinTrayMemory.Resources.Converters;

public class RamColorConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is double value && values[1] is double maximum && maximum > 0)
        {
            double percentage = (value / maximum) * 100;

            if (percentage < 50)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B7FFF"));
            else if (percentage < 75)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));
        }

        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B7FFF"));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
