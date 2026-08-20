using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SensorsWpf;

internal class StatusToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? Brushes.DarkGreen : Brushes.Red;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        ((Brush)value) == Brushes.DarkGreen;
}
