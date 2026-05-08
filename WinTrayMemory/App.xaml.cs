using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.Hosting;
using WinTrayMemory.Settings;
using WinTrayMemory.Shell;

namespace WinTrayMemory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _appHost;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _appHost = AppHost.Build(Environment.GetCommandLineArgs());
            await _appHost.InitializeDatabaseAsync();
            await _appHost.StartAsync();

            var mainViewModel = _appHost.Services.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();

            var updateService = _appHost.Services.GetRequiredService<CheckForUpdates>();
            _ = updateService.CheckForUpdatesAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_appHost != null)
            {
                await _appHost.StopAsync(TimeSpan.FromSeconds(5));
                _appHost.Dispose();
            }

            base.OnExit(e);
        }
    }

}
