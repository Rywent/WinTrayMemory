using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WinTrayMemory.Data.Interfaces;
using WinTrayMemory.Data.Persistence.Repositories;
using WinTrayMemory.Data.Services;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.MainPage;
using WinTrayMemory.Notifications;
using WinTrayMemory.Settings;
using WinTrayMemory.SettingsPage;
using WinTrayMemory.Shell;

namespace WinTrayMemory.Hosting;

/// <summary>
/// сentral application host configuration and builder
/// </summary>
public static class AppHost
{
    /// <summary>
    /// builds and configures the main application host with all required services, repositories and view models
    /// </summary>
    /// <param name="args">command line arguments passed to the application</param>
    /// <returns>configured IHost instance</returns>
    public static IHost Build(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDatabase();

                services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
                services.AddScoped<IProcessRuleRepository, ProcessRuleRepository>();

                services.AddScoped<IAppSettingsService, AppSettingsService>();
                services.AddScoped<IProcessRuleService, ProcessRuleService>();

                services.AddSingleton<WindowsNotification>();
                services.AddSingleton<CheckForUpdates>();
                services.AddSingleton<SettingsChangedEvent>();


                services.AddTransient<MainPageViewModel>();
                services.AddTransient<SettingsPageViewModel>();
                services.AddTransient<MainViewModel>();

            })
            .Build();
    }
}
