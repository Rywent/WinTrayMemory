using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using WinTrayMemory.Memory;
namespace WinTrayMemory.MainPage.Components.RamHistoryChart;

public partial class RamHistoryChartViewModel : ObservableObject
{
    

    [ObservableProperty] private ISeries[] _series;

    [ObservableProperty] private Axis[] _xAxes;

    [ObservableProperty] private Axis[] _yAxes;
    
    [ObservableProperty] private Paint _tooltipTextPaint = new SolidColorPaint(SKColors.White);

    public ObservableCollection<ObservablePoint> RamPoints { get; set; }

    private readonly DispatcherTimer _timer;
    private readonly MemoryInfoService _memoryService;


    public RamHistoryChartViewModel()
    {
        Init();
     

        _memoryService = new MemoryInfoService();


        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (s, e) => UpdateData();
        _timer.Start();
    }

    /// <summary>
    /// Initialization of the linear diagram
    /// </summary>
    private void Init()
    {
        RamPoints = new ObservableCollection<ObservablePoint>();

        for (int i = 0; i <= 60; i++)
        {
            RamPoints.Add(new ObservablePoint(i, 0));
        }

        var line = new LineSeries<ObservablePoint>
        {
            Values = RamPoints,
            Fill = new LinearGradientPaint(
                new[] {SKColors.SteelBlue.WithAlpha(90), SKColors.Transparent},
                new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(SKColors.SteelBlue, 3),
            GeometrySize = 0,
            LineSmoothness = 1,
            Name = "RAM Usage"
        };

        Series = new ISeries[] { line };

        XAxes = new Axis[]
        {
            new Axis
            {
                Labeler = value => $"{60 - value:F0}s",
                LabelsPaint = new SolidColorPaint(SKColors.White),
                TextSize = 10,
                MinLimit = 0,
                MaxLimit = 60
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                Labeler = value => $"{value:F0}%",
                 LabelsPaint = new SolidColorPaint(SKColors.White),
                TextSize = 10,
                MinLimit = 0,
                MaxLimit = 100
            }
        };
    }

    /// <summary>
    /// creating new memory usage points on the linear diagram
    /// </summary>
    private void UpdateData()
    {
        var (_, _, usedPercent) = _memoryService.GetMemoryInfo();

        for (int i = 0; i < RamPoints.Count - 1; i++)
        {
            RamPoints[i].Y = RamPoints[i + 1].Y;
        }
        RamPoints.Last().Y = (double)usedPercent;
    }
}
