using SensorsWpf.Enums;

namespace SensorsWpf.Models;

public class MainSettings
{
    public BallDataSource DataSource { get; set; } = BallDataSource.Accelerometer;
}
