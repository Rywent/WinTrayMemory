using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static WinTrayMemory.Processes.DeterminingProcessType;

namespace WinTrayMemory.Resources.View.UserControls;

public partial class UserProcessLine : UserControl
{
    public static readonly DependencyProperty TaskNameProperty =
        DependencyProperty.Register(
            nameof(TaskName),
            typeof(string),
            typeof(UserProcessLine),
            new PropertyMetadata(string.Empty));


    public static readonly DependencyProperty TaskClueProperty =
        DependencyProperty.Register(
            nameof(TaskClueMessage),
            typeof(string),
            typeof(UserProcessLine),
            new PropertyMetadata(string.Empty));


    public static readonly DependencyProperty TaskCategoryProperty =
        DependencyProperty.Register(
            nameof(TaskCategory),
            typeof(ProcessType),
            typeof(UserProcessLine),
            new PropertyMetadata(null));

    public static readonly DependencyProperty RemoveCommandProperty =
        DependencyProperty.Register(
            nameof(RemoveCommand),
            typeof(ICommand),
            typeof(UserProcessLine),
            new PropertyMetadata(null));

    public string TaskName
    {
        get => (string)GetValue(TaskNameProperty);
        set => SetValue(TaskNameProperty, value);
    }

    public string TaskClueMessage
    {
        get => (string)GetValue(TaskClueProperty);
        set => SetValue(TaskClueProperty, value);
    }

    public ProcessType TaskCategory
    {
        get => (ProcessType)GetValue(TaskCategoryProperty);
        set => SetValue(TaskCategoryProperty, value);
    }

    public ICommand RemoveCommand
    {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }
    public UserProcessLine()
    {
        InitializeComponent();
    }
}
