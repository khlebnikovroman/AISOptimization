using System;
using System.Linq;
using System.Windows.Controls;

using AISOptimization.Utils;

using ChartDirector;


namespace AISOptimization.Views.Pages;

public partial class ChartDirectorSurfaceProjection : IViewWithVM<ChartDirectorChartVM>
{
    private ContourLayer contourLayer;
    private ChartDirectorChartVM _viewModel;

    public ChartDirectorSurfaceProjection()
    {
        InitializeComponent();
    }

    private void ViewModelOnOptimizationResultVmChanged(object? sender, EventArgs e)
    {
        drawChart(WPFChartViewer1);
    }
    private void drawChart(WPFChartViewer viewer)
        {
            // Create a XYChart object of size 575 x 525 pixels
            var c = new XYChart(800, 800);

            // Set the plotarea at (75, 30) and of size 450 x 450 pixels. Use semi-transparent black
            // (80000000) dotted lines for both horizontal and vertical grid lines
            var p = c.setPlotArea(75, 30, 700, 700, -1, -1, -1, c.dashLineColor(unchecked((int) 0xaf000000), Chart.DotLine), -1);
            
            c.xAxis().setTitle(ViewModel.X.Description, "Arial Bold", 12, Wpf.Ui.Appearance.Accent.SecondaryAccent.ToInt());
            c.yAxis().setTitle(ViewModel.Y.Description, "Arial Bold", 12, Wpf.Ui.Appearance.Accent.SecondaryAccent.ToInt());


            // Put the y-axis on the right side of the chart
            c.setYAxisOnRight();

            // Set x-axis and y-axis labels to use Arial Bold font
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // When auto-scaling, use tick spacing of 40 pixels as a guideline
            c.xAxis().setLinearScale(ViewModel.DataX.Min(), ViewModel.DataX.Max(), 1);
            c.yAxis().setLinearScale(ViewModel.DataY.Min(), ViewModel.DataY.Max(), 1);

            // Add a contour layer using the given data
            contourLayer = c.addContourLayer(ViewModel.DataX, ViewModel.DataY, ViewModel.DataZ);
            contourLayer.setContourLabelFormat("<*font=Arial Bold,size=10*>{value}<*/font*>");

            contourLayer.setZBounds(0);

            // Move the grid lines in front of the contour layer
            c.getPlotArea().moveGridBefore(contourLayer);

            // Add a vertical color axis at x = 0 at the same y-position as the plot area.
            var cAxis = contourLayer.setColorAxis(0, p.getTopY(), Chart.TopLeft,
                                                  p.getHeight(), Chart.Right);

            // Use continuous gradient coloring (as opposed to step colors)
            cAxis.setColorGradient(true);

            // Add a title to the color axis using 12 points Arial Bold Italic font
            cAxis.setTitle("Color Legend Title Place Holder", "Arial Bold Italic", 10);

            // Set color axis labels to use Arial Bold font
            cAxis.setLabelStyle("Arial", 10);

            // Set the chart image to the WinChartViewer
            viewer.Chart = c;

            // Tooltip for the contour chart
            viewer.ImageMap = c.getHTMLImageMap("", "",
                                                "title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");

            // Initializse the crosshair position to the center of the chart


            // Draw the cross section and crosshair
        }
    public ChartDirectorChartVM ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            _viewModel.OptimizationResultVmChanged += ViewModelOnOptimizationResultVmChanged;
        }
    }
}

