using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorsWpf.Models;
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
        CultureInfo culture = new("en-US");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    #region Internal

    IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = Host.CreateApplicationBuilder();

        ConfigureServices(builder.Services, builder.Configuration);

        _host = builder.Build();
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow = mainWindow;
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            _host.Services
                .GetRequiredService<UserSettingsProvider>()
                .Save();

            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services, ConfigurationManager config)
    {
        services.Configure<AppSettings>(config);

        services.AddSingleton<UserSettingsProvider>();
        services.AddSingleton<SensorProvider>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddSingleton(sp => sp.GetRequiredService<UserSettingsProvider>().Settings);
    }

    #endregion
}
