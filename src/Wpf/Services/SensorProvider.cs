using System.Reflection;
using Windows.Devices.Sensors;
using Windows.Foundation;

namespace SensorsWpf.Services;

[AttributeUsage(AttributeTargets.Property)]
internal class SensorAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

public class SensorProvider
{
    [Sensor("Activity")]
    public ActivitySensor? Activity { get; }
    [Sensor("Accelerometer")]
    public Accelerometer? Accelerometer { get; }
    [Sensor("Altimeter")]
    public Altimeter? Altimeter { get; }
    [Sensor("Barometer")]
    public Barometer? Barometer { get; }
    [Sensor("Compass")]
    public Compass? Compass { get; }
    [Sensor("Gyrometer")]
    public Gyrometer? Gyrometer { get; }
    [Sensor("HingeAngle")]
    public HingeAngleSensor? HingeAngle { get; }
    [Sensor("HumanPresence")]
    public HumanPresenceSensor? HumanPresence { get; }
    [Sensor("Inclinometer")]
    public Inclinometer? Inclinometer { get; }
    [Sensor("Light")]
    public LightSensor? Light { get; }
    [Sensor("Magnetometer")]
    public Magnetometer? Magnetometer { get; }
    [Sensor("Orientation")]
    public OrientationSensor? Orientation { get; }
    [Sensor("Pedometer")]
    public Pedometer? Pedometer { get; }
    [Sensor("Proximity")]
    public ProximitySensor? Proximity { get; }
    [Sensor("SimpleOrientation")]
    public SimpleOrientationSensor? SimpleOrientation { get; }

    public record SensorStatus(string Name, bool IsAvailable);

    public SensorStatus[] Sensors => typeof(SensorProvider)
        .GetProperties()
        .Select(p => new
        {
            Property = p,
            Attribute = p.GetCustomAttribute<SensorAttribute>()
        })
        .Where(x => x.Attribute != null)
        .Select(x => new SensorStatus(x.Attribute!.Name, x.Property.GetValue(this) != null))
        .ToArray();


    public SensorProvider()
    {
        static T? GetSync<T>()
        {
            var method = typeof(T).GetMethod("GetDefaultAsync", BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                return default(T);
            }
            try
            {
                var task = (IAsyncOperation<T>?)method.Invoke(null, null);
                task.Wait();
                return task!.GetResults();
            }
            catch
            {
                return default(T);
            }
        }

        Activity = GetSync<ActivitySensor>();
        Accelerometer = Accelerometer.GetDefault();
        Altimeter = Altimeter.GetDefault();
        Barometer = Barometer.GetDefault();
        Compass = Compass.GetDefault();
        Gyrometer = Gyrometer.GetDefault();
        HingeAngle = GetSync<HingeAngleSensor>();
        HumanPresence = HumanPresenceSensor.GetDefault();
        Inclinometer = Inclinometer.GetDefault();
        Light = LightSensor.GetDefault();
        Magnetometer = Magnetometer.GetDefault();
        Orientation = OrientationSensor.GetDefault();
        Pedometer = GetSync<Pedometer>();
        SimpleOrientation = SimpleOrientationSensor.GetDefault();

        Proximity = null;
    }
}
