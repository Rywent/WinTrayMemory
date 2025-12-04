using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static WinTrayMemory.Processes.DeterminingProcessType;

namespace WinTrayMemory.Resources.View.UserControls;

public sealed partial class TaskLine : UserControl
{
    public static readonly DependencyProperty TaskNameProperty =
        DependencyProperty.Register(
            nameof(TaskName),
            typeof(string),
            typeof(TaskLine),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TaskCountProperty =
        DependencyProperty.Register(
            nameof(TaskCount),
            typeof(int),
            typeof(TaskLine),
            new PropertyMetadata(0));

    public static readonly DependencyProperty TaskClueProperty =
        DependencyProperty.Register(
            nameof(TaskClueMessage),
            typeof(string),
            typeof(TaskLine),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty TaskMemoryUsesProperty =
        DependencyProperty.Register(
            nameof(TaskMemoryUses),
            typeof(long),
            typeof(TaskLine),
            new PropertyMetadata(0L));

    public static readonly DependencyProperty TaskCategoryProperty =
        DependencyProperty.Register(
            nameof(TaskCategory),
            typeof(ProcessType),
            typeof(TaskLine),
            new PropertyMetadata(null));

    public static readonly DependencyProperty KillCommandProperty =
        DependencyProperty.Register(
            nameof(KillCommand),
            typeof(ICommand),
            typeof(TaskLine),
            new PropertyMetadata(null));

    public string TaskName
    {
        get => (string)GetValue(TaskNameProperty);
        set => SetValue(TaskNameProperty, value);
    }

    public int TaskCount
    {
        get => (int)GetValue(TaskCountProperty);
        set => SetValue(TaskCountProperty, value);
    }

    public string TaskClueMessage
    {
        get => (string)GetValue(TaskClueProperty);
        set => SetValue(TaskClueProperty, value);
    }

    public long TaskMemoryUses
    {
        get => (long)GetValue(TaskMemoryUsesProperty);
        set => SetValue(TaskMemoryUsesProperty, value);
    }

    public ProcessType TaskCategory
    {
        get => (ProcessType)GetValue(TaskCategoryProperty);
        set => SetValue(TaskCategoryProperty, value);
    }

    public ICommand KillCommand
    {
        get => (ICommand)GetValue(KillCommandProperty);
        set => SetValue(KillCommandProperty, value);
    }

    public TaskLine()
    {
        InitializeComponent();
    }
}
