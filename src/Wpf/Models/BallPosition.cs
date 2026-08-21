using CommunityToolkit.Mvvm.ComponentModel;

namespace SensorsWpf.Models;

public class BallPosition(double x = 0, double y = 0) : ObservableObject
{
    public double X => _x;
    public double Y => _y;

    public void MoveTo(double x, double y)
    {
        _x = x;
        _y = y;
        OnPropertyChanged(nameof(X));
        OnPropertyChanged(nameof(Y));
    }

    #region Internal

    private double _x = x;
    private double _y = y;

    #endregion
}
