using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WinTrayMemory.Memory;

namespace WinTrayMemory.MainPage.Components.MemoryPressure;

public partial class MemoryPressureViewModel : ObservableObject
{
    [ObservableProperty] private ISeries[] _gaugeSeries;
    [ObservableProperty] private string _pressureLabel = string.Empty;
    [ObservableProperty] private string _pressureStatus = string.Empty;
    [ObservableProperty] private string _hardFaultsLabel = string.Empty;
    [ObservableProperty] private string _hardFaultsColor = "#666666";

    private readonly DispatcherTimer _timer;
    private readonly MemoryInfoService _memoryService;

    public MemoryPressureViewModel()
    {
        _memoryService = new MemoryInfoService();
        UpdateData();


        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
        _timer.Tick += (s, e) => UpdateData();
        _timer.Start();
    }

    /// <summary>
    /// updating of the gauge diagram
    /// </summary>
    private void UpdateData()
    {
        var (commitPercent, hardFaults) = _memoryService.GetMemoryPressure();


        double pressure = commitPercent;
        if (hardFaults > 0)
        {
            pressure = Math.Min(100, commitPercent + hardFaults / 10.0);
        }

        PressureLabel = $"{commitPercent}%";
        PressureStatus = commitPercent switch
        {
            < 50 => "Low",
            < 70 => "Moderate",
            < 85 => "High",
            _ => "Critical"
        };

        HardFaultsLabel = hardFaults > 0
            ? $"{hardFaults} pg/s"
            : "0 pg/s";
        HardFaultsColor = hardFaults switch
        {
            0 => "#666666",
            < 50 => "#F1C40F",
            < 200 => "#E67E22", 
            _ => "#E74C3C"
        };

        BuildGauge(pressure, commitPercent, hardFaults);
    }

    /// <summary>
    /// build gauge diagram
    /// </summary>
    /// <param name="pressure"></param>
    /// <param name="commitPercent"></param>
    /// <param name="hardFaults"></param>
    private void BuildGauge(double pressure, int commitPercent, int hardFaults)
    {
        var color = commitPercent switch
        {
            < 50 => SKColor.Parse("#2ECC71"),
            < 70 => SKColor.Parse("#F1C40F"),
            < 85 => SKColor.Parse("#E67E22"),
            _ => SKColor.Parse("#E74C3C")
        };

        string centerText = hardFaults > 0
            ? $"{pressure:F0}%"
            : $"{pressure:F0}%";

        GaugeSeries = new ISeries[]
        {
            new PieSeries<double>
            {
                Values = new double[] { 100 },
                InnerRadius = 50,
                MaxRadialColumnWidth = 6,
                Fill = new SolidColorPaint(SKColor.Parse("#0F172B")),
                IsFillSeries = true,
            },
            new PieSeries<double>
            {
                Values = new double[] { pressure },
                InnerRadius = 50,
                MaxRadialColumnWidth = 6,
                Fill = new SolidColorPaint(color),
                DataLabelsPosition = PolarLabelsPosition.ChartCenter,
                DataLabelsFormatter = _ => centerText,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            }
        };
    }
}