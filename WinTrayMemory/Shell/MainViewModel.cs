using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using WinTrayMemory.Config;
using WinTrayMemory.Settings;
using WinTrayMemory.HeaviestProcesses;
using WinTrayMemory.Memory;

namespace WinTrayMemory.Shell;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? _currentView;

    public HeaviestProcessesViewModel HeaviestProcessesViewModel { get; }
    public MemoryInfoViewModel MemoryInfoViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }

    private readonly AppSettings _settings;

    /// <summary>
    /// initializes the main view model and loads application settings.
    /// </summary>
    public MainViewModel()
    {
        _settings = SettingsService.Load();

        MemoryInfoViewModel = new MemoryInfoViewModel();
        HeaviestProcessesViewModel  = new HeaviestProcessesViewModel(_settings, MemoryInfoViewModel);
        SettingsViewModel = new SettingsViewModel(_settings);

        CurrentView = HeaviestProcessesViewModel;
    }

    /// <summary>
    /// switches shell view to the heaviest processes view.
    /// </summary>
    [RelayCommand]
    public void ShowHeaviestProcesses() => CurrentView = HeaviestProcessesViewModel;

    /// <summary>
    /// switches shell view to the settings view.
    /// </summary>
    [RelayCommand]
    public void ShowSettings() => CurrentView = SettingsViewModel;
}
