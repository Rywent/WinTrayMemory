using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.MainPage.Components.Actions;
using WinTrayMemory.MainPage.Components.Header;
using WinTrayMemory.MainPage.Components.HeaviestProcesses;
using WinTrayMemory.MainPage.Components.MemoryDetailingChart;
using WinTrayMemory.MainPage.Components.MemoryPressure;
using WinTrayMemory.MainPage.Components.MemoryUsage;
using WinTrayMemory.MainPage.Components.RamHistoryChart;
using WinTrayMemory.Settings;

namespace WinTrayMemory.MainPage;

/// <summary>
/// main page view model, which creates the VM for all page components
/// </summary>
public class MainPageViewModel
{
    private readonly IAppSettingsService _settingsService;
    private readonly IProcessRuleService _processRuleService;

    public HeaderViewModel Header { get; set; }
    public MemoryUsageViewModel Memory { get; set; }
    public HeaviestProcessesViewModel HeaviestProcesses { get; set; }
    public ActionsViewModel Actions { get; set; }
    public RamHistoryChartViewModel RamHistoryChart { get; set; }
    public MemoryDetailingChartViewModel MemoryDetailingChart { get; set; }
    public MemoryPressureViewModel MemoryPressure { get; set; }

    public string VersionText => $"@Rywent • v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}";

    public MainPageViewModel(IAppSettingsService settingsService, IProcessRuleService processRuleService,
        SettingsChangedEvent settingsChangedEvent)
    {
        _settingsService = settingsService;
        _processRuleService = processRuleService;
        Header = new HeaderViewModel();
        Memory = new MemoryUsageViewModel();
        HeaviestProcesses = new HeaviestProcessesViewModel(_settingsService, _processRuleService);
        Actions = new ActionsViewModel(_settingsService);
        RamHistoryChart = new RamHistoryChartViewModel();
        MemoryDetailingChart = new MemoryDetailingChartViewModel();
        MemoryPressure = new MemoryPressureViewModel();

        settingsChangedEvent.SettingsChanged += async () =>
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await RefreshSettingsAsync();
            });
        };

    }

    /// <summary>
    /// refreshes settings in components that depend on them (refresh interval etc)
    /// </summary>
    public async Task RefreshSettingsAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();

        HeaviestProcesses.UpdateRefreshInterval(settings.RefreshInterval);
        await HeaviestProcesses.RefreshMonitorSettingsAsync();
        Actions.UpdateRefreshInterval(settings.RefreshInterval);
    }


}
