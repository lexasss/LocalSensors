using System.Reflection;
using System.Text.Json;

namespace SensorsWpf.Services;

public sealed class UserSettingsProvider
{
    public Models.UserSettings Settings { get; }    // single shared instance

    public UserSettingsProvider()
    {
        Models.UserSettings? settings = null;

        try
        {
            if (System.IO.File.Exists(_filename))
            {
                var json = System.IO.File.ReadAllText(_filename);
                settings = JsonSerializer.Deserialize<Models.UserSettings>(json);
            }
            else
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_filename) ?? throw new Exception());
            }
        }
        catch (Exception)
        {
            // skip silently
        }

        Settings = settings ?? new Models.UserSettings();
    }

    public void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions() { WriteIndented = true });
            System.IO.File.WriteAllText(_filename, json);
        }
        catch (Exception)
        {
            // silently skip
        }
    }

    #region Internal

    readonly string _filename = System.IO.Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Assembly.GetExecutingAssembly().GetName().Name,
        "appSettings.user.json"
    );

    #endregion
}
