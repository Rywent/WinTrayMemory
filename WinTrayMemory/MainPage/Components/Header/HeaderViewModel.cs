using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using WinTrayMemory.MainPage.Components.Messages;

namespace WinTrayMemory.MainPage.Components.Header;

public partial class HeaderViewModel : ObservableObject
{

    /// <summary>
    /// close application
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
    /// sends message to open settings page
    /// </summary>

    [RelayCommand]
    public void ShowSettings()
    {
        WeakReferenceMessenger.Default.Send(new OpenSettingsPageMessage(true));
    }
}
