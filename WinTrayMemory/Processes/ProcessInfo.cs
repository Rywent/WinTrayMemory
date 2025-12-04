using static WinTrayMemory.Processes.DeterminingProcessType;

namespace WinTrayMemory.Processes;


public sealed record ProcessInfo(
    string Name,
    string ClueMessage,
    int Count,
    decimal MemoryUses,
    ProcessType Category);
