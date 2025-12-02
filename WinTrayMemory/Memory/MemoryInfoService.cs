using System.Management;

namespace WinTrayMemory.Memory;

public class MemoryInfoService
{
    public (decimal totalGb, decimal usedGb, decimal usedPercent) GetMemoryInfo()
    {
        using var searcher = new ManagementObjectSearcher(
            "SELECT FreePhysicalMemory, TotalVisibleMemorySize FROM Win32_OperatingSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            decimal totalKb = Convert.ToDecimal(obj["TotalVisibleMemorySize"]);
            decimal freeKb = Convert.ToDecimal(obj["FreePhysicalMemory"]);

            decimal totalGb = totalKb / 1024m / 1024m;
            decimal freeGb = freeKb / 1024m / 1024m;
            decimal usedGb = totalGb - freeGb;

            decimal usedPercent = usedGb / totalGb * 100m;
            return (totalGb, usedGb, usedPercent);
        }

        return (0, 0, 0);
    }
}
