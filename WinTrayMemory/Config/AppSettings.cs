using CommunityToolkit.Mvvm.ComponentModel;

namespace WinTrayMemory.Config;

public sealed partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private List<string> safely = new();

    [ObservableProperty]
    private List<string> warning = new();

    [ObservableProperty]
    private List<string> dangerous = new();

    [ObservableProperty]
    private decimal minHeavyProcessSizeMb = 500;

    [ObservableProperty]
    private int maxProcessesShown = 10;

    [ObservableProperty]
    private int refreshIntervalSec = 3;

    [ObservableProperty]
    private bool cleanWorkingSet = true;

    [ObservableProperty]
    private bool cleanLowPriorityStandby = true;

    [ObservableProperty]
    private bool cleanStandbyList = false;

    [ObservableProperty]
    private bool cleanModifiedPageList = false;

    [ObservableProperty]
    private bool runOnStartup = false;
}
