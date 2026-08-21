using CommunityToolkit.Mvvm.ComponentModel;
using SensorsWpf.Enums;
using SensorsWpf.Models;
using SensorsWpf.Services;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Sensors;
using Windows.Foundation;

namespace SensorsWpf.ViewModels;

public partial class MainViewModel : ObservableObject
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

    public BallPosition Ball { get; } = new BallPosition();
    public double BallSize { get; } = 10;

    [ObservableProperty]
    public partial BallDataSource BallDataSource { get; set; } = BallDataSource.Accelerometer;

    public MainViewModel(SensorProvider sensors)
    {
        _dispatcher = Application.Current.Dispatcher;

        Register<ActivitySensor, ActivitySensorReadingChangedEventArgs>(Activity, sensors.Activity,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.Activity}");

        Register<Accelerometer, AccelerometerReadingChangedEventArgs>(Accelerometer, sensors.Accelerometer,
            (s, h) => s.ReadingChanged += h,
            r => $"X = {r.Reading.AccelerationX,6:F3} G, Y = {r.Reading.AccelerationY,6:F3} G, Z = {r.Reading.AccelerationZ,6:F3} G",
            r => MoveBall(BallDataSource.Accelerometer, r.Reading.AccelerationX, r.Reading.AccelerationY));

        Register<Altimeter, AltimeterReadingChangedEventArgs>(Altimeter, sensors.Altimeter,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.AltitudeChangeInMeters:F3} m");

        Register<Barometer, BarometerReadingChangedEventArgs>(Barometer, sensors.Barometer,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.StationPressureInHectopascals:F3} hPa");

        Register<Compass, CompassReadingChangedEventArgs>(Compass, sensors.Compass,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.HeadingTrueNorth:F3}°");

        Register<Gyrometer, GyrometerReadingChangedEventArgs>(Gyrometer, sensors.Gyrometer,
            (s, h) => s.ReadingChanged += h,
            r => $"X = {r.Reading.AngularVelocityX,7:F3}°/s, Y = {r.Reading.AngularVelocityY,7:F3}°/s, Z = {r.Reading.AngularVelocityZ,7:F3}°/s",
            r => MoveBall(BallDataSource.Gyrometer, r.Reading.AngularVelocityY, r.Reading.AngularVelocityX));

        Register<HingeAngleSensor, HingeAngleSensorReadingChangedEventArgs>(HingeAngle, sensors.HingeAngle,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.AngleInDegrees:F3}°");

        Register<HumanPresenceSensor, HumanPresenceSensorReadingChangedEventArgs>(HumanPresence, sensors.HumanPresence,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.Engagement}: {r.Reading.Presence} at {r.Reading.DistanceInMillimeters} mm");

        Register<Inclinometer, InclinometerReadingChangedEventArgs>(Inclinometer, sensors.Inclinometer,
            (s, h) => s.ReadingChanged += h,
            r => $"P = {r.Reading.PitchDegrees,7:F3}°, R = {r.Reading.RollDegrees,7:F3}°, Y = {r.Reading.YawDegrees,7:F3}°",
            r => MoveBall(BallDataSource.Inclinometer, r.Reading.RollDegrees, r.Reading.PitchDegrees));

        Register<LightSensor, LightSensorReadingChangedEventArgs>(Light, sensors.Light,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.IlluminanceInLux:F3} lux");

        Register<Magnetometer, MagnetometerReadingChangedEventArgs>(Magnetometer, sensors.Magnetometer,
            (s, h) => s.ReadingChanged += h,
            r => $"X = {r.Reading.MagneticFieldX,7:F3}, Y = {r.Reading.MagneticFieldY,7:F3}, Z = {r.Reading.MagneticFieldZ,7:F3}",
            r => MoveBall(BallDataSource.Magnetometer, r.Reading.MagneticFieldX, r.Reading.MagneticFieldY));

        Register<OrientationSensor, OrientationSensorReadingChangedEventArgs>(Orientation, sensors.Orientation,
            (s, h) => s.ReadingChanged += h,
            r => {
                var q = r.Reading.Quaternion;
                return $"W = {q.W,5:F3}, X = {q.X,5:F3}, Y = {q.Y,5:F3}, Z = {q.Z,5:F3}";
            },
            r => MoveBall(BallDataSource.Orientation, r.Reading.Quaternion.X, r.Reading.Quaternion.Y));

        Register<Pedometer, PedometerReadingChangedEventArgs>(Pedometer, sensors.Pedometer,
            (s, h) => s.ReadingChanged += h,
            r => $"{r.Reading.CumulativeStepsDuration}: {r.Reading.CumulativeSteps} ({r.Reading.StepKind})");

        Register<ProximitySensor, ProximitySensorReadingChangedEventArgs>(Proximity, sensors.Proximity,
            (s, h) => s.ReadingChanged += h,
            r => r.Reading.IsDetected ? $"{r.Reading.DistanceInMillimeters:F3} mm" : "-");

        Register<SimpleOrientationSensor, SimpleOrientationSensorOrientationChangedEventArgs>(SimpleOrientation, sensors.SimpleOrientation,
            (s, h) => s.OrientationChanged += h,
            r => $"{r.Orientation}");
    }

    #region Internal

    private readonly Dispatcher _dispatcher;

    private static readonly IReadOnlyDictionary<BallDataSource, double> Factors =
        new Dictionary<BallDataSource, double>
        {
            [BallDataSource.Accelerometer] = 100,
            [BallDataSource.Gyrometer] = 1,
            [BallDataSource.Inclinometer] = 2,
            [BallDataSource.Orientation] = 300,
            [BallDataSource.Magnetometer] = 3,
        };

    private void Register<TSensor, TArgs>(
        SensorData data,
        TSensor? sensor,
        Action<TSensor, TypedEventHandler<TSensor, TArgs>> subscribe,
        Func<TArgs, string> format,
        Action<TArgs>? moveBall = null)
        where TSensor : class
    {
        if (sensor is null)
            return;

        TypedEventHandler<TSensor, TArgs> handler = (_, args) =>
        {
            string info = format(args);
            RunOnUiThread(() =>
            {
                data.Info = info;
                moveBall?.Invoke(args);
            });
        };

        subscribe(sensor, handler);
        data.Status = true;
    }

    private void RunOnUiThread(Action action)
    {
        if (_dispatcher.CheckAccess())
            action();
        else
            _dispatcher.BeginInvoke(action);
    }

    private void MoveBall(BallDataSource source, double x, double y)
    {
        if (BallDataSource != source)
            return;

        Ball.MoveTo(Factors[source] * x, Factors[source] * -y);
    }

    #endregion
}
