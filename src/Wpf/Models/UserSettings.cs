using SensorsWpf.Enums;

namespace SensorsWpf.Models;

public class UserSettings
{
    public BallDataSource DataSource { get; set; } = BallDataSource.Accelerometer;
}
