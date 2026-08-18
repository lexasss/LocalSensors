using Windows.Devices.Sensors;
using Windows.Foundation;

namespace Sensors;

class Program
{
    const int TABLE_LEFT = 30;
    const int TABLE_TOP = 1;
    const int TABLE_CELL_WIDTH = 15;
    const int TABLE_CELL_HEIGHT = 3;

    readonly static Lock _locker = new();

    static async Task Main()
    {
        var provider = new SensorProvider();

        foreach (var sensor in provider.Sensors)
            Console.WriteLine($"[{(sensor.IsAvailable ? '+' : '-')}] {sensor.Name}");

        Console.WriteLine("\nPress Enter to exit.");
        var top = Console.CursorTop;

        if (provider.Activity != null)
        {
            provider.Activity.ReadingChanged += Activity_ReadingChanged;
            PrintAt(TABLE_LEFT + TABLE_CELL_WIDTH, TABLE_TOP, "Activity", provider.Activity.GetCurrentReadingAsync(), data => $"{data.Activity}");
        }
        if (provider.Accelerometer != null)
            provider.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        if (provider.Altimeter != null)
            provider.Altimeter.ReadingChanged += Altimeter_ReadingChanged;
        if (provider.Barometer != null)
            provider.Barometer.ReadingChanged += Barometer_ReadingChanged;
        if (provider.Compass != null)
            provider.Compass.ReadingChanged += Compass_ReadingChanged;
        if (provider.Gyrometer != null)
            provider.Gyrometer.ReadingChanged += Gyrometer_ReadingChanged;
        if (provider.HingeAngle != null)
            provider.HingeAngle.ReadingChanged += HingeAngle_ReadingChanged;
        if (provider.HumanPresence != null)
            provider.HumanPresence.ReadingChanged += HumanPresence_ReadingChanged;
        if (provider.Inclinometer != null)
            provider.Inclinometer.ReadingChanged += Inclinometer_ReadingChanged;
        if (provider.Light != null)
            provider.Light.ReadingChanged += Light_ReadingChanged;
        if (provider.Magnetometer != null)
            provider.Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
        if (provider.Orientation != null)
            provider.Orientation.ReadingChanged += Orientation_ReadingChanged;
        if (provider.Pedometer != null)
            provider.Pedometer.ReadingChanged += Pedometer_ReadingChanged;
        if (provider.Proximity != null)
            provider.Proximity.ReadingChanged += Proximity_ReadingChanged;
        if (provider.SimpleOrientation != null)
            provider.SimpleOrientation.OrientationChanged += SimpleOrientation_OrientationChanged;

        Console.CursorVisible = false;
        Console.ReadLine();

        Console.CursorTop = top;
    }

    private static void SimpleOrientation_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
    {
        var data = args.Orientation;
        var info = $"{data}";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP, "Mode", info.PadRight(15));
    }

    private static void Activity_ReadingChanged(ActivitySensor sender, ActivitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.Activity:F3}";
        PrintAt(
            TABLE_LEFT + TABLE_CELL_WIDTH,
            TABLE_TOP,
            "Activity", info.PadRight(15));
    }

    private static void Accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.AccelerationX:F3} G, Y = {data.AccelerationY:F3} G, Z = {data.AccelerationZ:F3} G";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 1 * TABLE_CELL_HEIGHT,
            "Accelerometer", info.PadRight(40));
    }

    private static void Gyrometer_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.AngularVelocityX:F3}°/s, Y = {data.AngularVelocityY:F3}°/s, Z = {data.AngularVelocityY:F3}°/s";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 2 * TABLE_CELL_HEIGHT,
            "Gyrometer", info.PadRight(45));
    }

    private static void Inclinometer_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"P = {data.PitchDegrees:F3}°, R = {data.RollDegrees:F3}°, Y = {data.YawDegrees:F3}°";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 3 * TABLE_CELL_HEIGHT,
            "Inclinometer", info.PadRight(40));
    }

    private static void Orientation_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"W = {data.Quaternion.W:F3}, X = {data.Quaternion.X:F3}, Y = {data.Quaternion.Y:F3}, Z = {data.Quaternion.Z:F3}";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 4 * TABLE_CELL_HEIGHT,
            "Orientation", info.PadRight(45));
    }

    private static void Altimeter_ReadingChanged(Altimeter sender, AltimeterReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.AltitudeChangeInMeters:F3} m";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 5 * TABLE_CELL_HEIGHT,
            "Altimeter", info.PadRight(15));
    }

    private static void Barometer_ReadingChanged(Barometer sender, BarometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.StationPressureInHectopascals:F3} hPa";
        PrintAt(
            TABLE_LEFT + TABLE_CELL_WIDTH,
            TABLE_TOP + 5 * TABLE_CELL_HEIGHT,
            "Barometer", info.PadRight(15));
    }

    private static void Compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.HeadingTrueNorth:F3}°";
        PrintAt(
            TABLE_LEFT + 2 * TABLE_CELL_WIDTH,
            TABLE_TOP + 5 * TABLE_CELL_HEIGHT,
            "Compass", info.PadRight(15));
    }

    private static void HingeAngle_ReadingChanged(HingeAngleSensor sender, HingeAngleSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.AngleInDegrees:F3}°";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 6 * TABLE_CELL_HEIGHT,
            "Hinge Angle", info.PadRight(15));
    }

    private static void Light_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.IlluminanceInLux:F3}°";
        PrintAt(
            TABLE_LEFT + 1 * TABLE_CELL_WIDTH,
            TABLE_TOP + 6 * TABLE_CELL_HEIGHT,
            "Hinge Angle", info.PadRight(15));
    }

    private static void Proximity_ReadingChanged(ProximitySensor sender, ProximitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = data.IsDetected ? $"{data.DistanceInMillimeters:F3} mm" : "-";
        PrintAt(
            TABLE_LEFT + 2 * TABLE_CELL_WIDTH,
            TABLE_TOP + 6 * TABLE_CELL_HEIGHT,
            "Proximity", info.PadRight(15));
    }

    private static void HumanPresence_ReadingChanged(HumanPresenceSensor sender, HumanPresenceSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.Engagement}: {data.Presence} at {data.DistanceInMillimeters} mm";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 7 * TABLE_CELL_HEIGHT,
            "Human Presence", info.PadRight(45));
    }

    private static void Magnetometer_ReadingChanged(Magnetometer sender, MagnetometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.MagneticFieldX:F3}, Y = {data.MagneticFieldY:F3}, Z = {data.MagneticFieldZ:F3}";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 8 * TABLE_CELL_HEIGHT,
            "Magnetometer", info.PadRight(45));
    }

    private static void Pedometer_ReadingChanged(Pedometer sender, PedometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.CumulativeStepsDuration}: {data.CumulativeSteps} ({data.StepKind})";
        PrintAt(
            TABLE_LEFT,
            TABLE_TOP + 9 * TABLE_CELL_HEIGHT,
            "Pedometer", info.PadRight(45));
    }

    private static void PrintAt<T>(int left, int top, string name, IAsyncOperation<T> task, Func<T, string> toString)
    {
        _locker.Enter();

        task.Wait();
        var info = toString.Invoke(task.GetResults());
        PrintAt(left, top, name, info.PadRight(15));

        _locker.Exit();
    }


    private static void PrintAt(int left, int top, string name, string info)
    {
        _locker.Enter();

        Console.CursorLeft = left;
        Console.CursorTop = top;
        Console.Write(name);
        Console.CursorLeft = left;
        Console.CursorTop = top + 1;
        Console.Write(info);

        _locker.Exit();
    }
}