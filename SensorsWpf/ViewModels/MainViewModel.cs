using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Devices.Sensors;
using SensorsWpf.Models;
using SensorsWpf.Enums;
using SensorsWpf.Services;

namespace SensorsWpf.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    public SensorData Activity { get; } = new("Activity");
    public SensorData Accelerometer { get; } = new("Accelerometer");
    public SensorData Altimeter { get; } = new("Altimeter");
    public SensorData Barometer { get; } = new("Barometer");
    public SensorData Compass { get; } = new("Compass");
    public SensorData Gyrometer { get; } = new("Gyrometer");
    public SensorData HingeAngle { get; } = new("Hinge Angle");
    public SensorData HumanPresence { get; } = new("Human Presence");
    public SensorData Inclinometer { get; } = new("Inclinometer");
    public SensorData Light { get; } = new("Light");
    public SensorData Magnetometer { get; } = new("Magnetometer");
    public SensorData Orientation { get; } = new("Orientation");
    public SensorData Pedometer { get; } = new("Pedometer");
    public SensorData Proximity { get; } = new("Proximity");
    public SensorData SimpleOrientation { get; } = new("Mode");

    public Point Ball { get; } = new Point();

    [ObservableProperty]
    public partial BallDataSource BallDataSource { get; set; } = BallDataSource.Accelerometer;

    public MainViewModel()
    {
        if (_sensors.Activity != null)
        {
            _sensors.Activity.ReadingChanged += Activity_ReadingChanged;
            Activity.Status = true;
        }
        if (_sensors.Accelerometer != null)
        {
            _sensors.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Status = true;
        }
        if (_sensors.Altimeter != null)
        {
            _sensors.Altimeter.ReadingChanged += Altimeter_ReadingChanged;
            Altimeter.Status = true;
        }
        if (_sensors.Barometer != null)
        {
            _sensors.Barometer.ReadingChanged += Barometer_ReadingChanged;
            Barometer.Status = true;
        }
        if (_sensors.Compass != null)
        {
            _sensors.Compass.ReadingChanged += Compass_ReadingChanged;
            Compass.Status = true;
        }
        if (_sensors.Gyrometer != null)
        {
            _sensors.Gyrometer.ReadingChanged += Gyrometer_ReadingChanged;
            Gyrometer.Status = true;
        }
        if (_sensors.HingeAngle != null)
        {
            _sensors.HingeAngle.ReadingChanged += HingeAngle_ReadingChanged;
            HingeAngle.Status = true;
        }
        if (_sensors.HumanPresence != null)
        {
            _sensors.HumanPresence.ReadingChanged += HumanPresence_ReadingChanged;
            HumanPresence.Status = true;
        }
        if (_sensors.Inclinometer != null)
        {
            _sensors.Inclinometer.ReadingChanged += Inclinometer_ReadingChanged;
            Inclinometer.Status = true;
        }
        if (_sensors.Light != null)
        {
            _sensors.Light.ReadingChanged += Light_ReadingChanged;
            Light.Status = true;
        }
        if (_sensors.Magnetometer != null)
        {
            _sensors.Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
            Magnetometer.Status = true;
        }
        if (_sensors.Orientation != null)
        {
            _sensors.Orientation.ReadingChanged += Orientation_ReadingChanged;
            Orientation.Status = true;
        }
        if (_sensors.Pedometer != null)
        {
            _sensors.Pedometer.ReadingChanged += Pedometer_ReadingChanged;
            Pedometer.Status = true;
        }
        if (_sensors.Proximity != null)
        {
            _sensors.Proximity.ReadingChanged += Proximity_ReadingChanged;
            Proximity.Status = true;
        }
        if (_sensors.SimpleOrientation != null)
        {
            _sensors.SimpleOrientation.OrientationChanged += SimpleOrientation_OrientationChanged;
            SimpleOrientation.Status = true;
        }
    }

    #region Internal

    SensorProvider _sensors = new();

    private void MoveBall(double x, double y)
    {
        Ball.X = x;
        Ball.Y = -y;
    }

    private void SimpleOrientation_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
    {
        var data = args.Orientation;
        SimpleOrientation.Info = $"{data}";
    }

    private void Activity_ReadingChanged(ActivitySensor sender, ActivitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Activity.Info = $"{data.Activity:F3}";
    }

    private void Compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Compass.Info = $"{data.HeadingTrueNorth:F3}°";
    }

    private void HingeAngle_ReadingChanged(HingeAngleSensor sender, HingeAngleSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        HingeAngle.Info = $"{data.AngleInDegrees:F3}°";
    }

    private void Accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Accelerometer.Info = $"X = {data.AccelerationX:F3} G, Y = {data.AccelerationY:F3} G, Z = {data.AccelerationZ:F3} G";
        if (BallDataSource == BallDataSource.Accelerometer)
            MoveBall(data.AccelerationX * 100, data.AccelerationY * 100);
    }

    private void Gyrometer_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Gyrometer.Info = $"X = {data.AngularVelocityX:F3}°/s, Y = {data.AngularVelocityY:F3}°/s, Z = {data.AngularVelocityY:F3}°/s";
        if (BallDataSource == BallDataSource.Gyrometer)
            MoveBall(data.AngularVelocityY * 1, data.AngularVelocityX * 1);
    }

    private void Inclinometer_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Inclinometer.Info = $"P = {data.PitchDegrees:F3}°, R = {data.RollDegrees:F3}°, Y = {data.YawDegrees:F3}°";
        if (BallDataSource == BallDataSource.Inclinometer)
            MoveBall(data.RollDegrees * 2, data.PitchDegrees * 2);
    }

    private void Orientation_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Orientation.Info = $"W = {data.Quaternion.W:F3}, X = {data.Quaternion.X:F3}, Y = {data.Quaternion.Y:F3}, Z = {data.Quaternion.Z:F3}";
        if (BallDataSource == BallDataSource.Orientation)
            MoveBall(data.Quaternion.X * 300, data.Quaternion.Y * 300);
    }

    private void Magnetometer_ReadingChanged(Magnetometer sender, MagnetometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Magnetometer.Info = $"X = {data.MagneticFieldX:F3}, Y = {data.MagneticFieldY:F3}, Z = {data.MagneticFieldZ:F3}";
        if (BallDataSource == BallDataSource.Magnetometer)
            MoveBall(data.MagneticFieldX * 3, data.MagneticFieldY * 3);
    }

    private void Pedometer_ReadingChanged(Pedometer sender, PedometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Pedometer.Info = $"{data.CumulativeStepsDuration}: {data.CumulativeSteps} ({data.StepKind})";
    }

    private void HumanPresence_ReadingChanged(HumanPresenceSensor sender, HumanPresenceSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        HumanPresence.Info = $"{data.Engagement}: {data.Presence} at {data.DistanceInMillimeters} mm";
    }

    private void Altimeter_ReadingChanged(Altimeter sender, AltimeterReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Altimeter.Info = $"{data.AltitudeChangeInMeters:F3} m";
    }

    private void Barometer_ReadingChanged(Barometer sender, BarometerReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Barometer.Info = $"{data.StationPressureInHectopascals:F3} hPa";
    }

    private void Light_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Light.Info = $"{data.IlluminanceInLux:F3}°";
    }

    private void Proximity_ReadingChanged(ProximitySensor sender, ProximitySensorReadingChangedEventArgs args)
    {
        var data = args.Reading;
        Proximity.Info = data.IsDetected ? $"{data.DistanceInMillimeters:F3} mm" : "-";
    }

    #endregion
}
