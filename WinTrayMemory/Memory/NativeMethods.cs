using System.Runtime.InteropServices;

namespace WinTrayMemory.Memory;

internal static partial class NativeMethods
{
    internal const int SystemMemoryListInformation = 80;

    internal enum SystemMemoryListCommand
    {
        MemoryPurgeStandbyList = 4,
        MemoryPurgeLowPriorityStandbyList = 5,
        MemoryPurgeModifiedList = 3
    }

    [LibraryImport("psapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool EmptyWorkingSet(IntPtr hProcess);

    [LibraryImport("ntdll.dll")]
    internal static partial int NtSetSystemInformation(int systemInformationClass, IntPtr systemInformation, int systemInformationLength);
}