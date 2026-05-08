using Microsoft.Win32;
using System.Diagnostics;

public static class StartupHelper
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "WinTrayMemory";

    private static string GetExePath()
    {
        using var process = Process.GetCurrentProcess();
        return process.MainModule?.FileName
            ?? Environment.ProcessPath
            ?? throw new InvalidOperationException("Cannot get executable path");
    }

    public static void Enable()
    {
        var exePath = GetExePath();

        if (exePath.Contains(' '))
            exePath = $"\"{exePath}\"";

        using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true)
            ?? Registry.CurrentUser.CreateSubKey(RunKeyPath)!;

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