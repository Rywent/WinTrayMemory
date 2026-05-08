using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static WinTrayMemory.Processes.DeterminingProcessType;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.Memory;
using WinTrayMemory.Processes;

namespace WinTrayMemory.MainPage.Components.HeaviestProcesses;

public sealed partial class HeaviestProcessesViewModel : ObservableObject
{
    private DispatcherTimer _timer;
    private readonly ProcessDataProvider _monitor;
    private readonly IAppSettingsService _settingsService;
    private readonly IProcessRuleService _processRuleService;

    private int _refreshInterval = 3;

    [ObservableProperty]
    private ObservableCollection<ProcessInfo> processes = new();

    /// <summary>
    /// initializes the view model and starts a timer to refresh the heaviest processes list.
    /// </summary>
    /// <param name="memory">memory info view model used to display ram usage.</param>
    public HeaviestProcessesViewModel(IAppSettingsService settingsSerice, IProcessRuleService processRule)
    {
        _settingsService = settingsSerice;
        _processRuleService = processRule;
        _monitor = new ProcessDataProvider(_settingsService, _processRuleService);

        _timer = new DispatcherTimer();
        _timer.Tick += async (_, _) => await RefreshProcesses();

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        _refreshInterval = settings.RefreshInterval;

        _timer.Interval = TimeSpan.FromSeconds(_refreshInterval);
        _timer.Start();

        await RefreshProcesses();
    }

    /// <summary>
    /// kills the selected process and removes it from the list.
    /// </summary>
    /// <param name="process">process to kill.</param>
    [RelayCommand]
    private async Task KillProcess(ProcessInfo? process)
    {
        if (process is null)
        {
            MessageBox.Show("Process not found. Cannot kill process.");
            return;
        }

        var settings = await _settingsService.GetSettingsAsync();

        if (process.Category == ProcessType.Warning)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to kill '{process.Name}'?\nIt may contain unsaved data.",
                "WinTrayMemory",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;
        }

        if (process.Category == ProcessType.Dangerous)
        {
            if (!settings.AllowKillSystemProcesses)
            {
                MessageBox.Show(
                    "This is a critical system process. Killing it may crash your system.\n\n" +
                    "To unlock this, enable 'Allow killing system processes' in Settings -> General.",
                    "WinTrayMemory",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }

            var result = MessageBox.Show(
                $"WARNING! You are about to kill a critical system process: '{process.Name}'.\n" +
                "This may cause system instability or crash.\n\nAre you absolutely sure?",
                "WinTrayMemory",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;
        }

        try
        {
            var processes = Process.GetProcessesByName(process.Name);
            foreach (var p in processes)
            {
                p.Kill();
            }

            Processes.Remove(process);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error killing process: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    /// <summary>
    /// refreshes the list of current processes and their memory usage.
    /// </summary>
    private async Task RefreshProcesses()
    {
        var data = await Task.Run(() => _monitor.GetHeaviestProcesses());

        Application.Current.Dispatcher.Invoke(() =>
        {
            Processes.Clear();
            foreach (var item in data)
            {
                Processes.Add(item);
            }
        });


    }

    /// <summary>
    /// Update refresh interval
    /// </summary>
    /// <param name="seconds"></param>
    public void UpdateRefreshInterval(int seconds)
    {
        _refreshInterval = seconds;
        _timer.Interval = TimeSpan.FromSeconds(_refreshInterval);
    }
    public async Task RefreshMonitorSettingsAsync()
    {
        await _monitor.RefreshSettingsAsync();
    }

}
