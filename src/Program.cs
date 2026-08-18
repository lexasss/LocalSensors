using Windows.Devices.Sensors;

namespace Sensors;

class Program
{
    static async Task Main()
    {
        var provider = new SensorProvider();

        foreach (var sensor in provider.Sensors)
            Console.WriteLine($"[{(sensor.IsAvailable ? '+' : '-')}] {sensor.Name}");

        /*
        var accelerometer = provider.Accelerometer;
        if (accelerometer == null)
        {
            Console.WriteLine("No accelerometer detected.");
            return;
        }

        Console.WriteLine("Accelerometer found!");
        Console.WriteLine($"Minimum interval: {accelerometer.MinimumReportInterval} ms");

        accelerometer.ReportInterval =
            Math.Max(20u, accelerometer.MinimumReportInterval);

        accelerometer.ReadingChanged += Accelerometer_ReadingChanged;

        Console.WriteLine("Reading accelerometer...");
        */

        Console.WriteLine("Press Enter to exit.");

        Console.ReadLine();
    }

    private static void Accelerometer_ReadingChanged(
        Accelerometer sender,
        AccelerometerReadingChangedEventArgs args)
    {
        var reading = args.Reading;
        var info = $"X={reading.AccelerationX:F3} G, " +
            $"Y={reading.AccelerationY:F3} G, " +
            $"Z={reading.AccelerationZ:F3} G";
        info.PadRight(80);
        
        Console.CursorLeft = 0;
        Console.Write(info);
    }
}