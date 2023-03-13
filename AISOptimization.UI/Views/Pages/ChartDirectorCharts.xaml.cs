using System;
using System.Windows.Controls;

using AISOptimization.Utils;


namespace AISOptimization.Views.Pages;

public partial class ChartDirectorCharts : IViewWithVM<ChartDirectorChartVM>, IDialogAware
{
    public ChartDirectorCharts(ChartDirectorChartVM vm, ChartDirectorSurface surface, ChartDirectorSurfaceProjection projection)
    {
        ViewModel = vm;
        InitializeComponent();
        surface.ViewModel = vm;
        projection.ViewModel = vm;
        Surface.Content = surface;
        Projection.Content = projection;
    }

    public ChartDirectorChartVM ViewModel { get; set; }
    public object Footer { get
    {
        var footer = Resources["Footer"] as StackPanel;

        footer.DataContext ??= this;

        return footer;
    }}
    public Action FinishInteraction { get; set; }
}

