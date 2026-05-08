
using static WinTrayMemory.Processes.DeterminingProcessType;

namespace WinTrayMemory.Processes;

public sealed record UserProcessInfo(
    string Name,
    string ClueMessage,
    ProcessType Category);
