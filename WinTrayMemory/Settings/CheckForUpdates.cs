using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Notifications;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.Notifications;

namespace WinTrayMemory.Settings;

public class CheckForUpdates
{
    private readonly WindowsNotification _notification;
    private readonly IAppSettingsService _settingsService;
    private const string RepoOwner = "Rywent";
    private const string RepoName = "WinTrayMemory";

    public CheckForUpdates(WindowsNotification notification, IAppSettingsService settingsService)
    {
        _notification = notification;
        _settingsService = settingsService;
    }

    public async Task CheckForUpdatesAsync()
    {
        var settings = await _settingsService.GetSettingsAsync();
        if (!settings.IsCheckForUpdates)
        {
            return;
        }

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("WinTrayMemory");

            var json = await client.GetStringAsync(
                $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest");

            using var doc = JsonDocument.Parse(json);
            var tagName = doc.RootElement.GetProperty("tag_name").GetString()!;
            var htmlUrl = doc.RootElement.GetProperty("html_url").GetString()!;

            var versionString = tagName
                .Replace("release-", "")
                .Replace("v", "")
                .Trim();

            var latestVersion = new Version(versionString);
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version!;


            if (latestVersion > currentVersion)
            {
                _notification.Show(
                    "Update Available!",
                    $"v{latestVersion} is available.\nYou have v{currentVersion}.",
                    onDownload: () =>
                    {
                        Process.Start(new ProcessStartInfo(htmlUrl) { UseShellExecute = true });
                    });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Update] Check failed: {ex.Message}");
        }
    }
}
