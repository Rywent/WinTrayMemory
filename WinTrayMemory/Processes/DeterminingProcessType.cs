using WinTrayMemory.Data.Enums;
using WinTrayMemory.Data.Services.Interface;

namespace WinTrayMemory.Processes;

public sealed class DeterminingProcessType
{
    public enum ProcessType
    {
        Safely,
        Warning,
        Dangerous,
        Unknown
    }

    private readonly IProcessRuleService _processRuleService;
    private Dictionary<string, Category>? _cache;

    /// <summary>
    /// initializes the process type determiner with app settings.
    /// </summary>
    /// <param name="processRuleService">app settings containing user process type lists.</param>


    public DeterminingProcessType(IProcessRuleService processRuleService)
    {
        _processRuleService = processRuleService;
    }



    /// <summary>
    /// gets process type by its name and returns svg icon path.
    /// </summary>
    /// <param name="processName">process name without .exe extension.</param>
    /// <returns>process type.</returns>
    public ProcessType GetTypeByProcessName(string processName)
    {
        EnsureCacheLoaded();

        var name = processName.ToLowerInvariant();

        if (_cache!.TryGetValue(name, out var userDefinedType))
            return userDefinedType switch
            {
                Category.Safe => ProcessType.Safely,
                Category.Warning => ProcessType.Warning,
                Category.Dangerous => ProcessType.Dangerous,
                _ => ProcessType.Unknown
            };


        if (DefaultProcesses.Rules.TryGetValue(name, out var defaultType))
            return defaultType;

        return ProcessType.Unknown;
    }


    /// <summary>
    /// Загружает/обновляет кеш из БД.
    /// </summary>
    private void EnsureCacheLoaded()
    {
        if (_cache == null)
        {
            var rules = Task.Run(async () => await _processRuleService.GetAllRulesAsync()).Result;
            _cache = rules.ToDictionary(r => r.Name.ToLowerInvariant(), r => r.Category);
        }
    }

    /// <summary>
    /// Сброс кеша (вызывать после добавления/удаления правил).
    /// </summary>
    public void InvalidateCache()
    {
        _cache = null;
    }

}
