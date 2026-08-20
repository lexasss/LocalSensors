using System.Globalization;
using System.Windows.Data;

namespace SensorsWpf.Converters;

internal class BallDataSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        ((Enum)value)?.HasFlag((Enum)parameter) == true;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value.Equals(true) == true ? parameter : Binding.DoNothing;
}
