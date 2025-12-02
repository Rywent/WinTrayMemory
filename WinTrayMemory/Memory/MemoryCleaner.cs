using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;

namespace WinTrayMemory.Memory;

public static class MemoryCleaner
{
    #region Admin check

    public static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    #endregion

    #region Working Set

    public static void TrimAllWorkingSets()
    {
        foreach (var process in Process.GetProcesses())
        {
            try
            {
                if (process.HasExited)
                    continue;

                NativeMethods.EmptyWorkingSet(process.Handle);
            }
            catch
            {
                // intentionally ignore per process errors: some system or protected processes
                // may refuse EmptyWorkingSet, but this is expected and not critical
            }
        }
    }

    #endregion

    #region Standby / Modified lists

    private static int CallMemoryListCommand(NativeMethods.SystemMemoryListCommand command)
    {
        IntPtr ptr = Marshal.AllocHGlobal(sizeof(int));
        try
        {
            Marshal.WriteInt32(ptr, (int)command);

            int status = NativeMethods.NtSetSystemInformation(
                NativeMethods.SystemMemoryListInformation,
                ptr,
                sizeof(int));

            return status;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static void PurgeStandbyList()
    {
        int status = CallMemoryListCommand(NativeMethods.SystemMemoryListCommand.MemoryPurgeStandbyList);
        if (status != 0)
            MessageBox.Show(
                $"PurgeStandbyList failed. NTSTATUS=0x{status:X8}. Run tool as SYSTEM (Task Scheduler) if you really need this feature.",
                "WinTrayMemory",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }

    public static void PurgeLowPriorityStandbyList()
    {
        int status = CallMemoryListCommand(NativeMethods.SystemMemoryListCommand.MemoryPurgeLowPriorityStandbyList);
        if (status != 0)
            MessageBox.Show(
                $"PurgeLowPriorityStandbyList failed. NTSTATUS=0x{status:X8}",
                "WinTrayMemory",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }

    public static void PurgeModifiedPageList()
    {
        int status = CallMemoryListCommand(NativeMethods.SystemMemoryListCommand.MemoryPurgeModifiedList);
        if (status != 0)
            MessageBox.Show(
                $"PurgeModifiedPageList failed. NTSTATUS=0x{status:X8}",
                "WinTrayMemory",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }

    #endregion
}
