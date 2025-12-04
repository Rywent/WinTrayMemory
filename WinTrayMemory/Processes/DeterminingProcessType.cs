using WinTrayMemory.Config;

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
    /// <returns>process type.</returns>
    public ProcessType GetTypeByProcessName(string processName)
    {
        var name = processName.ToLowerInvariant();

        if (_settings.Dangerous.Contains(name))
            return ProcessType.Dangerous;

        if (_settings.Warning.Contains(name))
            return ProcessType.Warning;

        if (_settings.Safely.Contains(name))
            return ProcessType.Safely;

        return ProcessType.Unknown;
    }

}
