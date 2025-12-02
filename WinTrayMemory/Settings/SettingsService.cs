using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

using WinTrayMemory.Config;

namespace WinTrayMemory.Settings;

internal class SettingsService
{
    /// <summary>
    /// full path to the configuration file in the user's appdata.
    /// </summary>
    private static readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"WinTrayMemory","config.json");

    /// <summary>
    /// watches the configuration file for external changes.
    /// </summary>
    private static readonly FileSystemWatcher? _watcher;

    public static AppSettings Settings { get; private set; } = null!;

    /// <summary>
    /// loads application settings from the configuration file.
    /// creates and saves default settings if the file does not exist.</summary>
    /// <returns>loaded application settings.</returns>
    public static AppSettings Load()
    {
        Settings = LoadInternal();
        return Settings;
    }

    /// <summary>
    /// static constructor that initializes settings and starts watching the config file for changes.
    /// </summary>
    static SettingsService()
    {
        try
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Settings = LoadInternal();

            if (!string.IsNullOrEmpty(dir))
            {
                _watcher = new FileSystemWatcher(dir, Path.GetFileName(_filePath));
                _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
                _watcher.Changed += OnConfigChanged;
                _watcher.Created += OnConfigChanged;
                _watcher.Renamed += OnConfigChanged;
                _watcher.EnableRaisingEvents = true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            Settings = CreateDefault();
        }
    }
    /// <summary>
    /// loads settings from the configuration file or returns default settings.
    /// does not update the public <see cref="Settings"/> property.</summary>
    private static AppSettings LoadInternal()
    {
        if (!File.Exists(_filePath))
        {
            var defaults = CreateDefault();
            Save(defaults);
            return defaults;
        }

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? CreateDefault();
    }
    /// <summary>
    /// saves the specified settings to the configuration file.
    /// </summary>
    /// <param name="settings">settings instance to save.</param>
    public static void Save(AppSettings settings)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(settings, options);
        File.WriteAllText(_filePath, json);
    }

    /// <summary>
    /// creates a new settings instance with default values.
    /// </summary>
    /// <returns>default application settings.</returns>
    private static AppSettings CreateDefault()
    {
        return new AppSettings
        {
            Safely = new List<string> { "chrome", "msedge", "firefox", "opera", "telegram", "discord", "steam" },
            Warning = new List<string> { "devenv", "code", "rider", "winword", "excel", "powerpnt", "figma", "notepad", "rainmeter" },
            Dangerous = new List<string>
            {
                "system", "smss", "csrss", "wininit", "services",
                "lsass", "winlogon", "dwm", "svchost", "fontdrvhost",
                "memory compression",

                "explorer", "logonui", "sihost", "userinit",
                "taskhost", "taskhostw", "taskhostex",

                "securityhealthservice", "securityhealthhost",
                "msmpeng", "nissrv", "nisrv",

                "spoolsv", "audiodg", "wmiprvse", "wmiadap",
                "winmgmt", "dnsclient", "dhcp", "cryptsvc",
                "eventlog", "lanmanworkstation", "lanmanserver",
                "netlogon", "nsi", "rpcss", "srumsvc",
                "sysmain", "timebroker", "timebrokerce",
                "swprv", "vss", "sppsvc",

                "searchindexer", "searchapp", "searchui",
                "startmenuexperiencehost", "shellexperiencehost",
                "runtimebroker",
                "wermgr", "werfault", "compattelemetryrunner", "compattelrunner",

                // this app
                "wintraymemory"
            },
            MinHeavyProcessSizeMb = 500,
            MaxProcessesShown = 15,
            RefreshIntervalSec = 3,

            CleanWorkingSet = true,
            CleanLowPriorityStandby = false,
            CleanModifiedPageList = false,
            CleanStandbyList = false,
            RunOnStartup = false
        };
    }

    /// <summary>
    /// handles configuration file changes by reloading settings from disk
    /// and updating the current in-memory settings instance.</summary>
    private static void OnConfigChanged(object? sender, FileSystemEventArgs e)
    {
        Task.Run(async () =>
        {
            await Task.Delay(150);

            AppSettings reloaded;
            try
            {
                reloaded = LoadInternal();
            }
            catch
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Settings.Safely = reloaded.Safely;
                Settings.Warning = reloaded.Warning;
                Settings.Dangerous = reloaded.Dangerous;
                Settings.MinHeavyProcessSizeMb = reloaded.MinHeavyProcessSizeMb;
                Settings.MaxProcessesShown = reloaded.MaxProcessesShown;
                Settings.RefreshIntervalSec = reloaded.RefreshIntervalSec;
                Settings.CleanWorkingSet = reloaded.CleanWorkingSet;
                Settings.CleanStandbyList = reloaded.CleanStandbyList;
                Settings.CleanLowPriorityStandby = reloaded.CleanLowPriorityStandby;
                Settings.CleanModifiedPageList = reloaded.CleanModifiedPageList;
                Settings.RunOnStartup = reloaded.RunOnStartup;
            });
        });
    }
}
