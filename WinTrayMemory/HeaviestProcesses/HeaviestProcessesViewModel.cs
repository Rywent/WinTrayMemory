using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using WinTrayMemory.Config;
using WinTrayMemory.Memory;
using WinTrayMemory.Processes;

namespace WinTrayMemory.HeaviestProcesses;

public sealed partial class HeaviestProcessesViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer;
    private readonly ProcessDataProvider _monitor;
    private readonly AppSettings _settings;

    public MemoryInfoViewModel Memory { get; }

    [ObservableProperty]
    private ObservableCollection<ProcessInfo> processes = new();

    /// <summary>
    /// initializes the view model and starts a timer to refresh the heaviest processes list.
    /// </summary>
    /// <param name="settings">application settings used for process monitoring and refresh interval.</param>
    /// <param name="memory">memory info view model used to display ram usage.</param>
    public HeaviestProcessesViewModel(AppSettings settings, MemoryInfoViewModel memory)
    {
        _settings = settings;
        Memory = memory;
        _monitor = new ProcessDataProvider(_settings);

        _settings.PropertyChanged += OnSettingsChanged;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(_settings.RefreshIntervalSec)
        };
        _timer.Tick += async (_, _) => await RefreshProcesses();
        _timer.Start();

        _ = RefreshProcesses();
    }

    /// <summary>
    /// kills the selected process and removes it from the list.
    /// </summary>
    /// <param name="process">process to kill.</param>
    [RelayCommand]
    private void KillProcess(ProcessInfo? process)
    {
        if (process is null)
        {
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
    /// closes the application
    /// </summary>
    [RelayCommand]
    private void CloseApp()
    {
        if (Application.Current.MainWindow is MainWindow mw)
        {
            mw.CloseFromViewModel();
        }
        else
        {
            Application.Current.Shutdown();
        }
    }

    /// <summary>
    /// starts memory cleaning using the types selected in the settings.
    /// </summary>

    [RelayCommand]
    private void SmartCleaning()
    {
        if (!MemoryCleaner.IsAdministrator())
        {
            MessageBox.Show("Run as administrator to clean memory.", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (_settings.CleanWorkingSet)
            {
                MemoryCleaner.TrimAllWorkingSets();
            }

            if (_settings.CleanLowPriorityStandby)
            {
                MemoryCleaner.PurgeLowPriorityStandbyList();
            }

            if (_settings.CleanStandbyList)
            {
                MemoryCleaner.PurgeStandbyList();
            }

            if (_settings.CleanModifiedPageList)
            {
                MemoryCleaner.PurgeModifiedPageList();
            }

            MessageBox.Show("Cleaning completed!", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cleaning error: {ex.Message}", "WinTrayMemory", MessageBoxButton.OK, MessageBoxImage.Error);
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

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppSettings.RefreshIntervalSec))
        {
            _timer.Interval = TimeSpan.FromSeconds(_settings.RefreshIntervalSec);
        }
    }

}
