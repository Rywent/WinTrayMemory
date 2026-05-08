using System.Diagnostics;
using WinTrayMemory.Data.Services.Interface;

namespace WinTrayMemory.Processes;

internal sealed class ProcessDataProvider
{
    private readonly IAppSettingsService _settingsService;
    private readonly IProcessRuleService _processRuleService;
    private readonly DeterminingProcessType _determiningProcess;

    private int _minProcessSize = 500;
    private int _maxProcessesShown = 15;
    /// <summary>
    /// initializes process data provider with application settings.
    /// </summary>
    /// <param name="settings">application settings for process filtering and thresholds.</param>
    public ProcessDataProvider(IAppSettingsService settings, IProcessRuleService processRule)
    {
        _settingsService = settings;
        _processRuleService = processRule;
        _determiningProcess = new DeterminingProcessType(_processRuleService);
        _ = LoadSettingsAsync();
    }
    private async Task LoadSettingsAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        _minProcessSize = settings.MinProcessSize;
        _maxProcessesShown = settings.MaxProcessesShown;
    }

    public async Task RefreshSettingsAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        _minProcessSize = settings.MinProcessSize;
        _maxProcessesShown = settings.MaxProcessesShown;
    }

    /// <summary>
    /// gets a list of heaviest processes grouped by name and filtered by memory usage.
    /// </summary>
    /// <returns>list of process info sorted by memory usage in descending order.</returns>
    public List<ProcessInfo> GetHeaviestProcesses()
    {
        var currentProcessName = Process.GetCurrentProcess().ProcessName;

        return Process
            .GetProcesses()
            .Where(p => p.ProcessName != currentProcessName)
            .GroupBy(p => p.ProcessName)
            .Select(g => new ProcessInfo
            (
                Name: g.Key,
                Count: g.Count(),
                ClueMessage: $"{g.Key}({g.Count()})",
                MemoryUses: g.Sum(p => p.WorkingSet64) / 1024m / 1024m,
                Category: _determiningProcess.GetTypeByProcessName(g.Key)
            ))
            .Where(x => x.MemoryUses >= _minProcessSize)
            .OrderByDescending(x => x.MemoryUses)
            .Take(_maxProcessesShown)
            .ToList();
    }
}
