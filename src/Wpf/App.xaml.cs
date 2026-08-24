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
        SaveConfiguration(_host.Services);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            SaveConfiguration(_host.Services);

            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager config)
    {
        services.Configure<Models.MainSettings>(config.GetSection("Main"));

        services.AddSingleton<SensorProvider>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
    }

    private static void SaveConfiguration(IServiceProvider services)
    {
        Dictionary<string, object> configSections = [];

        services.GetRequiredService<MainViewModel>().SaveSettings(settings =>
            configSections["Main"] = settings
        );

        var json = JsonSerializer.Serialize(configSections, new JsonSerializerOptions { WriteIndented = true });

        System.IO.File.WriteAllText("appsettings.json", json);
    }

    #endregion
}
