using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using WinTrayMemory.SettingsPage.Components.Messages;

namespace WinTrayMemory.SettingsPage.Components.Header;

public partial class HeaderViewModel : ObservableObject
{
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

    [RelayCommand]
    public void ShowSettings()
    {
        WeakReferenceMessenger.Default.Send(new OpenMonitoringPageMessage(true));
    }
}
