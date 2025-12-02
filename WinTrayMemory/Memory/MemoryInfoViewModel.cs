using CommunityToolkit.Mvvm.ComponentModel;
using System.Management;
using System.Windows.Threading;


namespace WinTrayMemory.Memory;

public partial class MemoryInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private decimal totalMemory;

    [ObservableProperty]
    private decimal usedMemory;

    [ObservableProperty]
    private decimal usedPercentage;

    private readonly DispatcherTimer _timer;
    private readonly MemoryInfoService _memoryService;


    /// <summary>
    /// initializes the memory info view model and starts periodic ram updates.
    /// </summary>
    public MemoryInfoViewModel()
    {
        _memoryService = new MemoryInfoService();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (s, e) => UpdateMemory();

        LoadTotalMemory();
        UpdateMemory();
        _timer.Start();
    }

    /// <summary>
    /// loads total physical memory (ram) of the user's pc in gigabytes.
    /// </summary>
    private void LoadTotalMemory()
    {
        using var searcher = new ManagementObjectSearcher(
            "SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            decimal totalKb = Convert.ToDecimal(obj["TotalVisibleMemorySize"]);
            TotalMemory = totalKb / 1024m / 1024m;
        }
    }

    /// <summary>
    /// updates used memory values and usage percentage.
    /// </summary>
    private void UpdateMemory()
    {
        var (totalGb, usedGb, usedPercent) = _memoryService.GetMemoryInfo();

        TotalMemory = totalGb;
        UsedMemory = usedGb;
        UsedPercentage = usedPercent;
    }
}
