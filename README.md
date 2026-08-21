# Sensors

Console and WPF apps to read and display data from sensors available on the device.
Both apps use the WinRT sensor APIs (`Windows.Devices.Sensors`) and show live readings
from every sensor the hardware provides.

## Projects

| Project | Path | Description |
|---------|------|-------------|
| SensorsConsole | `src/Console` | Console UI: lists available sensors and prints live readings in a table layout. |
| SensorsWpf | `src/Wpf` | WPF UI (Material Design): live readings with status indicators, plus a ball visualization driven by a selectable motion sensor. |

## Supported sensors

Activity, Accelerometer, Altimeter, Barometer, Compass, Gyrometer, Hinge Angle,
Human Presence, Inclinometer, Light, Magnetometer, Orientation (quaternion),
Pedometer, Proximity, Simple Orientation.

Availability depends entirely on the device hardware. Sensors that are not present
are marked accordingly (red indicator in WPF, `-` prefix in the console listing).

## Requirements

- Windows 11 22H2 or later (OS build 22621+)
- .NET 10 SDK:
- A device with the corresponding physical sensors

## Build and run

Open `Sensors.slnx` in Visual Studio, or use the CLI:

```sh
dotnet build src/Wpf/SensorsWpf.csproj
dotnet run --project src/Wpf/SensorsWpf.csproj
```

```sh
dotnet build src/Console/SensorsConsole.csproj
dotnet run --project src/Console/SensorsConsole.csproj
```

## WPF app

- Each tile shows a sensor's name, availability indicator and live reading.
- The panel on the right displays a ball whose position is driven by one of the
  motion sensors (Accelerometer, Gyrometer, Inclinometer, Orientation or Magnetometer).
  Select a source with the radio buttons next to the readings; only available
  sensors can be selected.

## License

Distributed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.
