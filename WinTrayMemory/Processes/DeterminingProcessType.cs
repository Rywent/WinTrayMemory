using WinTrayMemory.Config;

namespace WinTrayMemory.Proccesses;

internal class DeterminingProcessType
{
    public enum ProcessType
    {
        Safely,
        Warning,
        Dangerous,
        Unknown
    }

    private readonly AppSettings _settings;

    /// <summary>
    /// initializes the process type determiner with app settings.
    /// </summary>
    /// <param name="settings">app settings containing process type lists.</param>
    public DeterminingProcessType(AppSettings settings)
    {
        _settings = settings;
    }


    /// <summary>
    /// gets process type by its name and returns svg icon path.
    /// </summary>
    /// <param name="processName">process name without .exe extension.</param>
    /// <returns>path to svg icon for the process type.</returns>
    public string GetTypeByProcessName(string processName)
    {
        var name = processName.ToLowerInvariant();

        if (_settings.Dangerous.Contains(name))
            return GetCategoryPath(ProcessType.Dangerous);

        if (_settings.Warning.Contains(name))
            return GetCategoryPath(ProcessType.Warning);

        if (_settings.Safely.Contains(name))
            return GetCategoryPath(ProcessType.Safely);

        return GetCategoryPath(ProcessType.Unknown);
    }

    /// <summary>
    /// gets svg icon path for the specified process type.
    /// </summary>
    /// <param name="type">process type to get icon path for.</param>
    /// <returns>path to svg icon for the given process type.</returns>
    public static string GetCategoryPath(ProcessType type)
    {
        switch (type)
        {
            case ProcessType.Safely:
                return "pack://application:,,,/Resources/img/safely.svg";
            case ProcessType.Warning:
                return "pack://application:,,,/Resources/img/warning.svg";
            case ProcessType.Dangerous:
                return "pack://application:,,,/Resources/img/dangerous.svg";
            case ProcessType.Unknown:
            default:
                return "pack://application:,,,/Resources/img/unknown.svg";
        }
    }

}
