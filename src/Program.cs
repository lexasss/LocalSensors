using Windows.Devices.Sensors;

namespace Sensors;

class Program
{
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
            var task = provider.Activity.GetCurrentReadingAsync();
            task.Wait();
            var data = task.GetResults();
            var info = $"{data.Activity}";
            PrintAt(70, 13, "Activity", info.PadRight(15));
        }
        if (provider.Accelerometer != null)
            provider.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        if (provider.Gyrometer != null)
            provider.Gyrometer.ReadingChanged += Gyrometer_ReadingChanged;
        if (provider.Inclinometer != null)
            provider.Inclinometer.ReadingChanged += Inclinometer_ReadingChanged;
        if (provider.Orientation != null)
            provider.Orientation.ReadingChanged += Orientation_ReadingChanged;
        if (provider.SimpleOrientation != null)
            provider.SimpleOrientation.OrientationChanged += SimpleOrientation_OrientationChanged;

        Console.CursorVisible = false;
        Console.ReadLine();

        Console.CursorTop = top;
    }

    private static void Activity_ReadingChanged(ActivitySensor sender, ActivitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.Activity:F3}";
        PrintAt(70, 13, "Activity", info.PadRight(15));
    }

    private static void SimpleOrientation_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
    {
        var data = args.Orientation;
        var info = $"{data}";
        PrintAt(50, 13, "Mode", info.PadRight(15));
    }

    private static void Gyrometer_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.AngularVelocityX:F3}°/s, Y = {data.AngularVelocityY:F3}°/s, Z = {data.AngularVelocityY:F3}°/s";
        PrintAt(50, 10, "Gyrometer", info.PadRight(45));
    }

    private static void Orientation_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"W = {data.Quaternion.W:F3}, X = {data.Quaternion.X:F3}, Y = {data.Quaternion.Y:F3}, Z = {data.Quaternion.Z:F3}";
        PrintAt(50, 7, "Orientation", info.PadRight(45));
    }

    private static void Inclinometer_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"P = {data.PitchDegrees:F3}°, R = {data.RollDegrees:F3}°, Y = {data.YawDegrees:F3}°";
        PrintAt(50, 4, "Inclinometer", info.PadRight(40));
    }

    private static void Accelerometer_ReadingChanged(
        Accelerometer sender,
        AccelerometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.AccelerationX:F3} G, Y = {data.AccelerationY:F3} G, Z = {data.AccelerationZ:F3} G";
        PrintAt(50, 1, "Acelerometer", info.PadRight(40));
    }

    readonly static Lock _locker = new();

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