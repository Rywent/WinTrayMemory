using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows;

namespace WinTrayMemory.Notifications;

public class WindowsNotification
{
    private Action? _onDownloadAction;

    public WindowsNotification()
    {
        ToastNotificationManagerCompat.OnActivated += OnActivated;
    }

    public void Show(string title, string message, Action? onDownload = null)
    {
        _onDownloadAction = onDownload;

        var builder = new ToastContentBuilder()
            .AddText(title)
            .AddText(message);

        if (onDownload != null)
        {
            builder.AddButton("Download", ToastActivationType.Foreground, "download");
        }

        builder.Show();
    }

    private void OnActivated(ToastNotificationActivatedEventArgsCompat args)
    {
        if (args.Argument == "download")
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                _onDownloadAction?.Invoke();
            });
        }
    }

    public void Show(string title, string message)
    {
        Show(title, message, null);
    }
}