using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WinTrayMemory.Config;

namespace WinTrayMemory.Settings;

public sealed partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _configurationFilePath;

    private readonly AppSettings _settings;


    /// <summary>
    /// initializes the settings view model with current app settings.</summary>
    /// <param name="settings">current application settings instance.</param>
    public SettingsViewModel(AppSettings settings)
    {
        _settings = settings;
        ConfigurationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"WinTrayMemory","config.json");

        _settings.RunOnStartup = StartupHelper.IsEnabled();
    }

    public decimal HeaviestProcessSize
    {
        get => _settings.MinHeavyProcessSizeMb;
        set => _settings.MinHeavyProcessSizeMb = value;
    }

    public int MaxProcessesShown
    {
        get => _settings.MaxProcessesShown;
        set => _settings.MaxProcessesShown = value;
    }

    public int RefreshIntervalSec
    {
        get => _settings.RefreshIntervalSec;
        set => _settings.RefreshIntervalSec = value;
    }

    public bool CleanWorkingSet
    {
        get => _settings.CleanWorkingSet;
        set => _settings.CleanWorkingSet = value;
    }

    public bool CleanLowPriorityStandby
    {
        get => _settings.CleanLowPriorityStandby;
        set => _settings.CleanLowPriorityStandby = value;
    }

    public bool CleanStandbyList
    {
        get => _settings.CleanStandbyList;
        set => _settings.CleanStandbyList = value;
    }

    public bool CleanModifiedPageList
    {
        get => _settings.CleanModifiedPageList;
        set => _settings.CleanModifiedPageList = value;
    }

    public bool RunOnStartup
    {
        get => _settings.RunOnStartup;
        set
        {
            if (_settings.RunOnStartup == value)
                return;

            _settings.RunOnStartup = value;
            OnPropertyChanged();

            OnRunOnStartupChanged(value);
        }
    }

    /// <summary>
    /// opens the configuration file in notepad for manual editing.
    /// </summary>
    [RelayCommand]
    private void OpenConfigurationFile()
    {
        try
        {
            if (!File.Exists(ConfigurationFilePath))
            {
                SettingsService.Save(SettingsService.Settings);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = $"\"{ConfigurationFilePath}\"",
                UseShellExecute = true
            });
        }       
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot open config file: {ex.Message}", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// saves current settings to the configuration file.
    /// </summary>
    [RelayCommand]
    private void SaveSettings()
    {
        SettingsService.Save(_settings);
    }

    /// <summary>
    /// applies startup setting change and updates autostart registration.
    /// </summary>
    /// <param name="value">new value indicating whether app should run on startup.</param>
    private void OnRunOnStartupChanged(bool value)
    {
        try
        {
            if (value)
                StartupHelper.Enable();
            else
                StartupHelper.Disable();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot change startup setting: {ex.Message}", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Error);

            _settings.RunOnStartup = StartupHelper.IsEnabled();
            OnPropertyChanged(nameof(RunOnStartup));
        }
    }
}
