using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WinTrayMemory.Memory;

namespace WinTrayMemory.MainPage.Components.MemoryDetailingChart;

public partial class MemoryDetailingChartViewModel : ObservableObject
{

    public ObservableCollection<ISeries> PieSeries { get; set; }

    private readonly DispatcherTimer _timer;
    private readonly MemoryInfoService _memoryService;

    [ObservableProperty] private Paint _tooltipTextPaint = new SolidColorPaint(SKColors.White);
    [ObservableProperty] private Paint _legendTextPaint = new SolidColorPaint(SKColors.White);

    [ObservableProperty] private decimal privateVal;
    [ObservableProperty] private decimal sharedVal;
    [ObservableProperty] private decimal systemVal;


    public MemoryDetailingChartViewModel()
    {
        PieSeries = new ObservableCollection<ISeries>();

        _memoryService = new MemoryInfoService();
        UpdateData();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (s, e) => UpdateData();
        _timer.Start();
    }

    /// <summary>
    /// Initialization and updating of the pie diagram
    /// </summary>
    private void UpdateData()
    {
        var (privateGb, sharedGb, systemGb) = _memoryService.GetMemoryBreakdown();
        PrivateVal = privateGb;
        SharedVal = sharedGb;
        SystemVal = systemGb;

        PieSeries = new ObservableCollection<ISeries>();

        PieSeries.Clear();
        PieSeries.Add(new PieSeries<decimal>
        {
            Values = new[] { PrivateVal },
            Name = "Applications (Private)",
            Fill = new SolidColorPaint(SKColor.Parse("#E74C3C")),
        });
        PieSeries.Add(new PieSeries<decimal>
        {
            Values = new[] { SharedVal },
            Name = "Resources (Shared)",
            Fill = new SolidColorPaint(SKColor.Parse("#2ECC71")),
        });
        PieSeries.Add(new PieSeries<decimal>
        {
            Values = new[] { SystemVal },
            Name = "System",
            Fill = new SolidColorPaint(SKColor.Parse("#95A5A6")),
        });
    }
}
