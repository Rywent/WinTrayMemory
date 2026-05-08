using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinTrayMemory.MainPage
{
    /// <summary>
    /// Логика взаимодействия для MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        public MainPageView()
        {
            InitializeComponent();
        }

        private void OpenGitHub(object sender, MouseButtonEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Rywent",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
