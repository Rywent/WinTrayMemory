using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.MainPage;
using WinTrayMemory.MainPage.Components.Messages;
using WinTrayMemory.Memory;
using WinTrayMemory.Settings;
using WinTrayMemory.SettingsPage;
using WinTrayMemory.SettingsPage.Components.Messages;

namespace WinTrayMemory.Shell;

public sealed partial class MainViewModel : ObservableObject, IRecipient<OpenSettingsPageMessage>, IRecipient<OpenMonitoringPageMessage>
{

    private readonly MemoryInfoService _memoryInfoService;
    private readonly System.Timers.Timer _trayTimer;
    private readonly IAppSettingsService _settingsService;

    [ObservableProperty] private Object? _currentView;

    [ObservableProperty] private string _trayTooltip = "WinTrayMemory";
    public MainPageViewModel MainPage { get; }
    public SettingsPageViewModel SettingsPage { get; }


    /// <summary>
    /// initializes the main view model and loads application settings
    /// </summary>
    public MainViewModel(
        MainPageViewModel mainPage,
        SettingsPageViewModel settingsPage,
        IAppSettingsService settingsService,
        SettingsChangedEvent settingsChangedEvent)
    {
        MainPage = mainPage;
        SettingsPage = settingsPage;
        _settingsService = settingsService;

        CurrentView = MainPage;
        WeakReferenceMessenger.Default.RegisterAll(this);

        _memoryInfoService = new MemoryInfoService();

        _trayTimer = new System.Timers.Timer();
        _trayTimer.Elapsed += (_, _) => UpdateTrayTooltip();
        _trayTimer.AutoReset = true;
        _trayTimer.Start();

        _ = InitializeTrayTimerAsync();

        settingsChangedEvent.SettingsChanged += async () =>
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await RefreshTrayIntervalAsync();
            });
        };
    }

    private async Task InitializeTrayTimerAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        _trayTimer.Interval = TimeSpan.FromSeconds(settings.RefreshInterval).TotalMilliseconds;
        _trayTimer.Start();
        UpdateTrayTooltip();
    }

    private async Task RefreshTrayIntervalAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        _trayTimer.Interval = TimeSpan.FromSeconds(settings.RefreshInterval).TotalMilliseconds;
    }

    private void UpdateTrayTooltip()
    {
        var (totalGb, usedGb, usedPercent) = _memoryInfoService.GetMemoryInfo();

        Application.Current.Dispatcher.Invoke(() =>
        {
            TrayTooltip = $"WinTrayMemory\n" +
                          $"RAM: {usedGb:F1} / {totalGb:F1} GB ({usedPercent:F0}%)\n" +
                          $"Click to open";
        });
    }

    /// <summary>
    /// switches shell view to the heaviest processes view.
    /// </summary>
    public void Receive(OpenSettingsPageMessage message)
    {
        CurrentView = SettingsPage;
    }

    /// <summary>
    /// Switches shell view to the monitoring page.
    /// </summary>
    public void Receive(OpenMonitoringPageMessage message)
    {
        CurrentView = MainPage;
    }
}
