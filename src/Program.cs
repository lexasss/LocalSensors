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
            Printer.Print(
                "Activity",
                provider.Activity.GetCurrentReadingAsync(),
                data => $"{data.Activity}",
                1, 0);
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
        Console.CursorVisible = true;
    }

    private static void SimpleOrientation_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
    {
        var data = args.Orientation;
        var info = $"{data}";
        Printer.Print("Mode", info, 0, 0);
    }

    private static void Activity_ReadingChanged(ActivitySensor sender, ActivitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.Activity:F3}";
        Printer.Print("Activity", info, 1, 0);
    }

    private static void Compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.HeadingTrueNorth:F3}°";
        Printer.Print("Compass", info, 2, 0);
    }

    private static void HingeAngle_ReadingChanged(HingeAngleSensor sender, HingeAngleSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.AngleInDegrees:F3}°";
        Printer.Print("Hinge Angle", info, 3, 0);
    }

    private static void Accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.AccelerationX:F3} G, Y = {data.AccelerationY:F3} G, Z = {data.AccelerationZ:F3} G";
        Printer.Print("Accelerometer", info, 1);
    }

    private static void Gyrometer_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.AngularVelocityX:F3}°/s, Y = {data.AngularVelocityY:F3}°/s, Z = {data.AngularVelocityY:F3}°/s";
        Printer.Print("Gyrometer", info, 2);
    }

    private static void Inclinometer_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"P = {data.PitchDegrees:F3}°, R = {data.RollDegrees:F3}°, Y = {data.YawDegrees:F3}°";
        Printer.Print("Inclinometer", info, 3);
    }

    private static void Orientation_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"W = {data.Quaternion.W:F3}, X = {data.Quaternion.X:F3}, Y = {data.Quaternion.Y:F3}, Z = {data.Quaternion.Z:F3}";
        Printer.Print("Orientation", info, 4);
    }

    private static void Magnetometer_ReadingChanged(Magnetometer sender, MagnetometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"X = {data.MagneticFieldX:F3}, Y = {data.MagneticFieldY:F3}, Z = {data.MagneticFieldZ:F3}";
        Printer.Print("Magnetometer", info, 5);
    }

    private static void Pedometer_ReadingChanged(Pedometer sender, PedometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.CumulativeStepsDuration}: {data.CumulativeSteps} ({data.StepKind})";
        Printer.Print("Pedometer", info, 6);
    }

    private static void HumanPresence_ReadingChanged(HumanPresenceSensor sender, HumanPresenceSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.Engagement}: {data.Presence} at {data.DistanceInMillimeters} mm";
        Printer.Print("Human Presence", info, 7);
    }

    private static void Altimeter_ReadingChanged(Altimeter sender, AltimeterReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.AltitudeChangeInMeters:F3} m";
        Printer.Print("Altimeter", info, 0, 8);
    }

    private static void Barometer_ReadingChanged(Barometer sender, BarometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.StationPressureInHectopascals:F3} hPa";
        Printer.Print("Barometer", info, 1, 8);
    }

    private static void Light_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = $"{data.IlluminanceInLux:F3}°";
        Printer.Print("Light", info, 2, 8);
    }

    private static void Proximity_ReadingChanged(ProximitySensor sender, ProximitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        var info = data.IsDetected ? $"{data.DistanceInMillimeters:F3} mm" : "-";
        Printer.Print("Proximity", info, 3, 8);
    }
}