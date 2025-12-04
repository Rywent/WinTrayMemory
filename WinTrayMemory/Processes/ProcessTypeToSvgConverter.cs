using System;
using System.Globalization;
using System.Windows.Data;
using static WinTrayMemory.Processes.DeterminingProcessType;

namespace WinTrayMemory.Processes;

public sealed class ProcessTypeToSvgConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ProcessType type)
            return null;

        return type switch
        {
            ProcessType.Safely => "pack://application:,,,/Resources/img/safely.svg",
            ProcessType.Warning => "pack://application:,,,/Resources/img/warning.svg",
            ProcessType.Dangerous => "pack://application:,,,/Resources/img/dangerous.svg",
            ProcessType.Unknown => "pack://application:,,,/Resources/img/unknown.svg",
            _ => null
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
