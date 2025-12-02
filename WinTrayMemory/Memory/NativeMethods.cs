using System.Runtime.InteropServices;

namespace WinTrayMemory.Memory;

internal static class NativeMethods
{
    internal const int SystemMemoryListInformation = 80;

    internal enum SystemMemoryListCommand
    {
        MemoryPurgeStandbyList = 4,
        MemoryPurgeLowPriorityStandbyList = 5,
        MemoryPurgeModifiedList = 3
    }

    [DllImport("psapi.dll", SetLastError = true)]
    internal static extern bool EmptyWorkingSet(IntPtr hProcess);

    [DllImport("ntdll.dll")]
    internal static extern int NtSetSystemInformation(int systemInformationClass, IntPtr systemInformation, int systemInformationLength);
}