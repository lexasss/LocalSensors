using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorsWpf.Services;
using SensorsWpf.ViewModels;
using SensorsWpf.Views;
using System.Globalization;
using System.Text.Json;
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

    const string CONFIG_FILENAME = "appSettings.json";
    const string CONFIG_SECTION_MAIN = "Main";

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
            SaveConfiguration();

            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services, ConfigurationManager config)
    {
        services.Configure<Models.MainSettings>(config.GetSection(CONFIG_SECTION_MAIN));

        services.AddSingleton<SensorProvider>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
    }

    private void SaveConfiguration()
    {
        Dictionary<string, object> configSections = [];

        _host?.Services.GetRequiredService<MainViewModel>().SaveSettings(settings =>
            configSections[CONFIG_SECTION_MAIN] = settings
        );

        var json = JsonSerializer.Serialize(configSections, new JsonSerializerOptions { WriteIndented = true });

        System.IO.File.WriteAllText(CONFIG_FILENAME, json);
    }

    #endregion
}
