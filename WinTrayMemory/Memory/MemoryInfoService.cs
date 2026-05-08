using System.Diagnostics;
using System.Management;
using System.Windows;

namespace WinTrayMemory.Memory;

public sealed class MemoryInfoService
{
    public (decimal totalGb, decimal usedGb, decimal usedPercent) GetMemoryInfo()
    {
        using var searcher = new ManagementObjectSearcher(
            "SELECT FreePhysicalMemory, TotalVisibleMemorySize FROM Win32_OperatingSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            if (obj is ManagementObject)
            {
                decimal totalKb = Convert.ToDecimal(obj["TotalVisibleMemorySize"]);
                decimal freeKb = Convert.ToDecimal(obj["FreePhysicalMemory"]);

                decimal totalGb = totalKb / 1024m / 1024m;
                decimal freeGb = freeKb / 1024m / 1024m;
                decimal usedGb = totalGb - freeGb;

                decimal usedPercent = usedGb / totalGb * 100m;
                return (totalGb, usedGb, usedPercent);
            }
            else
            {
                MessageBox.Show("Unable to cast ManagementBaseObject to ManagementObject while reading memory info.", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            
        }

        return (0, 0, 0);
    }

    public (decimal privateWorkingSetGb, decimal sharedWorkingSetGb, decimal systemAndCacheGb) GetMemoryBreakdown()
    {
        long totalPrivateWs = 0;
        long totalWorkingSet = 0;

        foreach (Process proc in Process.GetProcesses())
        {
            try
            {
                long ws = proc.WorkingSet64;
                long priv = proc.PrivateMemorySize64;

                totalWorkingSet += ws;
                totalPrivateWs += Math.Min(priv, ws);
            }
            catch
            {
                continue;
            }
        }

        long sharedWorkingSet = Math.Max(0, totalWorkingSet - totalPrivateWs);
        long totalRam = 0;
        using (var searcher = new ManagementObjectSearcher(
            "SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem"))
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                totalRam = Convert.ToInt64(obj["TotalVisibleMemorySize"]) * 1024;
                break;
            }
        }

        long systemAndCache = Math.Max(0, totalRam - totalPrivateWs - sharedWorkingSet);

        return (
            Math.Round(totalPrivateWs / 1_000_000_000m, 2),
            Math.Round(sharedWorkingSet / 1_000_000_000m, 2),
            Math.Round(systemAndCache / 1_000_000_000m, 2)
        );
    }

    public (int commitPercent, int hardFaults) GetMemoryPressure()
    {
        using var searcher = new ManagementObjectSearcher(
            "SELECT CommitLimit, CommittedBytes, PagesInputPerSec, PagesOutputPerSec FROM Win32_PerfFormattedData_PerfOS_Memory");

        var obj = searcher.Get().Cast<ManagementObject>().First();

        long commitLimit = Convert.ToInt64(obj["CommitLimit"]);
        long committed = Convert.ToInt64(obj["CommittedBytes"]);
        int pagesIn = Convert.ToInt32(obj["PagesInputPerSec"]);
        int pagesOut = Convert.ToInt32(obj["PagesOutputPerSec"]);

        int commitPercent = (int)((decimal)committed / commitLimit * 100);
        int hardFaults = pagesIn + pagesOut;

        return (commitPercent, hardFaults);
    }
}
