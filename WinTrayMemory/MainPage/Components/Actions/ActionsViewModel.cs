using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Numerics;
using System.Windows;
using System.Windows.Threading;
using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.Memory;
using WinTrayMemory.Notifications;

namespace WinTrayMemory.MainPage.Components.Actions;

/// <summary>
/// actions viewmodel responsible for automatic memory cleaning features
/// </summary>
public partial class ActionsViewModel : ObservableObject
{
    private readonly WindowsNotification _notification;
    private readonly IAppSettingsService _settingsService;
    private readonly MemoryInfoService _memoryInfoService;
    private DispatcherTimer _autoCheckTimer;


    [ObservableProperty] private bool _isAuto = false;
    [ObservableProperty] private string _autoModeText = "AUTO: OFF";

    public ActionsViewModel(IAppSettingsService settings)
    {
        _settingsService = settings;
        _notification = new WindowsNotification();
        _memoryInfoService = new MemoryInfoService();

        _autoCheckTimer = new DispatcherTimer();
        _autoCheckTimer.Tick += async (_, _) => await CheckAndAutoCleanIfNeeded();

        _ = LoadAutoModeAsync();
    }

    /// <summary>
    /// loads current auto clean mode from settings and configures timer
    /// </summary>
    private async Task LoadAutoModeAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        IsAuto = settings.IsAutoCleanEnabled;

        _autoCheckTimer!.Interval = TimeSpan.FromSeconds(settings.RefreshInterval);

        if (IsAuto)
            _autoCheckTimer.Start();
    }

    partial void OnIsAutoChanged(bool value)
    {
        AutoModeText = value ? "AUTO: ON" : "AUTO: OFF";
        _ = SaveAutoModeAsync(value);

        if (value)
            _autoCheckTimer?.Start();
        else
            _autoCheckTimer?.Stop();
    }

    /// <summary>
    /// saves auto clean enabled state to settings
    /// </summary>
    private async Task SaveAutoModeAsync(bool enabled)
    {
        var request = new AppSettingsRequest(IsAutoCleanEnabled: enabled);
        await _settingsService.UpdateSettingsAsync(request);
    }

    /// <summary>
    /// checks current memory usage and runs auto clean if threshold is reached 
    /// </summary>
    public async Task CheckAndAutoCleanIfNeeded()
    {
        if (!IsAuto)
            return;

        var settings = await _settingsService.GetSettingsAsync();
        var (_, _, usedPercent) = _memoryInfoService.GetMemoryInfo();

        if (usedPercent >= settings.Threshold)
        {
            await PerformAutoClean(settings);
        }
    }
    /// <summary>
    /// performs the actual memory cleaning according to current settings
    /// </summary>
    private async Task PerformAutoClean(AppSettings settings)
    {
        if (!MemoryCleaner.IsAdministrator())
            return;

        try
        {
            bool cleaned = false;

            if (settings.CleanWorkingSet)
            {
                MemoryCleaner.TrimAllWorkingSets();
                cleaned = true;
            }
            if (settings.CleanLowPriorityStandby)
            {
                MemoryCleaner.PurgeLowPriorityStandbyList();
                cleaned = true;
            }
            if (settings.CleanStandbyList)
            {
                MemoryCleaner.PurgeStandbyList();
                cleaned = true;
            }
            if (settings.CleanModifiedPageList)
            {
                MemoryCleaner.PurgeModifiedPageList();
                cleaned = true;
            }

            if (cleaned && settings.ShowNotification)
            {
                _notification.Show("Auto-clean", "Memory threshold reached. Cleaning completed.");
            }
        }
        catch (Exception ex)
        {
            if (settings.ShowNotification)
            {
                _notification.Show("Auto-clean failed", ex.Message);
            }
        }
    }

    /// <summary>
    /// executes smart cleaning command
    /// </summary>
    [RelayCommand]
    private async Task SmartCleaning()
    {
        var settings = await _settingsService.GetSettingsAsync();

        if (!MemoryCleaner.IsAdministrator())
        {
            MessageBox.Show("Run as administrator to clean memory.", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (settings.ShowNotification)
                _notification.Show("Auto-clean", $"Memory usage reached {settings.Threshold}%. Starting cleanup...");

            bool cleaned = false;

            if (settings.CleanWorkingSet)
            {
                MemoryCleaner.TrimAllWorkingSets();
                cleaned = true;
            }
            if (settings.CleanLowPriorityStandby)
            {
                MemoryCleaner.PurgeLowPriorityStandbyList();
                cleaned = true;
            }
            if (settings.CleanStandbyList)
            {
                MemoryCleaner.PurgeStandbyList();
                cleaned = true;
            }
            if (settings.CleanModifiedPageList)
            {
                MemoryCleaner.PurgeModifiedPageList();
                cleaned = true;
            }

            if (cleaned && settings.ShowNotification)
            {
                _notification.Show("Auto-clean", "Memory threshold reached. Cleaning completed.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cleaning error: {ex.Message}", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// toggles auto mode on/off 
    /// </summary>
    [RelayCommand]
    private void SwitchAutoMode()
    {
        IsAuto = !IsAuto;
    }
    /// <summary>
    /// updates refresh interval for auto check timer
    /// </summary>
    public void UpdateRefreshInterval(int seconds)
    {
        _autoCheckTimer.Interval = TimeSpan.FromSeconds(seconds);
    }
}