
namespace WinTrayMemory.Settings;

public class SettingsChangedEvent
{
    public event Func<Task>? SettingsChanged;

    public async Task NotifyAsync()
    {
        if (SettingsChanged != null)
            await SettingsChanged.Invoke();
    }
}
