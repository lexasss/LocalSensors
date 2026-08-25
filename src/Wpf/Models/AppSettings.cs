using System.Text.Json.Serialization;
using System.Windows.Media;

namespace SensorsWpf.Models;

public class AppSettings
{
    public string BallColor { get; set; } = "0,0,0";

    [JsonIgnore]
    public Brush BallBrush {
        get {
            var ballColorRgb = BallColor.Split(",").Select(byte.Parse).ToArray();
            return new SolidColorBrush(Color.FromRgb(ballColorRgb[0], ballColorRgb[1], ballColorRgb[2]));
        }
    }
}
