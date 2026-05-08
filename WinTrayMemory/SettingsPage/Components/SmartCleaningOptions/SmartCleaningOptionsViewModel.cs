using CommunityToolkit.Mvvm.ComponentModel;
using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Services.Interface;

namespace WinTrayMemory.SettingsPage.Components.SmartCleaningOptions;

public partial class SmartCleaningOptionsViewModel : ObservableObject
{
    private readonly IAppSettingsService _settingsService;

    [ObservableProperty] private bool _cleanWorkingSet = true;
    [ObservableProperty] private bool _cleanLowPriorityStandby;
    [ObservableProperty] private bool _cleanStandbyList;
    [ObservableProperty] private bool _cleanModifiedPageList;

    public SmartCleaningOptionsViewModel(IAppSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    partial void OnCleanWorkingSetChanged(bool value)
    {
        _ = SaveSettingAsync(cleanWorkingSet: value);
    }

    partial void OnCleanLowPriorityStandbyChanged(bool value)
    {
        _ = SaveSettingAsync(cleanLowPriorityStandby: value);
    }

    partial void OnCleanStandbyListChanged(bool value)
    {
        _ = SaveSettingAsync(cleanStandbyList: value);
    }

    partial void OnCleanModifiedPageListChanged(bool value)
    {
        _ = SaveSettingAsync(cleanModifiedPageList: value);
    }

    private async Task SaveSettingAsync( bool? cleanWorkingSet = null, bool? cleanLowPriorityStandby = null,
        bool? cleanStandbyList = null,
        bool? cleanModifiedPageList = null)
    {
        try
        {
            var request = new AppSettingsRequest(
                CleanWorkingSet: cleanWorkingSet,
                CleanLowPriorityStandby: cleanLowPriorityStandby,
                CleanStandbyList: cleanStandbyList,
                CleanModifiedPageList: cleanModifiedPageList
            );
            await _settingsService.UpdateSettingsAsync(request);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save error: {ex.Message}");
        }
    }
}
