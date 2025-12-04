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
}
