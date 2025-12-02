using System.Windows;
using System.ComponentModel;

using WinTrayMemory.Shell;

namespace WinTrayMemory;

public partial class MainWindow : Window
{

    private bool _firstShow = true;
    private bool _closeApp;

    public MainWindow()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
            throw;
        }


        Loaded += (_, __) => Hide();
    }

    private void TrayIcon_OnClick(object sender, RoutedEventArgs e)
    {
        if (IsVisible)
        {
            Hide();
        }
        else
        {
            if (_firstShow)
            {
                _firstShow = false;
                Width = 315;
                Height = 210;
            }

            PositionNearTray();
            Show();
            Activate();
        }
    }

    private void PositionNearTray()
    {
        var wa = SystemParameters.WorkArea;
        Left = wa.Right - Width - 38;
        Top = wa.Bottom - Height - 16;
    }
    public void CloseFromViewModel()
    {
        _closeApp = true;
        Close();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_closeApp)
        {
            base.OnClosing(e);
            return;
        }

        e.Cancel = true;
        Hide();
    }
}
