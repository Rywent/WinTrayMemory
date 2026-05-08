using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Services.Interface;

namespace WinTrayMemory.SettingsPage.Components.General;

public partial class GeneralViewModel : ObservableObject
{
    private readonly IAppSettingsService _settingsService;

    [ObservableProperty] private bool _isRunOnStartup;
    [ObservableProperty] private bool _isCheckForUpdates;
    [ObservableProperty] private bool _allowKillSystemProcesses;

    public GeneralViewModel(IAppSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    partial void OnIsRunOnStartupChanged(bool value)
    {
        if (value)
            StartupHelper.Enable();
        else
            StartupHelper.Disable();

        _ = SaveSettingAsync(isRunOnStartup: value);
    }

    partial void OnIsCheckForUpdatesChanged(bool value)
    {
        _ = SaveSettingAsync(isCheckForUpdates: value);
    }
    partial void OnAllowKillSystemProcessesChanged(bool value)
    {
        _ = SaveSettingAsync(allowKillSystemProcesses: value);
    }

    private async Task SaveSettingAsync(
        bool? isRunOnStartup = null,
        bool? isCheckForUpdates = null,
        bool? allowKillSystemProcesses = null)
    {
        try
        {
            var request = new AppSettingsRequest(
                IsRunOnStartup: isRunOnStartup,
                IsCheckForUpdates: isCheckForUpdates,
                AllowKillSystemProcesses: allowKillSystemProcesses
            );
            await _settingsService.UpdateSettingsAsync(request);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Save error: {ex.Message}");
        }
    }
}

