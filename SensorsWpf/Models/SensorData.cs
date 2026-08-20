using CommunityToolkit.Mvvm.ComponentModel;

namespace SensorsWpf.Models;

public partial class SensorData(string name, bool status = false, string info = "") : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = name;
    [ObservableProperty]
    public partial bool Status { get; set; } = status;
    [ObservableProperty]
    public partial string Info { get; set; } = info;
}
