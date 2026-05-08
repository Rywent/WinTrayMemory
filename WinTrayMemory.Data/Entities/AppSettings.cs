using System.ComponentModel.DataAnnotations;

namespace WinTrayMemory.Data.Entities;

public class AppSettings
{
    [Key]
    public Guid Id { get; set; }
    public bool IsAutoCleanEnabled { get; set; } = false;
    public int Threshold { get; set; } = 60;
    public bool ShowNotification { get; set; } = true;
    public bool AllowKillSystemProcesses { get; set; } = false;
    public bool IsRunOnStartup { get; set; } = false;
    public bool IsCheckForUpdates { get; set; } = true;
    public int MinProcessSize { get; set; } = 500;
    public int MaxProcessesShown { get; set; } = 15;
    public int RefreshInterval { get; set; } = 3;
    public bool CleanWorkingSet { get; set; } = true;
    public bool CleanLowPriorityStandby { get; set; } = false;
    public bool CleanStandbyList { get; set; } = false;
    public bool CleanModifiedPageList { get; set; } = false;
}
