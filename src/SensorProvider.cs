using System.Reflection;
using Windows.Devices.Sensors;
using Windows.Foundation;

namespace Sensors;

[AttributeUsage(AttributeTargets.Property)]
internal class SensorAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

internal class SensorProvider
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
        T? GetSync<T>()
        {
            var method = typeof(T).GetMethod("GetDefaultAsync", BindingFlags.Public | BindingFlags.Static);
            if (method == null)
                return default(T);
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

    /*
    public void Print()
    {
        string[] ListDevices(string? selector, string? pattern = null)
        {
            List<string> ids = [];

            var task = selector == null
                ? DeviceInformation.FindAllAsync()
                : DeviceInformation.FindAllAsync(selector);
            task.Wait();
            var collection = task.GetResults();
            foreach (var item in collection)
            {
                if (!item.Name.StartsWith("Sound") &&
                    !item.Name.StartsWith("WKS") &&
                    !item.Name.StartsWith("Oleg") &&
                    !item.Name.StartsWith("ACX"))
                    Console.WriteLine($"{item.Name}  [{item.IsDefault}]");
                if (pattern != null && item.Name.Contains(pattern))
                {
                    Console.WriteLine(item.Id);
                    ids.Add(item.Id);
                    foreach (var prop in item.Properties)
                        Console.WriteLine($"  {prop}");
                }
            }

            return ids.ToArray();
        }

        ListDevices(ProximitySensor.GetDeviceSelector());
        ListDevices(Accelerometer.GetDeviceSelector(AccelerometerReadingType.Standard));
        ListDevices(ActivitySensor.GetDeviceSelector());
        ListDevices(Barometer.GetDeviceSelector());
        ListDevices(Compass.GetDeviceSelector());
        ListDevices(Gyrometer.GetDeviceSelector());
        ListDevices(Inclinometer.GetDeviceSelector(SensorReadingType.Absolute));
        ListDevices(LightSensor.GetDeviceSelector());
        ListDevices(Magnetometer.GetDeviceSelector());
        ListDevices(Pedometer.GetDeviceSelector());

        Task.Run(async () =>
        {
            string aqs = SerialDevice.GetDeviceSelector("COM4");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            if (dis.Count > 0)
            {
                DeviceInformation uart = dis[0];
                Console.WriteLine($"{uart.Id}");

                try
                {
                    var serialPort = await SerialDevice.FromIdAsync(uart.Id);
                } catch { }
            }

            var ids = ListDevices(null, "Accel");
            foreach (var id in ids)
            {
                try
                {
                    var acc = Accelerometer.FromIdAsync(id);
                }
                catch
                {
                    Console.WriteLine($"Accelerometer {id} cannot be created");
                }
            }
        }).Wait();
    }*/
    }
