using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Services.Interface;

namespace WinTrayMemory.SettingsPage.Components.AutoClean;

public partial class AutoCleanViewModel : ObservableObject
{
    private readonly IAppSettingsService _settingsService;

    [ObservableProperty] private int threshold;
    [ObservableProperty] private bool showNotification;

    public AutoCleanViewModel(IAppSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    partial void OnThresholdChanged(int value)
    {
        _ = SaveSettingAsync(threshold: value);
    }

    partial void OnShowNotificationChanged(bool value)
    {
        _ = SaveSettingAsync(showNotification: value);
    }

    private async Task SaveSettingAsync(
        int? threshold = null,
        bool? showNotification = null)
    {
        try
        {
            var request = new AppSettingsRequest(
                Threshold: threshold,
                ShowNotification: showNotification
            );
            await _settingsService.UpdateSettingsAsync(request);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save error: {ex.Message}");
        }
    }
}
