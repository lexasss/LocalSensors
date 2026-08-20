using System.Windows;

namespace SensorsWpf.Views;

public partial class MainWindow : Window
{
    public MainWindow(ViewModels.MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}