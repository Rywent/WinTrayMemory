using System.Runtime.InteropServices;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Interop;

namespace WinTrayMemory.Resources.Effects;

public class BlurBehavior : Behavior<FrameworkElement>
{
    protected override void OnAttached()
    {
        AssociatedObject.Loaded += (s, e) => { EnableBlur(); };
    }

    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    private uint _blurBackgroundColor = 0x0D000000;

    public uint BlurOpacity
    {
        get => (uint)GetValue(BlurOpacityProperty);
        set => SetValue(BlurOpacityProperty, value);
    }

    internal void EnableBlur()
    {
        var window = Window.GetWindow(AssociatedObject);
        if (window == null)
            return;

        var windowHelper = new WindowInteropHelper(window);
        if (windowHelper.Handle == IntPtr.Zero)
        {
            window.SourceInitialized += (s, e) => EnableBlur();
            return;
        }

        var accent = new AccentPolicy
        {
            AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND,
            AccentFlags = 0x20 | 0x40,
            GradientColor = _blurBackgroundColor,
            AnimationId = 0
        };

        var size = Marshal.SizeOf(accent);
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(accent, ptr, false);

        var data = new WindowCompositionAttributeData
        {
            Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
            SizeOfData = size,
            Data = ptr
        };

        SetWindowCompositionAttribute(windowHelper.Handle, ref data);
        Marshal.FreeHGlobal(ptr);
    }


    public static readonly DependencyProperty BlurOpacityProperty =
        DependencyProperty.Register(
            nameof(BlurOpacity),
            typeof(uint),
            typeof(BlurBehavior),
            new PropertyMetadata((uint)13, OnBlurOpacityChanged));

    private static void OnBlurOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (BlurBehavior)d;
        uint alphaValue = (uint)e.NewValue;

        behavior._blurBackgroundColor = (alphaValue << 24);
        behavior.EnableBlur();
    }


}

[StructLayout(LayoutKind.Sequential)]
internal struct WindowCompositionAttributeData
{
    public WindowCompositionAttribute Attribute;
    public IntPtr Data;
    public int SizeOfData;
}

internal enum WindowCompositionAttribute
{
    WCA_ACCENT_POLICY = 19
}

internal enum AccentState
{
    ACCENT_DISABLED = 0,
    ACCENT_ENABLE_GRADIENT = 1,
    ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
    ACCENT_ENABLE_BLURBEHIND = 3,
    ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
    ACCENT_INVALID_STATE = 5
}

[StructLayout(LayoutKind.Sequential)]
internal struct AccentPolicy
{
    public AccentState AccentState;
    public uint AccentFlags;
    public uint GradientColor;
    public uint AnimationId;
}