using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SensorsWpf.Converters;

internal class AbsoluteToRelativeXConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var x = (double)value;
        double dx = 0;
        var canvas = parameter as Canvas;
        if (canvas != null)
        {
            dx = canvas.ActualWidth / 2;
        }
        return x - 5 + dx;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}

internal class AbsoluteToRelativeYConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var y = (double)value;
        double dy = 0;
        var canvas = parameter as Canvas;
        if (canvas != null)
        {
            dy = canvas.ActualHeight / 2;
        }
        return y - 5 + dy;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
