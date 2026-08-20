using CommunityToolkit.Mvvm.ComponentModel;

namespace SensorsWpf.Models;

public partial class Point(double x = 0, double y = 0) : ObservableObject
{
    [ObservableProperty]
    public partial double X { get; set; } = x;
    [ObservableProperty]
    public partial double Y { get; set; } = y;
}
