using Microsoft.Win32;
using System.Reflection;

internal static class StartupHelper
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "WinTrayMemory";

    public static bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
        if (key == null)
            return false;

        var value = key.GetValue(ValueName) as string;
        string exePath = Assembly.GetEntryAssembly()!.Location;
        return string.Equals(value, exePath, StringComparison.OrdinalIgnoreCase);
    }

    public static void Enable()
    {
        string exePath = Assembly.GetEntryAssembly()!.Location;

        using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true) ?? Registry.CurrentUser.CreateSubKey(RunKeyPath)!;

        key.SetValue(ValueName, exePath);
    }

    public static void Disable()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true);
        if (key == null)
            return;

        key.DeleteValue(ValueName, throwOnMissingValue: false);
    }
}
