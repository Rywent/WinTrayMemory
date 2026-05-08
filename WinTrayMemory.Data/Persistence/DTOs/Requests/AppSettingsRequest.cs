
namespace WinTrayMemory.Data.Persistence.DTOs.Requests;

public record AppSettingsRequest(
    int? Threshold = null,
    bool? ShowNotification = null,
    bool? IsRunOnStartup = null,
    bool? AllowKillSystemProcesses = null,
    bool? IsCheckForUpdates = null,
    int? MinProcessSize = null,
    int? MaxProcessesShown = null,
    int? RefreshInterval = null,
    bool? CleanWorkingSet = null,
    bool? CleanLowPriorityStandby = null,
    bool? CleanStandbyList = null,
    bool? CleanModifiedPageList = null,
    bool? IsAutoCleanEnabled = null
);