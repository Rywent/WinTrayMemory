namespace WinTrayMemory.Processes;

public class DefaultProcesses
{
    public static readonly Dictionary<string, DeterminingProcessType.ProcessType> Rules = new()
    {
        /// <summary>
        /// System processes
        /// </summary>
        ["system"] = DeterminingProcessType.ProcessType.Dangerous,
        ["system idle process"] = DeterminingProcessType.ProcessType.Dangerous,
        ["winlogon"] = DeterminingProcessType.ProcessType.Dangerous,
        ["csrss"] = DeterminingProcessType.ProcessType.Dangerous,
        ["smss"] = DeterminingProcessType.ProcessType.Dangerous,
        ["services"] = DeterminingProcessType.ProcessType.Dangerous,
        ["lsass"] = DeterminingProcessType.ProcessType.Dangerous,
        ["svchost"] = DeterminingProcessType.ProcessType.Dangerous,
        ["wininit"] = DeterminingProcessType.ProcessType.Dangerous,
        ["spoolsv"] = DeterminingProcessType.ProcessType.Dangerous,
        ["dwm"] = DeterminingProcessType.ProcessType.Dangerous,
        ["taskhostw"] = DeterminingProcessType.ProcessType.Dangerous,
        ["shellexperiencehost"] = DeterminingProcessType.ProcessType.Dangerous,
        ["startmenuexperiencehost"] = DeterminingProcessType.ProcessType.Dangerous,
        ["runtimebroker"] = DeterminingProcessType.ProcessType.Dangerous,
        ["searchindexer"] = DeterminingProcessType.ProcessType.Dangerous,
        ["securityhealthservice"] = DeterminingProcessType.ProcessType.Dangerous,
        ["securityhealthsystray"] = DeterminingProcessType.ProcessType.Dangerous,
        ["sihost"] = DeterminingProcessType.ProcessType.Dangerous,
        ["fontdrvhost"] = DeterminingProcessType.ProcessType.Dangerous,
        ["explorer"] = DeterminingProcessType.ProcessType.Dangerous,
        ["audiodg"] = DeterminingProcessType.ProcessType.Dangerous,
        ["ctfmon"] = DeterminingProcessType.ProcessType.Dangerous,
        ["msmpeng"] = DeterminingProcessType.ProcessType.Dangerous,
        ["ntoskrnl"] = DeterminingProcessType.ProcessType.Dangerous,
        ["registry"] = DeterminingProcessType.ProcessType.Dangerous,

        /// <summary>
        /// Critical processes
        /// </summary>
        ["chrome"] = DeterminingProcessType.ProcessType.Warning,
        ["firefox"] = DeterminingProcessType.ProcessType.Warning,
        ["msedge"] = DeterminingProcessType.ProcessType.Warning,
        ["opera"] = DeterminingProcessType.ProcessType.Warning,
        ["brave"] = DeterminingProcessType.ProcessType.Warning,
        ["devenv"] = DeterminingProcessType.ProcessType.Warning,
        ["code"] = DeterminingProcessType.ProcessType.Warning,
        ["rider"] = DeterminingProcessType.ProcessType.Warning,
        ["idea64"] = DeterminingProcessType.ProcessType.Warning,
        ["discord"] = DeterminingProcessType.ProcessType.Warning,
        ["slack"] = DeterminingProcessType.ProcessType.Warning,
        ["teams"] = DeterminingProcessType.ProcessType.Warning,
        ["spotify"] = DeterminingProcessType.ProcessType.Warning,
        ["steam"] = DeterminingProcessType.ProcessType.Warning,
        ["epicgameslauncher"] = DeterminingProcessType.ProcessType.Warning,
        ["obs64"] = DeterminingProcessType.ProcessType.Warning,
        ["photoshop"] = DeterminingProcessType.ProcessType.Warning,
        ["afterfx"] = DeterminingProcessType.ProcessType.Warning,
        ["premiere pro"] = DeterminingProcessType.ProcessType.Warning,
        ["blender"] = DeterminingProcessType.ProcessType.Warning,
        ["unity"] = DeterminingProcessType.ProcessType.Warning,
        ["unreal"] = DeterminingProcessType.ProcessType.Warning,


        /// <summary>
        /// Non-essential processes 
        /// </summary>
        ["notepad"] = DeterminingProcessType.ProcessType.Safely,
        ["calc"] = DeterminingProcessType.ProcessType.Safely,
        ["mspaint"] = DeterminingProcessType.ProcessType.Safely,
        ["cmd"] = DeterminingProcessType.ProcessType.Safely,
        ["powershell"] = DeterminingProcessType.ProcessType.Safely,
        ["snippingtool"] = DeterminingProcessType.ProcessType.Safely,
        ["taskmgr"] = DeterminingProcessType.ProcessType.Safely,
        ["winword"] = DeterminingProcessType.ProcessType.Safely,
        ["excel"] = DeterminingProcessType.ProcessType.Safely,
        ["powerpnt"] = DeterminingProcessType.ProcessType.Safely,
        ["onedrive"] = DeterminingProcessType.ProcessType.Safely,
        ["githubdesktop"] = DeterminingProcessType.ProcessType.Safely,
        ["7z"] = DeterminingProcessType.ProcessType.Safely,
        ["winrar"] = DeterminingProcessType.ProcessType.Safely,
        ["notepad++"] = DeterminingProcessType.ProcessType.Safely,
        ["everything"] = DeterminingProcessType.ProcessType.Safely,
        ["vnc"] = DeterminingProcessType.ProcessType.Safely,
        ["anydesk"] = DeterminingProcessType.ProcessType.Safely,
        ["teamviewer"] = DeterminingProcessType.ProcessType.Safely,
    };
}
