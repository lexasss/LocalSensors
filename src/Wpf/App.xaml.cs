using Microsoft.Extensions.DependencyInjection;
using SensorsWpf.Services;
using SensorsWpf.ViewModels;
using SensorsWpf.Views;
using System.Globalization;
using System.Windows;

namespace SensorsWpf;

public partial class App : Application
{
    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();
    }

    #region Internal

    private readonly IServiceProvider _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        CultureInfo culture = new("en-US");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        MainWindow = mainWindow;
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<SensorProvider>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
    }

    #endregion
}
