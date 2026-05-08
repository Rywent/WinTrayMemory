using CommunityToolkit.Mvvm.ComponentModel;
using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Services.Interface;


namespace WinTrayMemory.SettingsPage.Components.ProcessMonitoring;

public partial class ProcessMonitoringViewModel : ObservableObject
{
    private readonly IAppSettingsService _settingsService;

    [ObservableProperty] private int _minProcessSize = 50;
    [ObservableProperty] private int _maxProcessesShown = 5;
    [ObservableProperty] private int _refreshInterval = 1;

    public ProcessMonitoringViewModel(IAppSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    partial void OnMinProcessSizeChanged(int value)
    {
        _ = SaveSettingAsync(minProcessSize: value);
    }

    partial void OnMaxProcessesShownChanged(int value)
    {
        _ = SaveSettingAsync(maxProcessesShown: value);
    }

    partial void OnRefreshIntervalChanged(int value)
    {
        _ = SaveSettingAsync(refreshInterval: value);
    }

    private async Task SaveSettingAsync( int? minProcessSize = null, int? maxProcessesShown = null,  int? refreshInterval = null)
    {
        try
        {
            var request = new AppSettingsRequest(
                MinProcessSize: minProcessSize,
                MaxProcessesShown: maxProcessesShown,
                RefreshInterval: refreshInterval
            );
            await _settingsService.UpdateSettingsAsync(request);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save error: {ex.Message}");
        }
    }
}