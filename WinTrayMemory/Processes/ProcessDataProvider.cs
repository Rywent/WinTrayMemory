using System.Diagnostics;
using WinTrayMemory.Config;

namespace WinTrayMemory.Proccesses;

internal class ProcessDataProvider
{
    private readonly DeterminingProcessType _determiningProcess;
    private readonly AppSettings _settings;


    /// <summary>
    /// initializes process data provider with application settings.
    /// </summary>
    /// <param name="settings">application settings for process filtering and thresholds.</param>
    public ProcessDataProvider(AppSettings settings)
    {
        _settings = settings;

        _determiningProcess = new DeterminingProcessType(_settings);
    }

    /// <summary>
    /// gets a list of heaviest processes grouped by name and filtered by memory usage.
    /// </summary>
    /// <returns>list of process info sorted by memory usage in descending order.</returns>
    public List<ProcessInfo> GetHeaviestProcesses()
    {
        return Process
            .GetProcesses()
            .GroupBy(p => p.ProcessName)
            .Select(g => new ProcessInfo
            {
                Name = g.Key,
                Count = g.Count(),
                ClueMessage = $"{g.Key}({g.Count()})",
                MemoryUses = g.Sum(p => p.WorkingSet64) / 1024m / 1024m,
                Category = _determiningProcess.GetTypeByProcessName(g.Key),
            })
            .Where(x => x.MemoryUses > _settings.MinHeavyProcessSizeMb)
            .OrderByDescending(x => x.MemoryUses)
            .Take(_settings.MaxProcessesShown)
            .ToList();
    }
}
