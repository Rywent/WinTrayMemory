using CommunityToolkit.Mvvm.ComponentModel;
using WinTrayMemory.Data.Services;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.SettingsPage.Components.AutoClean;
using WinTrayMemory.SettingsPage.Components.General;
using WinTrayMemory.SettingsPage.Components.Header;
using WinTrayMemory.SettingsPage.Components.ProcessCategories;
using WinTrayMemory.SettingsPage.Components.ProcessMonitoring;
using WinTrayMemory.SettingsPage.Components.SmartCleaningOptions;

namespace WinTrayMemory.SettingsPage;

/// <summary>
/// settings page view model, which creates the VM for all page components
/// </summary>
public partial class SettingsPageViewModel : ObservableObject
{
    private readonly IAppSettingsService _settingsService;
    private readonly IProcessRuleService _processRuleSerice;
    public HeaderViewModel Header { get; set; }
    public ProcessMonitoringViewModel ProcessMonitoring { get; set; }
    public AutoCleanViewModel AutoClean { get; set; }
    public SmartCleaningOptionsViewModel SmartCleaningOptions { get; set; }
    public ProcessCategoriesViewModel ProcessCategoreies { get; set; }
    [ObservableProperty] private GeneralViewModel general;

    public SettingsPageViewModel(IAppSettingsService settingsService, IProcessRuleService processRuleService)
    {
        _settingsService = settingsService;
        _processRuleSerice = processRuleService;

        Header = new HeaderViewModel();
        ProcessMonitoring = new ProcessMonitoringViewModel(_settingsService);
        AutoClean = new AutoCleanViewModel(_settingsService);
        SmartCleaningOptions = new SmartCleaningOptionsViewModel(_settingsService);
        ProcessCategoreies = new ProcessCategoriesViewModel(_processRuleSerice);
        General = new GeneralViewModel(_settingsService);

        
        _ = LoadSettingsAsync();
    }

    /// <summary>
    /// loading data from the database
    /// </summary>
    public async Task LoadSettingsAsync()
    {
        try
        {
            var settings = await _settingsService.GetSettingsAsync();
            var processesRule = await _processRuleSerice.GetAllRulesAsync();

            ProcessMonitoring.MinProcessSize = settings.MinProcessSize;
            ProcessMonitoring.MaxProcessesShown = settings.MaxProcessesShown;
            ProcessMonitoring.RefreshInterval = settings.RefreshInterval;

            AutoClean.Threshold = settings.Threshold;
            AutoClean.ShowNotification = settings.ShowNotification;

            SmartCleaningOptions.CleanWorkingSet = settings.CleanWorkingSet;
            SmartCleaningOptions.CleanLowPriorityStandby = settings.CleanLowPriorityStandby;
            SmartCleaningOptions.CleanStandbyList = settings.CleanStandbyList;
            SmartCleaningOptions.CleanModifiedPageList = settings.CleanModifiedPageList;

            await ProcessCategoreies.LoadRulesAsync();
            
            
            General.IsRunOnStartup = settings.IsRunOnStartup;
            General.IsCheckForUpdates = settings.IsCheckForUpdates;
            General.AllowKillSystemProcesses = settings.AllowKillSystemProcesses;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
        }
    }
}
