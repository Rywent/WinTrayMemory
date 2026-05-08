using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WinTrayMemory.Resources.Converters;

public class RamSliderValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double val = (double)values[0];
        double totalWidth = (double)values[1];
        double max = (double)values[2];
        return (val / max) * totalWidth;
    }
    public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null;
    }
}
