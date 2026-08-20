using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SensorsWpf;

internal class AbsoluteToRelativeXConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var canvas = parameter as Canvas;
        double dx = 0;
        if (canvas != null)
        {
            dx = canvas.ActualWidth / 2;
        }
        return -5 + dx;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}

internal class AbsoluteToRelativeYConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var canvas = parameter as Canvas;
        double dy = 0;
        if (canvas != null)
        {
            dy = canvas.ActualHeight / 2;
        }
        return -5 + dy;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
